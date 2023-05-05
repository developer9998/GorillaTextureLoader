using ComputerInterface;
using ComputerInterface.ViewLib;
using ComputerInterface.Views;
using System.Linq;
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
            selectionHandler.MaxIdx = 1;
            selectionHandler.OnSelected += SelectionHandler_OnSelected;

            texturePack = Core.TextureLoader.LoadTextureByPath(Core.TextureLoader.GetAllTextureNames().ElementAt((int)args[0]).Value);
            DrawPage();
        }

        private void DrawPage()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder
                .AddHeader(SCREEN_WIDTH, texturePack.package.Name, texturePack.package.Description, 3)
                .AppendLine(selectionHandler.GetIndicatedText(0, "Load Texture"))
                .AppendLine(selectionHandler.GetIndicatedText(1, "Unload"))
                .AppendLines(1)
                .AppendLine(texturePack.package.IsVerified ? "Compatible in all lobbies.".ToColor() : "This texturepack will ONLY work in modded lobbies.".ToColor("red"))
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
                case 1:
                    Core.TextureLoader.ResetTexture();
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
                ShowView<TexturesView>();
        }
    }
}
