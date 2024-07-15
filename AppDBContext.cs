using Microsoft.EntityFrameworkCore;

namespace MMOGame_EFCore
{  
    public class AppDBContext : DbContext
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<Player> players { get; set; }
        public DbSet<Guild> Guilds { get; set; }


        public const string ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EFcoreDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(ConnectionString);
        }
    }
}
