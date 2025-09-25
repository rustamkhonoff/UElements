using System;
using Sirenix.OdinInspector;
using UElements;
using UnityEngine;

namespace Sandbox.Demos
{
    public class TestMono : MonoBehaviour
    {
        [SerializeField] private TypedElementRequest _request;

        [Button]
        private void Create() => ElementsGlobal.Create(_request);
    }
}