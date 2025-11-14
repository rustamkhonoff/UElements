using UElements.Zenject;
using Zenject;

namespace Sandbox.Installers
{
    public class ZenjectProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.AddUElements();
        }
    }
}
