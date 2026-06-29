using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;

namespace UElements.Profiles
{
    [Serializable]
    public class ProfileSubject
    {
        [field: SerializeField] public string Key { get; private set; } = nameof(StateNames.None);
        [field: SerializeField] public bool HasDefaultValue { get; private set; } = true;
        [field: SerializeField] public string DefaultValue { get; private set; }
        [field: SerializeField] public List<SubjectStateVariant> Variants { get; private set; }

        private Dictionary<string, SubjectStateVariant> m_variants = new();
        public bool Cached { get; private set; }

        [Preserve]
        [field: SerializeField] public bool UsePredefinedValues { get; private set; } = true;

        [Preserve]
        public IEnumerable Keys => Variants.Select(a => a.Key);

        public void ClearCache()
        {
            Cached = false;
            m_variants.Clear();
        }

        public void Cache()
        {
            if (Cached) return;
            m_variants = Variants.ToDictionary(a => a.Key, a => a);
            Cached = true;
        }

        public bool TryGetVariant(string key, out SubjectStateVariant variant)
        {
            return m_variants.TryGetValue(key, out variant);
        }
    }
}