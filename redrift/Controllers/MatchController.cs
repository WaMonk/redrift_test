using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using redrift.DataClass;
using redrift.DB;
using redrift.SignalR.Hubs;

namespace RedRift.Controllers
{
	[ApiController]
	[Route("api/match")]
	public class MatchController : Controller
	{

		public IHubContext<MainHub> _hubContext { get; }

		public MatchController(IHubContext<MainHub> hubContext)
		{
			_hubContext = hubContext;
		}

		[HttpGet]
		[Route("read")]
		public IEnumerable<IMatch> Read()
		{

			var db = new Context();
			return db.Matches.ToList();
		}

		[HttpPost]
		[Route("damage")]
		public async Task<ActionResult> Damage(uint matchId, string token, uint value)
        {

			var db = new Context();

			var match = db.Matches.Find(matchId);
			if (match is null)
			{
				Console.WriteLine($"Match with id {matchId} is not found");
				return new StatusCodeResult(StatusCodes.Status500InternalServerError);
			}

			var user = db.Users.Where(u => u.Token == token).FirstOrDefault();
			if (user is null)
			{
				Console.WriteLine($"User with token {token} is not found");
				return new StatusCodeResult(StatusCodes.Status500InternalServerError);
			}

			if (!match.HasPlayer(user.UserId)) {
				Console.WriteLine($"User with token {token} is not match member");
				return new StatusCodeResult(StatusCodes.Status500InternalServerError);
			}

			var whoTaked = user.UserId == match.PlayerOneId ? match.PlayerTwoId : match.PlayerOneId;

			match.TakeDamage(whoTaked, value);


			var userTaked = db.Users.FirstOrDefault(u => u.UserId == whoTaked);
			if (userTaked is not null && userTaked.Token is not null)
			{
				await _hubContext.Clients.Group(userTaked.Token).SendAsync("MatchUpdate", "take_damage");
			}

			if (match.IsMatchFinished())
            {
				var lobby = db.Lobbies.Where(l => l.MatchId == match.MatchId).FirstOrDefault();
				if (lobby is null)
                {
					Console.WriteLine($"Broken match with id {matchId}");
					return new StatusCodeResult(StatusCodes.Status500InternalServerError);
				}

				lobby.Finish();

				if (userTaked is not null && userTaked.Token is not null)
				{
					await _hubContext.Clients.Group(userTaked.Token).SendAsync("MatchUpdate", "match_finished");
				}
			}

			db.SaveChanges();

			return new OkResult();
        }
	}
}

