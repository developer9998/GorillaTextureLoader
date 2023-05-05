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
            string value = args[0] as string;
            SetText(value.ToColor("red").ToStringBuilder());
        }

        public override void OnKeyPressed(EKeyboardKey key)
        {
            ReturnView();
        }
    }
}