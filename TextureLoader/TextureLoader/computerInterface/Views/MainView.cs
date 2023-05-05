/*
    The computer interface docmentation is really bad:/
*/
using ComputerInterface;
using ComputerInterface.ViewLib;
using System.Text;
using TextureLoader.Core;

namespace TextureLoader.computerInterface.Views
{
    internal class MainView : ComputerView
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
                .BeginCenter()
                .AddHeader(SCREEN_WIDTH, "Texture Loader", "By Crafterbot")
                .EndAlign()

                .AppendLine(selectionHandler.GetIndicatedText(0, "Load Texture"))
                .AppendLine(selectionHandler.GetIndicatedText(1, "Settings"))
                .AppendLine(selectionHandler.GetIndicatedText(2, "Credits"))
                ;

            SetText(stringBuilder);
        }

        private void SelectionHandler_OnSelected(int obj)
        {
            switch (obj)
            {
                case 0:
                    ShowView<TexturesView>();
                    break;
                case 1:
                    ShowView<SettingsView>();
                    break;
                case 2:
                    ShowView<Credits>();
                    break;
            }
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
                ReturnToMainMenu();
        }
    }
}
