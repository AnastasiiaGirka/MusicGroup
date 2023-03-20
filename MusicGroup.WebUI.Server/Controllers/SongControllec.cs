using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicGroup.Api.Common.Models;
using MusicGroup.Api.Common.Models.Requests;

namespace MusicGroup.WebUI.Server.Controllers
{
    public sealed class SongController : BaseWebApiController
    {
        public SongController(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        [AllowAnonymous]
        [HttpGet("GetSong")]
        public async Task<Song> GetSongAsync(Guid id)
        {
            return await SongApiClient.GetSongAsync(id);
        }

        [AllowAnonymous]
        [HttpGet("ListSongs")]
        public async Task<Song[]> ListSongsAsync()
        {
            List<Song> result = await SongApiClient.ListSongsAsync();

            return result.ToArray();
        }

        [AllowAnonymous]
        [HttpPost("SaveSong")]
        public async Task<Song> SaveSongAsync(SaveSongRequest request)
        {
            return await SongApiClient.SaveSongAsync(request);
        }
    }
}