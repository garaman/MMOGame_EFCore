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

                /*
                string command =
                    @"  CREATE FUNCTION GetAverageReviewScore (@itemId INT) RETURNS FLOAT
                        AS
                        BEGIN
                        DECLARE @result AS FLOAT

                        SELECT @result = AVG(CAST([Score] AS FLOAT))
                        FROM ItemReview AS r
                        WHERE @itemId = r.ItemId

                        RETURN @result
                        END";

                db.Database.ExecuteSqlRaw(command);
                */

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

        public static void ShowItems()
        {
            using (AppDBContext db = new AppDBContext())
            {                
                foreach (var item in db.Items.Include(i => i.Owner).IgnoreQueryFilters().ToList())
                {
                    if (item.SoftDeleted)
                    {
                        Console.WriteLine($"DELETED - ItemId({item.ItemId}) TemplateId({item.TemplateId})");
                    }
                    else
                    {   
                        if (item.Owner == null)
                        {
                            Console.WriteLine($"ItemId({item.ItemId}) TemplateId({item.TemplateId}) Owner(0)");
                        }
                        else
                        {
                            Console.WriteLine($"ItemId({item.ItemId}) TemplateId({item.TemplateId}) OwnerId({item.Owner.PlayerId}) Owner({item.Owner.Name})");
                        }   
                    }
                }
            }
        }

        public static void ShowGuild()
        {
            using (AppDBContext db = new AppDBContext())
            {
                foreach (var guild in db.Guilds.Include(g => g.Members).ToList())
                {
                    Console.WriteLine($"GuildId({guild.GuildId}) GuildName({guild.GuildName}) MemberCount({guild.Members.Count})");
                }
            }
        }

        public static void TestUpdateAttach()
        {
            using (AppDBContext db = new AppDBContext())
            {
                { 
                    Player p = new Player() { Name = "StateTest"};
                    db.Entry(p).State = EntityState.Added;

                    db.SaveChanges();
                }

                {
                    Player p = new Player() 
                    { 
                        PlayerId = 3,
                        Name = "San_Se" 
                    };

                    p.OwnedItem = new Item() { TemplateId = 777 };
                    p.Guild = new Guild() { GuildName = "TrackGraph" };

                    db.ChangeTracker.TrackGraph(p, e =>
                    {
                        if (e.Entry.Entity is Player)
                        {
                            e.Entry.State = EntityState.Unchanged;
                            e.Entry.Property("Name").IsModified = true;
                        }
                        else if (e.Entry.Entity is Guild)
                        {
                            e.Entry.State = EntityState.Unchanged;
                        }
                        else if (e.Entry.Entity is Item)
                        {
                            e.Entry.State = EntityState.Unchanged;
                        }
                    });

                    db.SaveChanges();
                }


            }
        }
    }
}
