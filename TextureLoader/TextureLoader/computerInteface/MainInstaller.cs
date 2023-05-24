using ComputerInterface.Interfaces;
using Zenject;

namespace TextureLoader.computerInteface
{
    internal class MainInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.Bind<IComputerModEntry>().To<computerInteface.Views.PrimaryView.Entry>().AsSingle();
        }
    }
}
