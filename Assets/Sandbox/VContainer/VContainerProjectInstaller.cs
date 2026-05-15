using UElements.VContainer;
using VContainer;
using VContainer.Unity;

namespace Sandbox.VContainer
{
    public class VContainerProjectInstaller : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.AddUElements();
        }
    }
}
