#if UNITY_LOCALIZATION
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Localization;

namespace UElements.Profiles
{
    [Serializable]
    public sealed class SetLocalizationStringOperation : BaseProfileOperation<LocalizeStringTarget, LocalizedString>
    {
        protected override UniTask OnApplyAsync(LocalizeStringTarget target, LocalizedString value, CancellationToken ct)
        {
            target.Value.StringReference.SetReference(value.TableReference, value.TableEntryReference);
            return UniTask.CompletedTask;
        }

        protected override void OnApply(LocalizeStringTarget target, LocalizedString value)
        {
            target.Value.StringReference.SetReference(value.TableReference, value.TableEntryReference);
        }
    }
}
#endif