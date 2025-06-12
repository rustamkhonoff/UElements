using UnityEngine;
using UnityEngine.UI;

namespace UElements.States
{
    public class ColorStatedElement : StatedElement
    {
        [SerializeField] private Graphic _graphic;
        [SerializeField] private StatedData<Color> _colors;

        protected override void OnSetState(State state, bool animate)
        {
            _graphic.color = _colors.Get(state);
        }
    }
}