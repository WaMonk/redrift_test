using System;
namespace redrift.DataClass
{

	public class Match : IMatch
	{

		private const uint DefaultHP = 10;
		
		public uint MatchId { get; set; }
		public uint PlayerOneId { get; set; }
		public uint PlayerTwoId { get; set; }

		public uint PlayerOneHP { get; set; }
		public uint PlayerTwoHP { get; set; }

		public Match()
		{

		}

		public Match(uint playerOne, uint playerTwo, uint HP = DefaultHP )
        {
			PlayerOneId = playerOne;
			PlayerOneHP = HP;

			PlayerTwoId = playerTwo;
			PlayerTwoHP = HP;
        }

		public bool HasPlayer(uint player)
        {
			return PlayerOneId == player || PlayerTwoId == player ? true : false;
        }

		public bool IsMatchFinished()
        {
			return PlayerOneHP == 0 || PlayerTwoHP == 0 ? true : false;
        }


		public void TakeDamage(uint player, uint value)
        {

			if (player == PlayerOneId)
            {
				PlayerOneHP = _takeDamage(PlayerOneHP, value);
            }else
            {
				PlayerTwoHP = _takeDamage(PlayerTwoHP, value);
			} 
        }

		private uint _takeDamage(uint current, uint dmg)
        {
			if (current < dmg)
            {
				dmg = current;
            }

			return current - dmg;
        }
	}
}

