using Microsoft.AspNetCore.SignalR;

namespace redrift.SignalR.Hubs
{
	public class MainHub : Hub
	{

		private readonly static Dictionary<string,string> _connections = new Dictionary<string, string>();

		public override async Task OnDisconnectedAsync(Exception? exception)
		{
            if (_connections.TryGetValue(Context.ConnectionId, out string row))
            {
                _connections.Remove(Context.ConnectionId);
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, row);
            }

            await base.OnDisconnectedAsync(exception);
		}

		public async Task Authorize(string token)
        {
			_connections[Context.ConnectionId] = token;
			await Groups.AddToGroupAsync(Context.ConnectionId, token);
			Console.WriteLine($"[MainHub]: Authorize {token} -- {Context.ConnectionId}");
			await Clients.Client(Context.ConnectionId).SendAsync("AuthorizeOK");
		}
	}
}

