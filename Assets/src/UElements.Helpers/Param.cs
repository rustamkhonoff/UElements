using System;
using UnityEngine;

namespace UElements.Helpers
{
    [Serializable]
    public class Param
    {
        [field: SerializeField] public string Key { get; private set; }
        [field: SerializeField] public string Value { get; private set; }
    }
}