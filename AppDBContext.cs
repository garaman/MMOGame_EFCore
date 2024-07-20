using Microsoft.EntityFrameworkCore;

namespace MMOGame_EFCore
{
    // EF Core 작동 스텝
    // 1) DbContext 만들 때
    // 2) DbSet<T>을 찾는다
    // 3) 모델링 class 분석해서, 칼럼을 찾는다
    // 4) 모델링 class에서 참조하는 다른 class가 있으면, 걔도 분석한다
    // 5) OnModelCreating 함수 호출 (추가 설정 = override)
    // 6) 데이터베이스의 전체 모델링 구조를 내부 메모리에 들고 있음

    public class AppDBContext : DbContext
    {
        public DbSet<Item> Items { get; set; }
        // TPH
        //public DbSet<EventItem> EventItems { get; set; }

        public DbSet<Player> players { get; set; }
        public DbSet<Guild> Guilds { get; set; }


        public const string ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EFcoreDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // 앞으로 Item Entity에 접근할 때 항상 사용되는 모델 레벨의 필터링
            // 필터를 무시하고 싶으면 IgnoreQueryFilters 옵션 추가
            builder.Entity<Item>().HasQueryFilter(i => i.SoftDeleted == false);

            builder.Entity<Player>()
                .HasIndex(p => p.Name)
                .HasName("Index_Person_Name")
                .IsUnique();

        }
    }
}
