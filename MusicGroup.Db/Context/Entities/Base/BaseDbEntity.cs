using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicGroup.Api.Db.Context.Entities.Base
{
    public abstract class BaseDbEntity
    {
        protected BaseDbEntity()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
    }
}