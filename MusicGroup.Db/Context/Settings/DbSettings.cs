using Microsoft.Extensions.Options;

namespace MusicGroup.Api.Db.Context.Settings;

public sealed class DbSettings : IOptions<DbSettings>
{
    DbSettings IOptions<DbSettings>.Value => this;
}