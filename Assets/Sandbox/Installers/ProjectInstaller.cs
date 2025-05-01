using UElements.Zenject;
using Zenject;

namespace Sandbox.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.AddUElements();
        }
    }
}
