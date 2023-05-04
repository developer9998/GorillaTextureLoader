using BepInEx;
using Bepinject;

namespace TextureLoader
{
    [BepInPlugin(GUID, NAME, VERSION)]
    [BepInDependency("tonimacaroni.computerinterface")] // All other dependencies are referenced in computer interface itself.
    [BepInDependency("org.legoandmars.gorillatag.utilla")]
    internal class Main : BaseUnityPlugin
    {
        internal const string
            GUID = "crafterbot.textureloader",
            NAME = "TextureLoader",
            VERSION = "1.0.0";
        private void Awake()
        {
            Zenjector.Install<computerInterface.MainInstaller>().OnProject();
            Utilla.Events.RoomLeft += Events_RoomLeft;
        }

        private void Events_RoomLeft(object sender, Utilla.Events.RoomJoinedArgs e)
        {
            Core.TextureLoader.ResetTexture();
        }
    }
}