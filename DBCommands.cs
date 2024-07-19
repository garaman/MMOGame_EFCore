﻿using Microsoft.EntityFrameworkCore;
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

        public static void ShowItems()
        {
            using (AppDBContext db = new AppDBContext())
            {
                foreach(var item in db.Items.Include(i=>i.Owner).ToList())
                {
                    if(item.SoftDeleted == false)
                    {
                        Console.WriteLine($"DELETED - ItemId({item.ItemId}), TemplateId({item.TemplateId}) , Owner(0)");
                    }
                    else
                    {
                        if (item.Owner == null)
                        {
                            Console.WriteLine($"ItemId({item.ItemId}), TemplateId({item.TemplateId}) , Owner(0)");
                        }
                        else
                        {
                            Console.WriteLine($"ItemId({item.ItemId}), TemplateId({item.TemplateId}) , OwnerId({item.Owner.PlayerId}), Owner({item.Owner.Name})");
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
                    Console.WriteLine($"GuildId({guild.GuildId}), GuildName({guild.GuildName}) , MembersCount({guild.Members.Count})");
                }
            }
        }
 
    }
}
