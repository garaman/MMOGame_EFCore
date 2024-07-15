using System.ComponentModel.DataAnnotations.Schema;

namespace MMOGame_EFCore
{
    // Entity 클래스
    [Table("Item")]
    public class Item
    {
        public int ItemId { get; set; }
        public int TemplateId { get; set; }
        public DateTime CreateTime { get; set; }

        // 다른 클래스 참조 -> FK (Navigational Property)
        [ForeignKey("OwnerId")]
        public Player Owner { get; set; }        
    }

    [Table("Player")]
    public class Player
    {
        public int PlayerId { get; set; }
        public string Name { get; set; }

        public Item Item { get; set; }
        public Guild Guild { get; set; }
    }

    [Table("Guild")]
    public class Guild
    {
        public int GuildId { get; set; }
        public string GuildName { get; set; }
        public ICollection<Player> Members { get; set; }
    }
}
