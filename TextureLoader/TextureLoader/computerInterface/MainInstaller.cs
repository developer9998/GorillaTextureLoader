using ComputerInterface.Interfaces;
using Zenject;

namespace TextureLoader.computerInterface
{
    internal class MainInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.Bind<IComputerModEntry>().To<Entry>().AsSingle();
        }
    }
}
