using System;
namespace redrift.DataClass
{
	public interface IMatchMember
	{
		public uint UID { get; }
		public uint hp { get; }

		public bool IsAlive();
		public void TakeDamage(uint damage);

	}
}

