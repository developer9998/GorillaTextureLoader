using ComputerInterface;
using ComputerInterface.ViewLib;
using TextureLoader.Core;

namespace TextureLoader.computerInterface.Views
{
    internal class Credits : ComputerView
    {
        public override void OnShow(object[] args)
        {
            base.OnShow(args);

            SetText(stringBuilder =>
            {
                stringBuilder
                .AddHeader(SCREEN_WIDTH, "Credits & Info", "Press any key to exit.")
                .Append("This plugin was created by Crafterbot. The template images were created by Another-Axiom.")
                .AppendLine("In addition, This plugin was inspired by fchb1239 preview video. fchb1239s plugin was never released, and is now outdated.")
                .AppendLines(2)
                .AppendLine("All textures used in this plugin belong to there respective owners.".ToColor("gray"));

            });
        }

        public override void OnKeyPressed(EKeyboardKey key)
        {
            ShowView<MainView>();
        }
    }
}
