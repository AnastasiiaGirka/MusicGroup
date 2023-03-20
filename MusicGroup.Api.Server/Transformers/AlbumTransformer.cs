using MusicGroup.Api.Common.Models;
using MusicGroup.Api.Db.Context.Entities;

namespace MusicGroup.Server.Transformers
{
    public static class AlbumTransformer
    {
        public static Album Transform(this AlbumDbEntity from)
        {
            if (from == null)
            {
                return null;
            }

            var to = new Album
            {
                Id = from.Id,
                Name = from.Name,
                Songs = from.Songs.Transform()
            };

            return to;
        }

        public static Album[] Transform(this IEnumerable<AlbumDbEntity> from)
        {
            return from?
                .Select(Transform)
                .Where(item => item != null)
                .ToArray();
        }
    }
}