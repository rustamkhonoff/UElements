using UElements;
using UnityEngine;

namespace Sandbox.Demos.DisposingElements
{
    public class TestingElement : Element
    {
        protected override void OnDisposing()
        {
            Debug.Log("Disposing");
        }

        public override void Initialize()
        {
            Debug.Log("Init");
        }
    }
}