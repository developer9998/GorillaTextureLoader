using ComputerInterface;
using ComputerInterface.Interfaces;
using ComputerInterface.ViewLib;
using System;
using System.Text;

namespace TextureLoader.computerInteface.Views
{
    internal class PrimaryView : ComputerView
    {
        internal static int _CurrentSelectedIndex;
        internal class Entry : IComputerModEntry
        {
            public string EntryName => Main.NAME;
            public Type EntryViewType => typeof(PrimaryView);
        }

        private UISelectionHandler selectionHandler;
        public override void OnShow(object[] args)
        {
            base.OnShow(args);

            selectionHandler = new UISelectionHandler(EKeyboardKey.Up, EKeyboardKey.Down, EKeyboardKey.Enter);
            selectionHandler.MaxIdx = 2;
            selectionHandler.ConfigureSelectionIndicator("<color=#ed6540>></color>", "", " ", "");
            selectionHandler.CurrentSelectionIndex = _CurrentSelectedIndex;
            selectionHandler.OnSelected += SelectionHandler_OnSelected;

            DrawPage();
        }

        private void DrawPage()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder
                .AppendHeader(Main.NAME, "By Crafterbot")
                .AppendLine(selectionHandler.GetIndicatedText(0, "Load Texture"))
                .AppendLine(selectionHandler.GetIndicatedText(1, "Settings"))
                .AppendLine(selectionHandler.GetIndicatedText(2, "Credits"))
                ;
            SetText(stringBuilder);
        }

        /* Handler methods */

        private void SelectionHandler_OnSelected(int obj)
        {
            _CurrentSelectedIndex = selectionHandler.CurrentSelectionIndex;
            switch (obj)
            {
                case 0:
                    ShowView<TextureSelectView>();
                    break;
                case 1:
                    ShowView<SettingsView>();
                    break;
                case 2:
                    ShowView<CreditsView>();
                    break;
            }
        }

        public override void OnKeyPressed(EKeyboardKey key)
        {
            if (selectionHandler.HandleKeypress(key))
            {
                DrawPage();
                return;
            }
            if (key == EKeyboardKey.Back)
            {
                _CurrentSelectedIndex = selectionHandler.CurrentSelectionIndex;
                ReturnToMainMenu();
            }
        }
    }
}
