namespace MusicGroup.Api.Common.Models
{
    public sealed class Song
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Album Album { get; set; }
    }
}