/*
    The computer interface docmentation is really bad:/
*/
using ComputerInterface;
using ComputerInterface.ViewLib;
using System.Linq;
using System.Text;
using TextureLoader.Core;
using TextureLoader.Models;
using UnityEngine;

namespace TextureLoader.computerInterface.Views
{
    internal class MainView : ComputerView
    {
        // constants
        private UISelectionHandler selectionHandler;
        private UIElementPageHandler<TextItem> pageHandler;

        private TextItem[] ExternalTextItemListing;

        public override void OnShow(object[] args)
        {
            base.OnShow(args);
            selectionHandler = new UISelectionHandler(EKeyboardKey.Up, EKeyboardKey.Down, EKeyboardKey.Enter);
            selectionHandler.ConfigureSelectionIndicator("<color=#ed6540>></color> ", "", "  ", "");
            selectionHandler.OnSelected += SelectionHandler_OnSelected;

            pageHandler = new UIElementPageHandler<TextItem>(EKeyboardKey.Left, EKeyboardKey.Right);

            var texturePack = Core.TextureLoader.GetAllTexture();
            ExternalTextItemListing = texturePack.Select(x => new TextItem(x.package.Name, x)).ToArray();

            pageHandler.EntriesPerPage = Mathf.Clamp(texturePack.Length, 0, 5);
            selectionHandler.MaxIdx = pageHandler.EntriesPerPage;
            DrawPage();
        }

        private void DrawPage()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder
                .BeginCenter()
                    .AddHeader(SCREEN_WIDTH, "Texture Loader", "By Crafterbot")
                    .EndAlign();

            int StartingIndex = pageHandler.GetAbsoluteIndex(0);
            for (int i = StartingIndex; i < StartingIndex + pageHandler.EntriesPerPage; i++)
            {
                int AbsoluteIndex = pageHandler.GetAbsoluteIndex(i);
                string text = ExternalTextItemListing[AbsoluteIndex].Text;
                stringBuilder.AppendLine(selectionHandler.GetIndicatedText(AbsoluteIndex, text));
            }
            selectionHandler.CurrentSelectionIndex = StartingIndex;

            SetText(stringBuilder);
        }

        private void SelectionHandler_OnSelected(int obj)
        {
            ShowView<TextureInfoView>(ExternalTextItemListing[obj].TexturePack);
        }

        public override void OnKeyPressed(EKeyboardKey key)
        {
            base.OnKeyPressed(key);
            if (pageHandler.HandleKeyPress(key) || selectionHandler.HandleKeypress(key))
            {
                DrawPage();
                return;
            }

            if (key == EKeyboardKey.Back)
                ReturnToMainMenu();
        }
    }
    internal class TextItem
    {
        public string Text;
        public TexturePack TexturePack;
        public TextItem(string text, TexturePack TexturePack)
        {
            Text = text;
            this.TexturePack = TexturePack;
        }
    }
}
