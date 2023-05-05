using ComputerInterface;
using ComputerInterface.ViewLib;
using ModestTree;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TextureLoader.Core;

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
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder
                .AddHeader(SCREEN_WIDTH, "Settings", "Press the back key to exit.")
                .AppendLine(selectionHandler.GetIndicatedText(0, "LoadOnStartup:" + (SettingsController.LoadOnStartup ? "[Enabled]".ToColor() : "[Disabled]".ToColor("red"))))
                .AppendLine(selectionHandler.GetIndicatedText(1, "LoadKey:" + (SettingsController.LoadOnStartup ? File.Exists(SettingsController.SelectedKey) ? Path.GetFileNameWithoutExtension(SettingsController.SelectedKey) : "[Invalid]".ToColor("red") : "[None]".ToColor("gray"))))
                .AppendLine(selectionHandler.GetIndicatedText(2, "Reset All"))
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
                    {
                        Dictionary<string, string> textures = Core.TextureLoader.GetAllTextureNames();
                        string[] Values = textures.ValueToArray();
                        int IndexOf = Values.IndexOf(SettingsController.SelectedKey);
                        if (IndexOf == -1)
                            SettingsController.SelectedKey = Values[0];
                        else if (IndexOf == Values.Length - 1)
                            SettingsController.SelectedKey = Values[0];
                        else
                            SettingsController.SelectedKey = Values[IndexOf + 1];
                    }
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