using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UElements
{
    public class GameObjectActiveElementController : ElementsControllerBase
    {
        [SerializeField] private GameObject _gameObject;

        public override UniTask Show(ElementBase element)
        {
            _gameObject.SetActive(true);
            return UniTask.CompletedTask;
        }

        public override UniTask Hide(ElementBase element)
        {
            _gameObject.SetActive(true);
            return UniTask.CompletedTask;
        }
    }
}