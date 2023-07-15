using BepInEx.Configuration;

namespace GorillaTextureLoader
{
    internal static class Configuration
    {
        internal static ConfigEntry<bool> LoadOnStartup;
        internal static ConfigEntry<string> LoadOnStartupKey;

        internal static void Init(ConfigFile Config)
        {
            LoadOnStartup = Config.Bind("General", "LoadOnStartup", false, "Load the texture pack on startup");
            LoadOnStartupKey = Config.Bind("General", "LoadOnStartupKey", "", "The texture pack to load on startup");
        }
    }
}
