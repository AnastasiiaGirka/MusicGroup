namespace MusicGroup.Common;

public static class Constants
{
    /// <summary>
    /// "MusicGroup"
    /// </summary>
    public const string ApplicationName = @"MusicGroup";
    
    /// <summary>
    /// "MusicGroup"
    /// </summary>
    public const string ApplicationInMemoryDatabaseName = @"MusicGroupDb";
    
    public static class Http
    {
        /// <summary>
        /// "application/json"
        /// </summary>
        public const string ApiContextType = "application/json";
    }
    
    public const string XsrfTokenName = @"xsrf-token";

    public static class Db
    {
        /// <summary>
        /// "255" (Email, Name, Key, etc.)
        /// </summary>
        public const int KeyLength = 255;

        /// <summary>
        /// "1024"
        /// </summary>
        public const int DescriptionLength = 1024;

        /// <summary>
        /// "FileDbFolder"
        /// </summary>
        public const string FileDbDirectory = "/Users/oleksandrnefodov/Documents/DB";
    }
}