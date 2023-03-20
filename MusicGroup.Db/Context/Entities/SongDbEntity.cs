
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MusicGroup.Api.Db.Context.Entities.Base;
using MusicGroup.Common;

namespace MusicGroup.Api.Db.Context.Entities
{
    public sealed class SongDbEntity : BaseDbEntity
    {
        [Required]
        [MaxLength(Constants.Db.KeyLength)]
        public string Name { get; set; }
        
        public Guid AlbumId { get; set; }

        [ForeignKey(nameof(AlbumId))]
        public AlbumDbEntity Album { get; set; }
    }
}