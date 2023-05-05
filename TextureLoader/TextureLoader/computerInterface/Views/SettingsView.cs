using ComputerInterface;
using ComputerInterface.ViewLib;
using System.IO;
using System.Text;
using TextureLoader.Core;
using TextureLoader.Models;

namespace TextureLoader.computerInterface.Views
{
    internal class SettingsView : ComputerView
    {
        private UISelectionHandler selectionHandler;
        private int SelectedIndex;
        public override void OnShow(object[] args)
        {
            base.OnShow(args);

            selectionHandler = new UISelectionHandler(EKeyboardKey.Up, EKeyboardKey.Down, EKeyboardKey.Enter);
            selectionHandler.ConfigureSelectionIndicator("<color=#ed6540>></color>", "", " ", "");
            selectionHandler.OnSelected += SelectionHandler_OnSelected;
            selectionHandler.MaxIdx = 2;
            selectionHandler.CurrentSelectionIndex = SelectedIndex;

            DrawPage();
        }

        private void DrawPage()
        {
            TexturePack texturePack = SettingsController.LoadOnStartup ? Core.TextureLoader.LoadTextureByPath(SettingsController.SelectedKey, true) : null;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder
                .AddHeader(SCREEN_WIDTH, "Settings", "Press the back key to exit.")
                .AppendLine(selectionHandler.GetIndicatedText(0, "LoadOnStartup:" + (SettingsController.LoadOnStartup ? "[Enabled]".ToColor() : "[Disabled]".ToColor("red"))))
                .AppendLine(selectionHandler.GetIndicatedText(1, "LoadKey:" + (SettingsController.LoadOnStartup ? File.Exists(SettingsController.SelectedKey) ? Path.GetFileNameWithoutExtension(SettingsController.SelectedKey) : "[Invalid]".ToColor("red") : "[None]".ToColor("gray"))))
                .AppendLine(selectionHandler.GetIndicatedText(2, "Reset All"))
                .AppendLines(1)
                .AppendLine(SettingsController.LoadOnStartup ? texturePack != null ? texturePack.package.IsVerified ? "This package is verified, thus it is compatible in all lobbies.".ToColor() : "Invalid texturepack, please select a different pack.".ToColor("red") : "ERROR: Failed to load texturepack package".ToColor("red") : "")
            ;
            SetText(stringBuilder);
        }

        private void SelectionHandler_OnSelected(int obj)
        {
            switch (obj)
            {
                case 0:
                    SettingsController.LoadOnStartup = !SettingsController.LoadOnStartup;
                    break;
                case 1:
                    if (SettingsController.LoadOnStartup)
                        ShowView<SettingsView_Selection>();
                    else
                        ShowView<ErrorView>("Invalid Input:You cannot preform this action when the LoadOnStartup value is FALSE.");
                    break;
                case 2:
                    SettingsController.settingsController.MakeFile(new Settings());
                    break;
            }
            DrawPage();
        }

        public override void OnKeyPressed(EKeyboardKey key)
        {
            base.OnKeyPressed(key);
            if (selectionHandler.HandleKeypress(key))
            {
                SelectedIndex = selectionHandler.CurrentSelectionIndex;
                DrawPage();
                return;
            }
            if (key == EKeyboardKey.Back)
                ShowView<MainView>();
        }
    }
}