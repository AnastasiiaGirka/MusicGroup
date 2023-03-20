namespace MusicGroup.WebUI.Server.Models
{
    public sealed class ResponseContainer
    {
        public object Value { get; set; }

        public ServerError Error { get; set; }

        /// <summary>
        /// Command to React to re-create application context in case of session timeout
        /// </summary>
        public bool Unauthorized { get; set; }

        public bool IsResponseContainer => true;
    }
}