// -------------------------------------------------------------------
// Author: Shokhrukhkhon Rustamkhonov
// Date: 27.01.2026
// Description:
// -------------------------------------------------------------------

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UElements.Profiles
{
    [Serializable]
    public sealed class SetImageSpriteOperation : BaseProfileOperation<ImageTarget, Sprite>
    {
        protected override void OnApply(ImageTarget target, Sprite value)
        {
            target.Value.sprite = value;
        }

        protected override UniTask OnApplyAsync(ImageTarget target, Sprite value, CancellationToken ct = default)
        {
            target.Value.sprite = value;
            return UniTask.CompletedTask;
        }
    }
}