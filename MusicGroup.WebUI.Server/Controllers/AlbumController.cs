using Microsoft.AspNetCore.Authorization;
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

        [AllowAnonymous]
        [HttpGet("GetAlbum")]
        public async Task<Album> GetAlbumAsync(Guid Id)
        {
            return await AlbumApiClient.GetAlbumAsync(Id);
        }

        [AllowAnonymous]
        [HttpGet("ListAlbums")]
        public async Task<Album[]> ListAlbumsAsync()
        {
            List<Album> result = await AlbumApiClient.ListAlbumsAsync();

            return result.ToArray();
        }

        [AllowAnonymous]
        [HttpPost("SaveAlbum")]
        public async Task<Album> SaveAlbumAsync(SaveAlbumRequest request)
        {
            return await AlbumApiClient.SaveAlbumAsync(request);
        }
    }
}