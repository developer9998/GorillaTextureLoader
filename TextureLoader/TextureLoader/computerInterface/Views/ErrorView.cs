using ComputerInterface;
using ComputerInterface.ViewLib;
using TextureLoader.Core;

namespace TextureLoader.computerInterface.Views
{
    internal class ErrorView : ComputerView
    {
        public override void OnShow(object[] args)
        {
            base.OnShow(args);
            SetText($"<color=red>{args[0]}</color>".ToStringBuilder());
        }

        public override void OnKeyPressed(EKeyboardKey key)
        {
            ReturnToMainMenu();
        }
    }
}
