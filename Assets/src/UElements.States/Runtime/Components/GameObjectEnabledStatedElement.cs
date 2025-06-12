using UnityEngine;

namespace UElements.States
{
    public class GameObjectEnabledStatedElement : StatedElement
    {
        [SerializeField] private StatedData<bool> _events;
        [SerializeField] private GameObject[] _gameObjects;

        protected override void OnSetState(State state, bool animate)
        {
            bool goState = _events.Get(state);
            foreach (GameObject o in _gameObjects) o.SetActive(goState);
        }
    }
}