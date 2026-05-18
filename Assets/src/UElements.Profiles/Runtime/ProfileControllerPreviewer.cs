// -------------------------------------------------------------------
// Author: Shokhrukhkhon Rustamkhonov
// Date: 27.04.2026
// Description:
// -------------------------------------------------------------------
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;

namespace UElements.Profiles
{
    [RequireComponent(typeof(ProfileController))]
    public sealed class ProfileControllerPreviewer : MonoBehaviour
    {
        [SerializeField] private string _subject;
        [SerializeField] private string _value;

        public ProfileController Controller => GetComponent<ProfileController>();

        public void Set()
        {
            if (Application.isPlaying)
                Controller.SetValue(_subject, _value);
            else
                Controller.SetValueEditor(_subject, _value);
        }

        [Preserve]
        public IEnumerable GetSubjects => Controller.Subjects.Select(a => a.Key);

        [Preserve]
        public IEnumerable GetStatesForSubject(string subject) => Controller.Subjects.First(a => a.Key == subject).Variants.Select(a => a.Key);

        [Preserve]
        public bool IsValidSubjectId(string subject) => Controller.Subjects.Any(a => a.Key == subject);

        [Preserve]
        public bool IsValidStateForSubject(string subject, string state)
        {
            if (!IsValidSubjectId(subject)) return false;
            return Controller.Subjects.First(a => a.Key == subject).Variants.Any(a => a.Key == state);
        }

        [Preserve]
        public void AutoSelectDefaultSubjectState()
        {
            if (!IsValidSubjectId(_subject)) return;
            _value = Controller.Subjects.First(a => a.Key == _subject).Variants.FirstOrDefault()?.Key;
        }
    }
}