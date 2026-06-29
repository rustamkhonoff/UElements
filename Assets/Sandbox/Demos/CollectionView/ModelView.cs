using System.Threading;
using R3;
using TMPro;
using UElements;
using UnityEngine;
using UnityEngine.UI;

namespace Demos.CollectionView
{
    internal class ModelView : Element
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Button _click;

        public Observable<Unit> Clicked { get; private set; }

        protected override void Initialize(CancellationToken ct)
        {
            Clicked = _click.OnClickAsObservable();
        }

        public void SetText(string text)
        {
            _text.SetText(text);
        }
    }
}