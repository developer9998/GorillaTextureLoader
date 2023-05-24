using ComputerInterface;
using ComputerInterface.ViewLib;
using ModestTree;
using System.IO;
using System.Text;
using TextureLoader.Models;
using UnityEngine;

namespace TextureLoader.computerInteface.Views
{
    internal class SettingsView : ComputerView
    {
        private UISelectionHandler selectionHandler;
        private UISelectionHandler SelectPackHandler; // stole this idea from Dev9998

        private Package Package;

        public override void OnShow(object[] args)
        {
            base.OnShow(args);

            selectionHandler = new UISelectionHandler(EKeyboardKey.Up, EKeyboardKey.Down, EKeyboardKey.Enter);
            selectionHandler.ConfigureSelectionIndicator("<color=#ed6540>></color>", "", " ", "");
            selectionHandler.MaxIdx = 2;
            selectionHandler.OnSelected += SelectionHandler_OnSelected;

            SelectPackHandler = new UISelectionHandler(EKeyboardKey.Left, EKeyboardKey.Right);
            SelectPackHandler.MaxIdx = Utilities.GetAllFileNamesInDirectory(Utilities.AddonsDirectoryPath, "*.texture").Length;
            SelectPackHandler.CurrentSelectionIndex = File.Exists(PlayerPrefs.GetString(Main.PLAYERPREFS_ONLOADKEY)) ? Directory.GetFiles(Utilities.AddonsDirectoryPath, "*.textures").IndexOf(PlayerPrefs.GetString(Main.PLAYERPREFS_ONLOADKEY)) : -1;

            if (SelectPackHandler.CurrentSelectionIndex != -1)
                Package = TextureController.GetPackage(Directory.GetFiles(Utilities.AddonsDirectoryPath, "*.textures")[SelectPackHandler.CurrentSelectionIndex]);
            DrawPage();
        }

        private void DrawPage()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder
                .AppendHeader("Settings", "", 3)
                .AppendLine(selectionHandler.GetIndicatedText(0, "LoadOnStartup:[" + (Utilities.GetBool(Main.PLAYERPREFS_ONLOAD_TOGGLED) ? "Enabled".ToColor() : "Disabled".ToColor("red")) + "]"))
                .AppendLine(selectionHandler.GetIndicatedText(1, "LoadKey:[" + (Utilities.GetBool(Main.PLAYERPREFS_ONLOAD_TOGGLED) ? (SelectPackHandler.CurrentSelectionIndex != -1) ? (Utilities.GetAllFileNamesInDirectory(Utilities.AddonsDirectoryPath, "*.texture")[SelectPackHandler.CurrentSelectionIndex]) : "Invalid".ToColor("red") : "Disabled".ToColor("gray")) + "]"))
                .AppendLine(selectionHandler.GetIndicatedText(2, "Reset All"))
                .AppendLines(1)
                .AppendLine(Package != null ? (Package.IsVerified ? "This package is verified.".ToColor("green") : "Uh oh, please select a verified package.".ToColor("red")) : "Invalid package")
                ;
            SetText(stringBuilder);
        }

        /* Handler methods */

        public override void OnKeyPressed(EKeyboardKey key)
        {
            if (selectionHandler.HandleKeypress(key) || SelectPackHandler.HandleKeypress(key))
            {
                DrawPage();
                return;
            }
            if (key == EKeyboardKey.Back)
                ReturnView();
        }
        private void SelectionHandler_OnSelected(int obj)
        {
            switch (obj)
            {
                case 0:
                    Utilities.GetBool(Main.PLAYERPREFS_ONLOAD_TOGGLED, !Utilities.GetBool(Main.PLAYERPREFS_ONLOAD_TOGGLED));
                    PlayerPrefs.Save();
                    break;
                case 1:
                    PlayerPrefs.SetString(Main.PLAYERPREFS_ONLOADKEY, Directory.GetFiles(Utilities.AddonsDirectoryPath)[SelectPackHandler.CurrentSelectionIndex]);
                    PlayerPrefs.Save();
                    break;
                case 2:
                    PlayerPrefs.SetString(Main.PLAYERPREFS_ONLOADKEY, "");
                    Utilities.GetBool(Main.PLAYERPREFS_ONLOAD_TOGGLED, false);
                    PlayerPrefs.Save();
                    break;
            }
        }
    }
}
