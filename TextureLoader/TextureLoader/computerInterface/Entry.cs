using ComputerInterface.Interfaces;
using System;
using TextureLoader.computerInterface.Views;

namespace TextureLoader.computerInterface
{
    public class Entry : IComputerModEntry
    {
        public string EntryName => Main.NAME;
        public Type EntryViewType => typeof(MainView);
    }
}
