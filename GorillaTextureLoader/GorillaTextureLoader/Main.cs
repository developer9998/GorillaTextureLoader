/*
    Most of the code here is pretty old, and I am not going to clean it up too much.
*/
using BepInEx;
using Bepinject;
using GorillaTextureLoader;
using System.IO;
using TextureLoader.Models;
using UnityEngine;
using Utilla;

namespace TextureLoader
{
    [BepInPlugin("crafterbot.dumbmonkegame.textureloader", "TextureLoader", "1.0.2"), BepInDependency("org.legoandmars.gorillatag.utilla"), BepInDependency("tonimacaroni.computerinterface"), BepInDependency("dev.auros.bepinex.bepinject")]
    [ModdedGamemode]
    internal class Main : BaseUnityPlugin
    {
        internal static Main Instance;

        internal bool RoomModded;

        internal Main()
        {
            Instance = this;
            Configuration.Init(Config);

            Utilla.Events.GameInitialized += OnGameInitialized;
            Zenjector.Install<computerInteface.MainInstaller>().OnProject();
            // new Harmony(Info.Metadata.GUID).PatchAll(typeof(Patches));
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

        private void OnGameInitialized(object sender, object args)
        {
            if (File.Exists(Configuration.LoadOnStartupKey.Value) && Configuration.LoadOnStartup.Value)
            {
                Package package = TextureController.GetPackage(Configuration.LoadOnStartupKey.Value);
                if (package.IsVerified)
                {
                    $"Loading texturepack:{package.Name}".Log();
                    TextureController.LoadTexture(Configuration.LoadOnStartupKey.Value, package);
                    "Loaded texturepack".Log(BepInEx.Logging.LogLevel.Message);
                }
                else
                    $"Uh oh, you can only load verified textures on startup:/".Log(BepInEx.Logging.LogLevel.Fatal);
            }
        }

#if DEBUG

        private int SelectedTexture;
        private void OnGUI()
        {
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

