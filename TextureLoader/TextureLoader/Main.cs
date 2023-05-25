using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using Bepinject;
using HarmonyLib;
using Utilla;

namespace TextureLoader
{
    [BepInPlugin(GUID, NAME, VERSION), BepInDependency("org.legoandmars.gorillatag.utilla"), BepInDependency("tonimacaroni.computerinterface"), BepInDependency("dev.auros.bepinex.bepinject")]
    [ModdedGamemode]
    internal class Main : BaseUnityPlugin
    {
        internal const string
            GUID = "crafterbot.dumbmonkegame.textureloader",
            NAME = "TextureLoader",
            VERSION = "1.0.0";
        internal static bool RoomModded;
        internal static ManualLogSource manualLogSource;

        #region Config
        internal static ConfigEntry<bool> LoadOnStartup;
        internal static ConfigEntry<string> LoadOnStartupKey;
        #endregion

        internal Main()
        {
            manualLogSource = Logger;
            $"Init : {GUID}".Log();

            #region Config
            LoadOnStartup = Config.Bind("General", "LoadOnStartup", false, "Load the texture pack on startup");
            LoadOnStartupKey = Config.Bind("General", "LoadOnStartupKey", "", "The texture pack to load on startup");
            #endregion

            Zenjector.Install<computerInteface.MainInstaller>().OnProject();
            new Harmony(GUID).PatchAll();
        }

        [ModdedGamemodeJoin]
        private void OnJoin() =>
            RoomModded = true;
        [ModdedGamemodeLeave]
        private void OnLeft()
        {
            RoomModded = false;
            if (TextureController.GetTextureLoaded())
                if (!TextureController.LoadedPackage.IsVerified)
                    TextureController.ResetTextures();
        }
    }
}

