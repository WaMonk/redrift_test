using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using redrift.DataClass;
using redrift.DB;
using redrift.SignalR.Hubs;

namespace RedRift.Controllers
{
	[ApiController]
	[Route("api/lobby")]
	public class LobbyController : Controller
	{

		public IHubContext<MainHub> _hubContext { get; }

		public LobbyController(IHubContext<MainHub> hubContext)
		{
			_hubContext = hubContext;
		}

		[HttpGet]
		[Route("read")]
        public IEnumerable<ILobby> Read()
        {

			var db = new Context();
			return db.Lobbies.Where(l => l.Status != LobbyStatus.Finished).ToList();
		}

		[HttpPost]
		[Route("create")]
		public ActionResult Create(string token)
		{

			var db = new Context();
			var user = db.Users.Where(u => u.Token == token).FirstOrDefault();
			if (user is null){
				Console.WriteLine($"User with token {token} is not found");
				return new StatusCodeResult(StatusCodes.Status500InternalServerError);
			}

			var activeLobby = db.Lobbies.Where(l => ((l.Owner == user.UserId || l.OpponentId == user.UserId) && l.Status != LobbyStatus.Finished)).FirstOrDefault();
			if (activeLobby is not null)
            {
				return new StatusCodeResult(StatusCodes.Status500InternalServerError);
			}

			ILobby lobby = new Lobby(user.UserId);
			db.Lobbies.Add((Lobby)lobby);
			db.SaveChanges();
			
			return new JsonResult(lobby);
		}

        [HttpPost]
        [Route("join")]
        public async Task<ActionResult> Join(uint lobbyId, string token)
        {
			var db = new Context();
			var user = db.Users.Where(u => u.Token == token).FirstOrDefault();
			if (user is null)
			{
				Console.WriteLine($"User with token {token} is not found");
				return new StatusCodeResult(StatusCodes.Status500InternalServerError);
			}

			var lobby = db.Lobbies.Find(lobbyId);
			if (lobby is null)
            {
				Console.WriteLine($"Lobby with id {lobbyId} is not found");
				return new StatusCodeResult(StatusCodes.Status500InternalServerError);
			}

            if (lobby.Status != LobbyStatus.Pending)
            {
                Console.WriteLine($"Lobby with id {lobbyId} is have wrong status");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            if (lobby.Owner == user.UserId)
            {
                Console.WriteLine($"Lobby with id {lobbyId} user is owner");
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            lobby.OpponentId = user.UserId;

			IMatch match = new Match(lobby.Owner, user.UserId);
			db.Matches.Add((Match) match);
			db.SaveChanges();

			lobby.Start(match.MatchId);
			db.Lobbies.Update(lobby);
			db.SaveChanges();

			var owner = db.Users.FirstOrDefault(u => u.UserId == lobby.Owner);
			if (owner is not null && owner.Token is not null)
            {
				Console.WriteLine($"SendUpdate {owner.Token}");
				await _hubContext.Clients.Group(owner.Token).SendAsync("LobbyUpdate", "join_user");
			}
			
			return new OkResult();
        }

		[HttpDelete]
		[Route("delete")]
		public ActionResult Delete(string token)
        {
			var db = new Context();
			var user = db.Users.Where(u => u.Token == token).FirstOrDefault();
			if (user is null)
			{
				Console.WriteLine($"User with token {token} is not found");
				return new StatusCodeResult(StatusCodes.Status500InternalServerError);
			}

			var lobby = db.Lobbies.Where(l => (l.Owner == user.UserId)).FirstOrDefault();
			if (lobby is null)
			{
				return new StatusCodeResult(StatusCodes.Status500InternalServerError);
			}

			db.Lobbies.Remove(lobby);
			db.SaveChanges();

			return new OkResult();
		}
    }
}

