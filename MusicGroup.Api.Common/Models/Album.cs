namespace MusicGroup.Api.Common.Models
{
    public sealed class Album
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Song[] Songs { get; set; }
    }
}