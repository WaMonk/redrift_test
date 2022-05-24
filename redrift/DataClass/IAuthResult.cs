using System;
namespace redrift.DataClass
{
	public interface IAuthResult
	{
		public uint UserId { get; }
		public string Token { get; }
	}
}

