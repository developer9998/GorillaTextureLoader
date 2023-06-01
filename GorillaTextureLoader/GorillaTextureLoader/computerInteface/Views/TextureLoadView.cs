using ComputerInterface;
using ComputerInterface.ViewLib;
using System.Text;
using TextureLoader.Models;

namespace TextureLoader.computerInteface.Views
{
    internal class TextureLoadView : ComputerView
    {
        private string TexturePath;
        private Package package;

        private UISelectionHandler selectionHandler;

        public override void OnShow(object[] args)
        {
            base.OnShow(args);
            TexturePath = (string)args[0];
            package = TextureController.GetPackage(TexturePath);

            selectionHandler = new UISelectionHandler(EKeyboardKey.Up, EKeyboardKey.Down, EKeyboardKey.Enter);
            selectionHandler.OnSelected += SelectionHandler_OnSelected;
            selectionHandler.ConfigureSelectionIndicator("<color=#ed6540>></color>", "", " ", "");
            selectionHandler.MaxIdx = 1;

            DrawPage();
        }

        private void DrawPage()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder
                .AppendHeader(package.Name, package.Description)
                .BeginAlign("left")
                .AppendLine(selectionHandler.GetIndicatedText(0, "Load"))
                .AppendLine(selectionHandler.GetIndicatedText(1, "Unload"))
                .AppendLines(1)
                .AppendLine(package.IsVerified ? "Verified | Works in ALL lobbies".ToColor() : "UnVerified | Only works in modded lobbies".ToColor("red"))
                ;
            SetText(stringBuilder);
        }

        public override void OnKeyPressed(EKeyboardKey key)
        {
            if (selectionHandler.HandleKeypress(key))
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
                    if (!Main.RoomModded && !package.IsVerified)
                        return;
                    TextureController.LoadTexture(TexturePath, package);
                    break;
                case 1:
                    TextureController.ResetTextures();
                    break;
            }
        }
    }
}
