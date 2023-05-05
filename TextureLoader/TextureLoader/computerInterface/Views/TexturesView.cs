using ComputerInterface;
using ComputerInterface.ViewLib;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TextureLoader.Core;

namespace TextureLoader.computerInterface.Views
{
    internal class TexturesView : ComputerView
    {
        private UIElementPageHandler<TextItem_Entry> pageHandler;
        private UISelectionHandler selectionHandler;

        public override void OnShow(object[] args)
        {
            base.OnShow(args);

            pageHandler = new UIElementPageHandler<TextItem_Entry>(EKeyboardKey.Left, EKeyboardKey.Right);
            pageHandler.EntriesPerPage = 5;
            pageHandler.NextMark = "→";
            pageHandler.PrevMark = "←";

            selectionHandler = new UISelectionHandler(EKeyboardKey.Up, EKeyboardKey.Down, EKeyboardKey.Enter);
            selectionHandler.ConfigureSelectionIndicator("<color=#ed6540>></color>", "", " ", "");
            selectionHandler.OnSelected += SelectionHandler_OnSelected;
            selectionHandler.MaxIdx = pageHandler.EntriesPerPage;

            Dictionary<string, string> AlltexturePaths = Core.TextureLoader.GetAllTextureNames();
            TextItem_Entry[] Entries = new TextItem_Entry[AlltexturePaths.Count];
            selectionHandler.MaxIdx = AlltexturePaths.Count;
            for (int i = 0; i < AlltexturePaths.Count; i++)
            {
                KeyValuePair<string, string> pair = AlltexturePaths.ElementAt(i);
                Entries[i] = new TextItem_Entry(pair.Key, pair.Value);
            }
            pageHandler.SetElements(Entries);

            DrawPage();
        }

        private void DrawPage()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AddHeader(SCREEN_WIDTH, "Textures", $"[{Core.TextureLoader.GetAllTextureNames().Count}] Entries");
            pageHandler.EnumarateElements((obj, index) =>
            {
                stringBuilder.AppendLine(selectionHandler.GetIndicatedText(index, obj.Name));
            });
            stringBuilder.AppendFooter(pageHandler);
            SetText(stringBuilder);
        }

        private void SelectionHandler_OnSelected(int obj)
        {
            ShowView<TextureInfoView>(pageHandler.GetAbsoluteIndex(obj));
        }

        public override void OnKeyPressed(EKeyboardKey key)
        {
            if (selectionHandler.HandleKeypress(key) || pageHandler.HandleKeyPress(key))
            {
                DrawPage();
                return;
            }
            if (key == EKeyboardKey.Back)
                ShowView<MainView>();
        }
    }

    internal class TextItem_Entry
    {
        public string Name { get; set; }
        public string FullPath { get; set; }
        public TextItem_Entry(string name, string fullPath)
        {
            Name = name;
            FullPath = fullPath;
        }
    }
}
