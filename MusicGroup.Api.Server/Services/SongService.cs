using System.Diagnostics.CodeAnalysis;
using MusicGroup.Api.Common.Models;
using MusicGroup.Api.Common.Models.Requests;
using MusicGroup.Api.Db.Context.Entities;
using MusicGroup.Server.Transformers;

namespace MusicGroup.Server.Services
{
    public sealed class SongService : BaseService
    {
        public SongService(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        public async Task<Song> GetSongAsync(Guid songId)
        {
            SongDbEntity song = await DbService.GetSongAsync(songId);

            return song.Transform();
        }
        
        public async Task<Song[]> ListSongsAsync()
        {
            SongDbEntity[] songs = await DbService.ListSongsAsync();

            return songs.Transform();
        }

        public async Task<Song> SaveSongAsync([NotNull] SaveSongRequest request)
        {
            SongDbEntity song;

            if (request.Id == null)
            {
                song = new SongDbEntity
                {
                    Name = request.Name
                };
            }
            else
            {
                song = await DbService.GetSongAsync(request.Id.Value);

                song.Name = request.Name;
            }

            await DbService.SaveSongAsync(song);

            return song.Transform();
        }
    }
}