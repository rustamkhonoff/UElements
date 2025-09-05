using System;
using R3;

namespace Demos.CollectionView
{
    [Serializable]
    public class Model
    {
        public SerializableReactiveProperty<string> Nickname = new();
        public SerializableReactiveProperty<int> Health = new();

        public Model() { }

        public Model(int health, string name)
        {
            Nickname.Value = name;
            Health.Value = health;
        }

        public void IncreaseAmount()
        {
            Health.Value += 1;
        }

        public Observable<(string name, int health)> AnyValueChanged => Nickname.CombineLatest(Health, (a, b) => (a, b));
    }
}