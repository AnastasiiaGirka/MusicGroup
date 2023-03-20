using Microsoft.EntityFrameworkCore;
using MusicGroup.Api.Db.Context.Entities;
#pragma warning disable CS8602

namespace MusicGroup.Api.Db
{
    public sealed partial class DbService
    {
        public async Task SaveAlbumAsync(AlbumDbEntity album)
        {
            await  _context.AddAsync(album);
            
            await _context.SaveChangesAsync();
        }

        public async Task<AlbumDbEntity[]> ListAlbumsAsync()
        {
            return await _context
                .Albums
                .ToArrayAsync();
        }
        
        public async Task<AlbumDbEntity> GetAlbumAsync(Guid albumId)
        {
            AlbumDbEntity? album = await FindAlbumAsync(albumId);

            if (album == null)
                throw new ArgumentOutOfRangeException(nameof(albumId), $"Album with id \"{albumId}\" does not exist.");

            return album;
        }

        public async Task<AlbumDbEntity?> FindAlbumAsync(Guid id)
        {
            return await _context
                .Albums.FirstOrDefaultAsync(row => row.Id == id);
        }
    }
}