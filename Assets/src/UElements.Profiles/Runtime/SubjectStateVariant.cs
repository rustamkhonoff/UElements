using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace UElements.Profiles
{
    [Serializable]
    public class SubjectStateVariant
    {
        [field: SerializeField] public string Key { get; private set; }
        [field: SerializeField] public ApplyMode ApplyMode { get; private set; } = ApplyMode.Parallel;
        [field: SerializeField, SerializeReference] public IProfileOperation[] Operations { get; private set; } = Array.Empty<IProfileOperation>();

        [Preserve]
        [field: SerializeField] public bool UsePredefinedStateKeys { get; private set; }

        private Action m_callback;

        internal void Bind(Action callback)
        {
            m_callback = callback;
        }

        private void Activate()
        {
            m_callback?.Invoke();
        }
    }
}