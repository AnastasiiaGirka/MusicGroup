using MusicGroup.Common.Helpers;

namespace MusicGroup;

public class Program
{
    public static void Main(string[] args)
    {
        ProgramHelper.Start<Startup>(args);
    }

    //Do not delete, EF is needed it
    public static IWebHostBuilder? CreateWebHostBuilder(string[] args) => ProgramHelper.CreateWebHostBuilder<Startup>(args);
}