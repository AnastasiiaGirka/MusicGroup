namespace MusicGroup.Api.Common.Models.Requests;

public class SaveSongRequest
{
    public Guid? Id { get; set; }
    
    public string Name { get; set; }
}