using System.Collections.Generic;
using UnityEngine;

namespace UElements.Profiles
{
    public class EditorTimeTargetProvider : ITargetProvider
    {
        private readonly IEnumerable<ProfileTarget> m_targets;

        public EditorTimeTargetProvider(IEnumerable<ProfileTarget> targets)
        {
            m_targets = targets;
        }

        public bool TryGet<T>(string id, out T target) where T : ProfileTarget
        {
            target = null;
            foreach (ProfileTarget profileTarget in m_targets)
            {
                if (profileTarget.Id == id && profileTarget is T t)
                {
                    target = t;
                    return true;
                }
            }
            return false;
        }
    }
}