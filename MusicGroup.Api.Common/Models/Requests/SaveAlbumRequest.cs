namespace MusicGroup.Api.Common.Models.Requests;

public class SaveAlbumRequest
{
    public Guid? Id { get; set; }

    public string Name { get; set; }
}