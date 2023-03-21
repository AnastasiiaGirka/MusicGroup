using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MusicGroup.Api.Common.Models;
using MusicGroup.Api.Common.Models.Requests;
using MusicGroup.Server.Services;

namespace MusicGroup.Server.Controllers
{
    [AllowAnonymous]
    public sealed class AlbumController : BaseController
    {
        private readonly AlbumService _albumService;
        
        public AlbumController(AlbumService albumService)
        {
            _albumService = albumService ?? throw new ArgumentNullException(nameof(albumService));
        }

        [HttpGet]
        [Route("GetAlbum")]
        public Task<Album> GetAlbumAsync([BindRequired][FromQuery] Guid id)
        {
            return _albumService.GetAlbumAsync(id);
        }
        
        [HttpGet]
        [Route("ListAlbums")]
        [ProducesResponseType(200, Type = typeof(Album[]))]
        public Task<Album[]> ListAlbumsAsync()
        {
            return _albumService.ListAlbumsAsync();
        }

        [HttpPost]
        [Route("SaveAlbum")]
        [ProducesResponseType(200, Type = typeof(Album))]
        public async Task<Album> SaveAlbumAsync([FromBody] SaveAlbumRequest requests)
        {
            return await _albumService.SaveAlbumAsync(requests);
        }
        
        [HttpDelete]
        [Route("DeleteAlbum")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public async Task<bool> DeleteAlbumAsync([FromBody] Guid id)
        {
            return await _albumService.DeleteAlbumAsync(id);
        }
    }
}