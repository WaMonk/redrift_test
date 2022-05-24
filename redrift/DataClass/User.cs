using System;
namespace redrift.DataClass
{
	public class User
	{

		public User(string login, string password)
        {
			Login = login;
			Password = password;

        }

		public uint UserId { get; set; }
        public string Login { get; set; }
		public string Password { get; set; }
		public string ?Token { get; set; }
	}
}

