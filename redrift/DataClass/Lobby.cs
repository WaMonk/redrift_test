using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace redrift.DataClass
{
	public class Lobby : ILobby
	{
		
		public uint LobbyId { get; set; }
		public uint Owner { get; set; }
		public LobbyStatus Status { get; set; }

		public uint ?OpponentId { get; set; }
		public uint ?MatchId { get; set; }

		public Lobby(uint owner)
        {
			Owner = owner;
			Status = LobbyStatus.Pending;

		}

		public void Start(uint matchId)
        {
			Status = LobbyStatus.Battle;
			MatchId = matchId;
        }

		public void Finish()
        {
			Status = LobbyStatus.Finished;
        }
	}
}

