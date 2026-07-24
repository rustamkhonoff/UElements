// -------------------------------------------------------------------
// File: ProfileSystem.cs
// Description:
// Group-based editor-driven UI profile system for Unity
// Keeps ProfileTarget and IProfileOperation as-is
// -------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Scripting;
#if ODIN_INSPECTOR && UNITY_EDITOR
using Sirenix.Utilities.Editor;
#endif

namespace UElements.Profiles
{
    [DefaultExecutionOrder(-999)]
    public sealed class ProfileController : MonoBehaviour, ITargetProvider
    {
        [field: SerializeField] public ProfileSubject[] Subjects { get; private set; }

        [field: SerializeField]
#if ODIN_INSPECTOR&& UNITY_EDITOR
        [field: ListDrawerSettings(OnTitleBarGUI = nameof(DrawTitleBar))]
#endif
        public ProfileTarget[] Targets { get; private set; } = Array.Empty<ProfileTarget>();

        private readonly Dictionary<string, ProfileSubject> m_profileSubjects = new();
        private readonly Dictionary<string, string> m_stateBag = new();
        private readonly Dictionary<string, ProfileTarget> m_profileTargetsMap = new();

        private CancellationTokenSource Lifetime { get; set; } = new();

#if ODIN_INSPECTOR && UNITY_EDITOR
        private void DrawTitleBar()
        {
            if (SirenixEditorGUI.ToolbarButton(EditorIcons.Refresh))
            {
                Rebuild();
            }
        }
#endif

        [Preserve]
        public IEnumerable KeysByTargetType(Type type)
        {
            return
                from profileTarget in Targets
                where profileTarget.GetType() == type
                select profileTarget.Id;
        }

        [Preserve]
        public bool IsValidTargetId(string id) => Targets.Any(a => a.Id == id);

        private void Awake()
        {
            Prepare();
        }

        private void OnDestroy()
        {
            Lifetime?.Cancel();
            Lifetime?.Dispose();
            Lifetime = null;
        }

        private void OnValidate()
        {
            Rebuild();
        }

        public void SetValueEditor(string subject, string value)
        {
            ProfileSubject sbj = Subjects.FirstOrDefault(a => a.Key == subject);
            if (sbj == null)
                return;
            EditorTimeTargetProvider provider = new(Targets);

            foreach (IProfileOperation profileOperation in sbj.Variants.FirstOrDefault(a => a.Key == value)!.Operations)
            {
                profileOperation.Apply(provider);
            }

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                UnityEditor.EditorUtility.SetDirty(this);
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);

                UnityEditor.SceneView.RepaintAll();
                UnityEditorInternal.InternalEditorUtility.RepaintAllViews();

                UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
            }
#endif
        }

        private void Prepare()
        {
            m_profileTargetsMap.Clear();
            foreach (ProfileTarget profileTarget in Targets)
                m_profileTargetsMap[profileTarget.Id] = profileTarget;

            foreach (ProfileSubject profileSubject in Subjects)
                m_profileSubjects[profileSubject.Key] = profileSubject;

            foreach (ProfileSubject subject in Subjects.Where(a => a.HasDefaultValue))
                SetValueImmediate(subject.Key, subject.DefaultValue);
        }

        public bool TryGet<T>(string id, out T target) where T : ProfileTarget
        {
            target = null;
            bool has = m_profileTargetsMap.TryGetValue(id, out ProfileTarget found);
            if (!has) return false;
            target = (T)found;
            return true;
        }

        [Preserve]
#if ODIN_INSPECTOR && UNITY_EDITOR
        [OnInspectorInit]
#endif
        private void Rebuild()
        {
            foreach (ProfileSubject profileSubject in Subjects)
            {
                foreach (SubjectStateVariant profileSubjectVariant in profileSubject.Variants)
                {
                    void Callback()
                    {
                        if (Application.isPlaying)
                            SetValue(profileSubject.Key, profileSubjectVariant.Key);
                        else
                            SetValueEditor(profileSubject.Key, profileSubjectVariant.Key);
                    }

                    profileSubjectVariant.Bind(Callback);
                }
            }
            Targets = GetComponentsInChildren<ProfileTarget>(true);
        }
        
        public void SetValueImmediate(string key, string value, bool force = false)
        {
            if (m_stateBag.ContainsKey(key))
            {
                if (m_stateBag.TryGetValue(key, out string cState) && cState == value)
                {
                    return;
                }
            }

            if (!GetSubject(key, out ProfileSubject subject))
                return;

            if (force)
                subject.ClearCache();

            if (!subject.Cached)
                subject.Cache();

            if (!subject.TryGetVariant(value, out SubjectStateVariant variant))
            {
                return;
            }

            m_stateBag[key] = value;

            for (int i = variant.Operations.Length - 1; i >= 0; i--)
            {
                IProfileOperation op = variant.Operations[i];
                op.Apply(this);
            }
        }

        public UniTask SetValue(string key, string value, CancellationToken ct = default)
        {
            if (m_stateBag.ContainsKey(key))
            {
                if (m_stateBag.TryGetValue(key, out string cState) && cState == value)
                    return UniTask.CompletedTask;
            }

            if (!GetSubject(key, out ProfileSubject subject))
                return UniTask.CompletedTask;

            if (!subject.Cached)
                subject.Cache();

            if (!subject.TryGetVariant(value, out SubjectStateVariant variant))
                return UniTask.CompletedTask;

            m_stateBag[key] = value;

            StopPreviousOperations(subject, value);

            var linkedCt = CancellationTokenSource.CreateLinkedTokenSource(Lifetime.Token, ct);

            return variant.ApplyMode == ApplyMode.Parallel
                ? RunParallel(variant.Operations, linkedCt.Token)
                : RunSequential(variant.Operations, linkedCt.Token);
        }

        private void StopPreviousOperations(ProfileSubject subject, string value)
        {
            foreach (SubjectStateVariant subjectStateVariant in subject.Variants.Where(a => a.Key != value))
            foreach (IProfileOperation profileOperation in subjectStateVariant.Operations)
                profileOperation.Cancel();
        }

        private UniTask RunParallel(IReadOnlyList<IProfileOperation> operations, CancellationToken token)
        {
            var tasks = new UniTask[operations.Count];
            for (int i = 0; i < tasks.Length; i++)
                tasks[i] = operations[i].ApplyAsync(this, token);
            return UniTask.WhenAll(tasks);
        }

        private async UniTask RunSequential(IEnumerable<IProfileOperation> operations, CancellationToken token)
        {
            foreach (IProfileOperation profileOperation in operations)
                await profileOperation.ApplyAsync(this, token);
        }

        private bool GetSubject(string key, out ProfileSubject subject)
        {
            return m_profileSubjects.TryGetValue(key, out subject);
        }
    }
}