using System.ComponentModel.DataAnnotations;
using MusicGroup.Api.Db.Context.Entities.Base;
using MusicGroup.Common;

namespace MusicGroup.Api.Db.Context.Entities
{
    public sealed class AlbumDbEntity : BaseDbEntity
    {
        [Required]
        [MaxLength(Constants.Db.KeyLength)]
        public string Name { get; set; }
        
        public ICollection<SongDbEntity> Songs { get; set; }
    }
}