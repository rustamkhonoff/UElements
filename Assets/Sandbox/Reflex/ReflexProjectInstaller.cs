// -------------------------------------------------------------------
// Author: Shokhrukhkhon Rustamkhonov
// Date: 14.11.2025
// Description:
// -------------------------------------------------------------------

using Reflex.Core;
using UElements.Zenject;
using UnityEngine;

namespace Sandbox.Reflex
{
    public class ReflexProjectInstaller : MonoBehaviour, IInstaller
    {
        public void InstallBindings(ContainerBuilder containerBuilder)
        {
            containerBuilder.AddSingleton("Received");
            containerBuilder.AddUElements();
        }
    }
}