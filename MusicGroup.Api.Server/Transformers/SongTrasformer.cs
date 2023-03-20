using MusicGroup.Api.Common.Models;
using MusicGroup.Api.Db.Context.Entities;

namespace MusicGroup.Server.Transformers
{
    public static class SongTransformer
    {
        public static Song Transform(this SongDbEntity from)
        {
            if (from == null)
            {
                return null;
            }

            var to = new Song
            {
                Id = from.Id,
                Name = from.Name
            };

            return to;
        }

        public static Song[] Transform(this IEnumerable<SongDbEntity> from)
        {
            return from?
                .Select(Transform)
                .Where(item => item != null)
                .ToArray();
        }
    }
}