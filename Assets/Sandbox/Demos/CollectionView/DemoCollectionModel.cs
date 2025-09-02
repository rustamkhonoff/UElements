using System;
using R3;

namespace Demos.CollectionView
{
    [Serializable]
    public class DemoCollectionModel
    {
        public SerializableReactiveProperty<string> Nickname = new();
        public SerializableReactiveProperty<int> Health = new();

        public DemoCollectionModel() { }

        public DemoCollectionModel(int health, string name)
        {
            Nickname.Value = name;
            Health.Value = health;
        }

        public void IncreaseAmount()
        {
            Health.Value += 1;
        }

        public Observable<(string, int)> AnyValueChanged => Nickname.CombineLatest(Health, (a, b) => (a, b));
    }
}