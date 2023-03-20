using System.Reflection;

namespace MusicGroup.Api.Common
{
    public abstract class BaseApiClient
    {
        public static Type[] GetClientTypes()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            Type[] types = assemblies.SelectMany(assembly => assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(BaseApiClient)))).ToArray();
            return types;
        }
    }
}