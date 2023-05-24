using BepInEx;
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
            VERSION = "0.0.3",
            PLAYERPREFS_ONLOADKEY = "textureloaderloadtextureongameloadtexturepath",
            PLAYERPREFS_ONLOAD_TOGGLED = "textureloaderloadtextureonload";
        internal static bool RoomModded;
        internal static ManualLogSource manualLogSource;
        internal Main()
        {
            manualLogSource = Logger;
            $"Init : {GUID}".Log();
            Utilities.AssemblyDirectoryPath.Log();

            Zenjector.Install<computerInteface.MainInstaller>().OnProject();
            new Harmony(GUID).PatchAll();
        }

        [ModdedGamemodeJoin]
        private void OnJoin()
        {
            RoomModded = true;
        }
        [ModdedGamemodeLeave]
        private void OnLeft()
        {
            RoomModded = false;
        }
    }
}

