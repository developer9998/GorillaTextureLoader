using HarmonyLib;
using System.IO;
using TextureLoader.Models;
using UnityEngine;

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
            if (PlayerPrefs.HasKey(Main.PLAYERPREFS_ONLOAD_TOGGLED) && PlayerPrefs.HasKey(Main.PLAYERPREFS_ONLOADKEY))
            {
                bool LoadOnStartup = Utilities.GetBool(Main.PLAYERPREFS_ONLOAD_TOGGLED);
                string Key = PlayerPrefs.GetString(Main.PLAYERPREFS_ONLOADKEY);

                if (File.Exists(Key) && LoadOnStartup)
                {
                    Package package = TextureController.GetPackage(Key);
                    if (package.IsVerified)
                    {
                        $"Loading texturepack:{package.Name}".Log();
                        TextureController.LoadTexture(Key);
                        "Loaded texturepack".Log(BepInEx.Logging.LogLevel.Message);
                    }
                    else
                        $"Uh oh, you can only load verified textures on startup:/ | Or it is just not on:'(".Log(BepInEx.Logging.LogLevel.Fatal);
                }
            }
            else
            {
                PlayerPrefs.SetString(Main.PLAYERPREFS_ONLOADKEY, "");
                Utilities.GetBool(Main.PLAYERPREFS_ONLOAD_TOGGLED, false);
                PlayerPrefs.Save();
            }
        }
    }
}
