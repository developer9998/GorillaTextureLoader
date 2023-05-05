/*
    If any other developers want to add more maps, have fun. 
    DM me and I will give you the texture maker. However the 
    canyon has like 50 different textures so I don't recommend.
    I personally am way to lazy to do that. And to be honest
    I would run out of motivation and stop working on the project.
*/
using BepInEx;
using Bepinject;
using System.Collections;
using System.IO;
using TextureLoader.Core;
using TextureLoader.Models;
using UnityEngine;
using Utilla;

namespace TextureLoader
{
    [BepInPlugin(GUID, NAME, VERSION)]
    [BepInDependency("tonimacaroni.computerinterface")] // All other dependencies are referenced in computer interface itself.
    [BepInDependency("org.legoandmars.gorillatag.utilla")]

    [ModdedGamemode]
    internal class Main : BaseUnityPlugin
    {
        internal const string
            GUID = "crafterbot.textureloader",
            NAME = "TextureLoader",
            VERSION = "0.0.2";
        internal static RoomType roomType;

        private void Awake()
        {
            if (GetIsFolderValid())
            {
                new SettingsController();
                Zenjector.Install<computerInterface.MainInstaller>().OnProject();
                Utilla.Events.RoomLeft += Events_RoomLeft;
                Utilla.Events.RoomJoined += Events_RoomJoined;
                Utilla.Events.GameInitialized += (object sender, System.EventArgs e) => StartCoroutine(LoadDefault());
            }
        }
        private IEnumerator LoadDefault()
        {
            yield return new WaitForSeconds(2f); // buffer time. The game is already loaded but to decrease initial lag.
            if (SettingsController.LoadOnStartup)
            {
                if (!File.Exists(SettingsController.SelectedKey))
                    throw new System.Exception("FUCK");
                TexturePack texturePack = Core.TextureLoader.LoadTextureByPath(SettingsController.SelectedKey);
                if (texturePack.package.IsVerified)
                {
                    Core.TextureLoader.SetTexture(texturePack);
                }
            }
        }
        private bool GetIsFolderValid()
        {
            return Directory.Exists(Path.GetDirectoryName(typeof(Main).Assembly.Location) + "/addons");
        }
        #region Room event handlers
        [ModdedGamemodeJoin]
        private void OnModdedJoin() =>
            roomType = RoomType.Modded;
        private void Events_RoomJoined(object sender, Utilla.Events.RoomJoinedArgs e) =>
            roomType = RoomType.Standard;
        private void Events_RoomLeft(object sender, Utilla.Events.RoomJoinedArgs e)
        {
            roomType = RoomType.None;
            if (Core.TextureLoader.CurrentTexturePack != null)
            {
                if (!Core.TextureLoader.CurrentTexturePack.package.IsVerified)
                    Core.TextureLoader.ResetTexture();
            }
        }

        #endregion
    }

    internal enum RoomType
    {
        None,
        Modded,
        Standard
    }
}