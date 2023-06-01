using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using Bepinject;
using HarmonyLib;
using Utilla;
using System.IO;
using UnityEngine;

namespace TextureLoader
{
    [BepInPlugin(GUID, NAME, VERSION), BepInDependency("org.legoandmars.gorillatag.utilla"), BepInDependency("tonimacaroni.computerinterface"), BepInDependency("dev.auros.bepinex.bepinject")]
    [ModdedGamemode]
    internal class Main : BaseUnityPlugin
    {
        internal const string
            GUID = "crafterbot.dumbmonkegame.textureloader",
            NAME = "TextureLoader",
            VERSION = "1.0.1";
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

#if DEBUG

        private int SelectedTexture;
        private void OnGUI()
        {
            GUILayout.Label(NAME);
            GUILayout.Label(VERSION);

            if (GUILayout.Button("Reset textures"))
                TextureController.ResetTextures();

            GUILayout.Label($"RoomModded: {RoomModded}");
            GUILayout.Label("Selected Texture:" + Utilities.GetAllFileNamesInDirectory(Utilities.AddonsDirectoryPath, "*.texture")[SelectedTexture]);
            if (GUILayout.Button("Next texture"))
                SelectedTexture++;
            if (GUILayout.Button("Previous texture"))
                SelectedTexture--;

            if (GUILayout.Button("Load texture"))
            {
                TextureController.LoadTexture(Directory.GetFiles(Utilities.AddonsDirectoryPath, "*.texture")[SelectedTexture], TextureController.GetPackage(Directory.GetFiles(Utilities.AddonsDirectoryPath, "*.texture")[SelectedTexture]));
                $"Loaded texturepack:{Utilities.GetAllFileNamesInDirectory(Utilities.AddonsDirectoryPath, "*.texture")[SelectedTexture]}".Log();
            }

            GUILayout.Label("");
            GUILayout.Label("");

            GUILayout.Label("Go To Private");
            const string RoomID = "crafterbot";
            if (GUILayout.Button("Go to private"))
                Utilla.Utils.RoomUtils.JoinPrivateLobby(RoomID);
        }
#endif
    }
}

