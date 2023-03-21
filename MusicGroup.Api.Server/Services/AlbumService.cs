using System.Diagnostics.CodeAnalysis;
using MusicGroup.Api.Common.Models;
using MusicGroup.Api.Common.Models.Requests;
using MusicGroup.Api.Db.Context.Entities;
using MusicGroup.Server.Transformers;

namespace MusicGroup.Server.Services
{
    public sealed class AlbumService : BaseService
    {
        public AlbumService(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        public async Task<Album> GetAlbumAsync(Guid albumId)
        {
            AlbumDbEntity album = await DbService.GetAlbumAsync(albumId);

            return album.Transform();
        }

        public async Task<Album[]> ListAlbumsAsync()
        {
            AlbumDbEntity[] albums = await DbService.ListAlbumsAsync();

            AlbumDbEntity[] items = FileDb.List<AlbumDbEntity>();
            
            return albums.Transform();
        }

        public async Task<Album> SaveAlbumAsync([NotNull] SaveAlbumRequest request)
        {
            AlbumDbEntity album;

            if (request.Id == null)
            {
                album = new AlbumDbEntity
                {
                    Name = request.Name
                };
                
                await DbService.SaveAlbumAsync(album);
            }
            else
            {
                album = await DbService.GetAlbumAsync(request.Id.Value);

                album.Name = request.Name;
            }

            await DbService.SaveChangesAsync();

            FileDb.Insert(album);
            
            return album.Transform();
        }
        
        public async Task<bool> DeleteAlbumAsync(Guid id)
        {
            AlbumDbEntity album = await DbService.GetAlbumAsync(id);
            
            return await DbService.DeleteAlbumAsync(album);
        }
    }
}