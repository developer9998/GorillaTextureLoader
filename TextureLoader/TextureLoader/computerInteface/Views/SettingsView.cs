using ComputerInterface;
using ComputerInterface.ViewLib;
using ModestTree;
using System.IO;
using System.Text;
using TextureLoader.Models;

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
            string[] AllTexturepacksNames = Utilities.GetAllFileNamesInDirectory(Utilities.AddonsDirectoryPath, "*.texture");

            selectionHandler = new UISelectionHandler(EKeyboardKey.Up, EKeyboardKey.Down, EKeyboardKey.Enter);
            selectionHandler.ConfigureSelectionIndicator("<color=#ed6540>></color>", "", " ", "");
            selectionHandler.MaxIdx = 2;
            selectionHandler.OnSelected += SelectionHandler_OnSelected;

            SelectPackHandler = new UISelectionHandler(EKeyboardKey.Left, EKeyboardKey.Right);
            SelectPackHandler.MaxIdx = AllTexturepacksNames.Length;
            int KeyIndex = AllTexturepacksNames.IndexOf(Path.GetFileNameWithoutExtension(Main.LoadOnStartupKey.Value));
            SelectPackHandler.CurrentSelectionIndex = KeyIndex == -1 ? 0 : KeyIndex;

            if (File.Exists(Main.LoadOnStartupKey.Value))
                Package = TextureController.GetPackage(Main.LoadOnStartupKey.Value);
            DrawPage();
        }

        private void DrawPage()
        {
            string DisplayKey = Utilities.GetAllFileNamesInDirectory(Utilities.AddonsDirectoryPath, "*.texture")[SelectPackHandler.CurrentSelectionIndex];
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder
                .AppendHeader("Settings", "", 3)
                .AppendLine(selectionHandler.GetIndicatedText(0, "LoadOnStartup:[" + (Main.LoadOnStartup.Value ? "Enabled".ToColor() : "Disabled".ToColor("red")) + "]"))
                .AppendLine(selectionHandler.GetIndicatedText(1, "LoadKey:[" + (Main.LoadOnStartup.Value ? DisplayKey : "Disabled".ToColor("gray")) + "]"))
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
                    Main.LoadOnStartup.Value = !Main.LoadOnStartup.Value;
                    break;
                case 1:
                    if (Main.LoadOnStartup.Value)
                    {
                        Main.LoadOnStartupKey.Value = Directory.GetFiles(Utilities.AddonsDirectoryPath, "*.texture")[SelectPackHandler.CurrentSelectionIndex];
                        Package = TextureController.GetPackage(Main.LoadOnStartupKey.Value);
                    }
                    break;
                case 2:
                    Main.LoadOnStartup.Value = false;
                    Main.LoadOnStartupKey.Value = "";
                    break;
            }
        }
    }
}
