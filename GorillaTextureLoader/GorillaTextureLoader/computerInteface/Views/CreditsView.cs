using ComputerInterface;
using ComputerInterface.ViewLib;
using System.Text;

namespace TextureLoader.computerInteface.Views
{
    internal class CreditsView : ComputerView
    {
        public override void OnShow(object[] args)
        {
            base.OnShow(args);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder
                .AppendHeader("Credits", "", 3)
                .AppendLine("This plugin was created by Crafterbot and is inspired by fch1239's GorillaTexturepack mod. I (Crafterbot) hold no responsibility for players pirating textures.");
            SetText(stringBuilder);
        }
        public override void OnKeyPressed(EKeyboardKey key)
        {
            ShowView<PrimaryView>();
        }
    }
}
