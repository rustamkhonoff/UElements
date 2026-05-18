using System.Collections.Generic;
using System.Linq;

namespace UElements.Profiles
{
    public class EditorTimeTargetProvider : ITargetProvider
    {
        private readonly IEnumerable<ProfileTarget> m_targets;

        public EditorTimeTargetProvider(IEnumerable<ProfileTarget> targets)
        {
            m_targets = targets;
        }

        public bool TryGet<T>(string key, out T target) where T : ProfileTarget
        {
            target = null;
            if (m_targets.FirstOrDefault(a => a.Id == key) is not { } found)
                return false;

            target = (T)found;
            return true;
        }
    }
}