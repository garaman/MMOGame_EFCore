using System.Runtime.CompilerServices;

namespace MMOGame_EFCore
{
    public static class Extensions
    {
        public static IQueryable<GuildDto> MapGuildToDto(this IQueryable<Guild> guild)
        {
            return guild.Select(g => new GuildDto()
            {
                GuildId = g.GuildId,
                Name = g.GuildName,
                MemberCount = g.Members.Count
            });
        }
    }
}
