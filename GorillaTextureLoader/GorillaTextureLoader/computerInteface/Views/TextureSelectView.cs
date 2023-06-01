using ComputerInterface;
using ComputerInterface.ViewLib;
using System.IO;
using System.Linq;
using System.Text;

namespace TextureLoader.computerInteface.Views
{
    internal class TextureSelectView : ComputerView
    {
        private static int _CurrentPageIndex;
        private static int _CurrentSelectedIndex;

        private UIElementPageHandler<TextItem> pageHandler;
        private UISelectionHandler selectionHandler;
        public override void OnShow(object[] args)
        {
            base.OnShow(args);

            pageHandler = new UIElementPageHandler<TextItem>(EKeyboardKey.Left, EKeyboardKey.Right);
            pageHandler.CurrentPage = _CurrentPageIndex;
            pageHandler.EntriesPerPage = 7;
            pageHandler.SetElements(Utilities.GetAllFileNamesInDirectory(Utilities.AddonsDirectoryPath, "*.texture").Select(x => new TextItem(x)).ToArray());
            pageHandler.Footer = "{2}/{3}".ToColor("gray");

            selectionHandler = new UISelectionHandler(EKeyboardKey.Up, EKeyboardKey.Down, EKeyboardKey.Enter);
            selectionHandler.CurrentSelectionIndex = _CurrentSelectedIndex;
            selectionHandler.ConfigureSelectionIndicator("<color=#ed6540>></color>", "", " ", "");
            selectionHandler.OnSelected += (int index) => ShowView<TextureLoadView>(Directory.GetFiles(Utilities.AddonsDirectoryPath, "*.texture")[pageHandler.GetAbsoluteIndex(index)]); // load texture
            selectionHandler.MaxIdx = pageHandler.EntriesPerPage;

            DrawPage();
        }
        private void DrawPage()
        {
            StringBuilder stringBuilder = new StringBuilder(); // Utilities.appendHeader("", "");
            stringBuilder
                .AppendHeader("Load Texture", $"Entries[{Utilities.GetAllFileNamesInDirectory(Utilities.AddonsDirectoryPath, "*.texture").Length}]")
                .BeginAlign("left");
            pageHandler.EnumarateElements((TextItem Item, int Index) => stringBuilder.AppendLine(selectionHandler.GetIndicatedText(Index, Item.Text)));
            stringBuilder.BeginAlign("right");
            pageHandler
                .AppendFooter(stringBuilder);

            SetText(stringBuilder);
        }
        public override void OnKeyPressed(EKeyboardKey key)
        {
            if (selectionHandler.HandleKeypress(key) || pageHandler.HandleKeyPress(key))
            {
                _CurrentPageIndex = pageHandler.CurrentPage;
                _CurrentSelectedIndex = selectionHandler.CurrentSelectionIndex;
                DrawPage();
                return;
            }
            if (key == EKeyboardKey.Back)
                ShowView<PrimaryView>();
        }

        internal class TextItem
        {
            public string Text;
            public TextItem(string text) =>
                Text = text;
        }
    }
}
