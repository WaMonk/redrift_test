using System;
namespace redrift.DataClass
{
	public enum LobbyStatus { Pending = 0, Battle = 1, Finished = 2 };
	
	public interface ILobby
	{

		public uint LobbyId { get; set; }
		public uint Owner { get; set; }
		public LobbyStatus Status { get; }

		public uint ?OpponentId { get; set; }
		public uint ?MatchId { get; set; }

		public void Start(uint matchId);
		public void Finish();



	}
}

