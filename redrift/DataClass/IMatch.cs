using System;
namespace redrift.DataClass
{
	public interface IMatch
	{
		public uint MatchId { get; set; }

		public uint PlayerOneId { get; set; }
		public uint PlayerTwoId { get; set; }

		public uint PlayerOneHP { get; set; }
		public uint PlayerTwoHP { get; set; }

		public void TakeDamage(uint player, uint value);
		public bool HasPlayer(uint player);
		public bool IsMatchFinished();
	}
}

