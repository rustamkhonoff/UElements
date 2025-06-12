using UnityEngine;
using UnityEngine.UI;

namespace UElements.States
{
    public class ImageSpriteStatedElement : StatedElement
    {
        [SerializeField] private Image _image;
        [SerializeField] private StatedData<Sprite> _sprites;

        protected override void OnSetState(State state, bool animate)
        {
            _image.sprite = _sprites.Get(state);
        }
    }
}