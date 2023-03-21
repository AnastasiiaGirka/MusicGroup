using Microsoft.AspNetCore.Mvc;
using MusicGroup.Api.Common.Models;
using MusicGroup.Api.Common.Models.Requests;

namespace MusicGroup.WebUI.Server.Controllers
{
    public sealed class AlbumController : BaseWebApiController
    {
        public AlbumController(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        [HttpGet("GetAlbum")]
        public async Task<Album> GetAlbumAsync(Guid id)
        {
            return await AlbumApiClient.GetAlbumAsync(id);
        }
        
        [HttpDelete("DeleteAlbum")]
        public async Task<bool> DeleteAlbumAsync(Guid id)
        {
            return await AlbumApiClient.DeleteAlbumAsync(id);
        }
        
        [HttpGet("ListAlbums")]
        public async Task<Album[]> ListAlbumsAsync()
        {
            List<Album> result = await AlbumApiClient.ListAlbumsAsync();

            return result.ToArray();
        }

        [HttpPost("SaveAlbum")]
        public async Task<Album> SaveAlbumAsync(SaveAlbumRequest request)
        {
            return await AlbumApiClient.SaveAlbumAsync(request);
        }
    }
}