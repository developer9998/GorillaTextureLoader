using ComputerInterface;
using ComputerInterface.ViewLib;
using System.Text;
using TextureLoader.Core;
using TextureLoader.Models;

namespace TextureLoader.computerInterface.Views
{
    internal class TextureInfoView : ComputerView
    {
        private UISelectionHandler selectionHandler;
        private TexturePack texturePack;

        public override void OnShow(object[] args)
        {
            base.OnShow(args);

            selectionHandler = new UISelectionHandler(EKeyboardKey.Up, EKeyboardKey.Down, EKeyboardKey.Enter);
            selectionHandler.ConfigureSelectionIndicator("<color=#ed6540>></color> ", "", "  ", "");
            selectionHandler.MaxIdx = 0;
            selectionHandler.OnSelected += SelectionHandler_OnSelected;

            texturePack = (TexturePack)args[0];
            DrawPage();
        }

        private void DrawPage()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder
                .AddHeader(SCREEN_WIDTH, texturePack.package.Name, texturePack.package.Description, 3)
                .AppendLine(selectionHandler.GetIndicatedText(0, "Load Texture"))
                ;
            SetText(stringBuilder);
        }

        private void SelectionHandler_OnSelected(int obj)
        {
            switch (obj)
            {
                case 0:
                    Core.TextureLoader.SetTexture(texturePack);
                    break;
            }
        }

        public override void OnKeyPressed(EKeyboardKey key)
        {
            base.OnKeyPressed(key);
            if (selectionHandler.HandleKeypress(key))
            {
                DrawPage();
                return;
            }

            if (key == EKeyboardKey.Back)
                ReturnToMainMenu();
        }
    }
}
