using Microsoft.EntityFrameworkCore;
using redrift.DataClass;

namespace redrift.DB
{
	public class Context : DbContext
	{

        public string DbPath { get; }
        public DbSet<User> Users { get; set; }
        public DbSet<Lobby> Lobbies { get; set; }
        public DbSet<Match> Matches { get; set; }

        public Context()
		{
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "database.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");

        
	}
}

