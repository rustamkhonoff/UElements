using System.Threading;
using UElements;
using UnityEngine;

namespace Sandbox.Demos.DisposingElements
{
    public class TestingElement : Element
    {
        protected override void DeInitialize()
        {
            Debug.Log("Disposing");
        }

        protected override void Initialize(CancellationToken ct)
        {
            Debug.Log("Init");
        }
    }
}