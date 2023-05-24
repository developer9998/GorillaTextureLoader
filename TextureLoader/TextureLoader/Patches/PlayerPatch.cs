using HarmonyLib;
using System.IO;
using TextureLoader.Models;

namespace TextureLoader.Patches
{
    [HarmonyPatch(typeof(GorillaLocomotion.Player))]
    internal class PlayerPatch
    {
        [HarmonyPostfix]
        [HarmonyWrapSafe]
        [HarmonyPatch("Awake")]
        private static void HookAwake()
        {
            bool LoadOnStartup = Main.LoadOnStartup.Value;
            string Key = Main.LoadOnStartupKey.Value;

            if (File.Exists(Key) && LoadOnStartup)
            {
                Package package = TextureController.GetPackage(Key);
                if (package.IsVerified)
                {
                    $"Loading texturepack:{package.Name}".Log();
                    TextureController.LoadTexture(Key, package);
                    "Loaded texturepack".Log(BepInEx.Logging.LogLevel.Message);
                }
                else
                    $"Uh oh, you can only load verified textures on startup:/".Log(BepInEx.Logging.LogLevel.Fatal);
            }
        }
    }
}
