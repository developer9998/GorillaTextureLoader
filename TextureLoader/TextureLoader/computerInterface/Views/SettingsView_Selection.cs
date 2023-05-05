using ComputerInterface;
using ComputerInterface.ViewLib;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TextureLoader.Core;

namespace TextureLoader.computerInterface.Views
{
    internal class SettingsView_Selection : ComputerView
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
            string path = Core.SettingsController.SelectedKey;

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AddHeader(SCREEN_WIDTH, "Texture Selection on Startup", $"[{Core.TextureLoader.GetAllTextureNames().Count}] Entries");
            pageHandler.EnumarateElements((obj, index) =>
            {
                bool IsSelected = path == obj.FullPath;
                stringBuilder.AppendLine(selectionHandler.GetIndicatedText(index, IsSelected ? $"[{obj.Name}]".ToColor("green") : obj.Name));
            });
            stringBuilder.AppendFooter(pageHandler);
            SetText(stringBuilder);
        }

        private void SelectionHandler_OnSelected(int obj)
        {
            SettingsController.SelectedKey = pageHandler.GetElementsForPage(pageHandler.CurrentPage)[obj].FullPath;
            ShowView<SettingsView>();
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
}
