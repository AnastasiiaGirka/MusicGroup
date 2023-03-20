using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MusicGroup.Api.Common.Models;
using MusicGroup.Api.Common.Models.Requests;
using MusicGroup.Server.Services;

namespace MusicGroup.Server.Controllers
{
    [AllowAnonymous]
    public sealed class SongController : BaseController
    {
        private readonly SongService _songService;
        
        public SongController(SongService songService)
        {
            _songService = songService ?? throw new ArgumentNullException(nameof(songService));
        }

        [HttpGet]
        [Route("GetSong")]
        [ProducesResponseType(200, Type = typeof(Song))]
        public Task<Song> GetSongAsync([BindRequired][FromQuery] Guid id)
        {
            return _songService.GetSongAsync(id);
        }
        
        [HttpGet]
        [Route("ListSongs")]
        [ProducesResponseType(200, Type = typeof(Song[]))]
        public Task<Song[]> ListSongsAsync()
        {
            return _songService.ListSongsAsync();
        }

        [HttpPost]
        [Route("SaveSong")]
        [ProducesResponseType(200, Type = typeof(Song))]
        public async Task<Song> SaveSongAsync([FromBody] SaveSongRequest requests)
        {
            return await _songService.SaveSongAsync(requests);
        }
    }
}