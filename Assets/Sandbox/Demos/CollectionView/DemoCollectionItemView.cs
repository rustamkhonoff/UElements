using R3;
using TMPro;
using UElements;
using UnityEngine;
using UnityEngine.UI;

namespace Demos.CollectionView
{
    internal class ModelView : ModelElement<Model>
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Button _click;

        public Observable<Unit> Clicked => m_clicked;
        private Observable<Unit> m_clicked;

        protected override void Initialize()
        {
            m_clicked = _click.OnClickAsObservable();
        }

        public void SetText(string text)
        {
            _text.SetText(text);
        }
    }
}