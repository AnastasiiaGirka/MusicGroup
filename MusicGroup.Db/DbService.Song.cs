using Microsoft.EntityFrameworkCore;
using MusicGroup.Api.Db.Context.Entities;
#pragma warning disable CS8602

namespace MusicGroup.Api.Db
{
    public sealed partial class DbService
    {
        public async Task SaveSongAsync(SongDbEntity song)
        {
            await  _context.AddAsync(song);
            
            await _context.SaveChangesAsync();
        }

        public async Task<SongDbEntity[]> ListSongsAsync()
        {
            return await _context
                .Songs
                .ToArrayAsync();
        }
        
        public async Task<SongDbEntity> GetSongAsync(Guid songId)
        {
            SongDbEntity? song = await FindSongAsync(songId);

            if (song == null)
                throw new ArgumentOutOfRangeException(nameof(songId), $"Song with id \"{songId}\" does not exist.");

            return song;
        }

        public async Task<SongDbEntity?> FindSongAsync(Guid id)
        {
            return await _context
                .Songs.FirstOrDefaultAsync(row => row.Id == id);
        }
    }
}