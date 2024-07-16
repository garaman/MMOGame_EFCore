using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Numerics;

namespace MMOGame_EFCore
{
    public class DBCommands
    {
        public static void InitializeDB(bool forceReset = false)
        {
            using (AppDBContext db = new AppDBContext())
            {
                if (!forceReset && (db.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists())
                {
                    return;
                }

                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                CreateTestData(db);
                Console.WriteLine("DB Initialized");
            }
        }

        // CRUD (Create-Read-Update-Delete)
        public static void CreateTestData(AppDBContext db)
        {
            var Bada = new Player() { Name = "Bada" };
            var Gang = new Player() { Name = "Gang" };
            var San = new Player() { Name = "San" };

            List<Item> items = new List<Item>()
            {
                new Item()
                {
                    TemplateId = 101,
                    CreateTime = DateTime.Now,
                    Owner = Bada
                },
                new Item()
                {
                    TemplateId = 102,
                    CreateTime = DateTime.Now,
                    Owner = Gang
                },
                new Item()
                {
                    TemplateId = 103,
                    CreateTime = DateTime.Now,
                    Owner = San
                }
            };

            Guild guild = new Guild()
            {
                GuildName = "자연",
                Members = new List<Player>() { Bada, Gang, San }
            };

            db.Items.AddRange(items);
            db.Guilds.AddRange(guild);
            db.SaveChanges();
        }

        // Update 3단계
        // 1) Tracked Entity를 얻어온다.
        // 2) Entity 클래스의 property를 변경(set)
        // 3) SaveChangs 호출.

        public static void UpdateTest()
        {
            using(AppDBContext db = new AppDBContext())
            {
                var guild = db.Guilds.Single(g => g.GuildName == "자연");

                guild.GuildName = "자연별곡";

                db.SaveChanges(); 
            }
        }

        public static void EagerLoading()
        {
            Console.WriteLine("길드 이름을 입력하세요");
            Console.WriteLine("> ");
            string name = Console.ReadLine();

            using (var db = new AppDBContext())
            {
                Guild guild = db.Guilds.AsNoTracking().Where(g => g.GuildName == name).Include(g => g.Members).ThenInclude(p => p.Item).First();
                foreach (Player player in guild.Members)
                {
                    Console.WriteLine($"TemplateId({player.Item.TemplateId}) / Owner({player.Name})");
                }
            }
        }

        public static void ExplicitLoading()
        {
            Console.WriteLine("길드 이름을 입력하세요");
            Console.WriteLine("> ");
            string name = Console.ReadLine();

            using (var db = new AppDBContext())
            {
                Guild guild = db.Guilds.Where(g => g.GuildName == name).First();

                // 명시적
                db.Entry(guild).Collection(g => g.Members).Load();
                foreach (Player player in guild.Members)
                {
                    db.Entry(player).Reference(p => p.Item).Load();
                }

                foreach (Player player in guild.Members)
                {
                    Console.WriteLine($"TemplateId({player.Item.TemplateId}) / Owner({player.Name})");
                }
            }
        }

        public static void SelectLoading()
        {
            Console.WriteLine("길드 이름을 입력하세요");
            Console.WriteLine("> ");
            string name = Console.ReadLine();

            using (var db = new AppDBContext())
            {
                var info = db.Guilds.Where(g => g.GuildName == name)
                    .MapGuildToDto()
                    .First();

                Console.WriteLine($"GuildName({info.Name}) / MenberCount({info.MemberCount})");
            }
        }

    }
}
