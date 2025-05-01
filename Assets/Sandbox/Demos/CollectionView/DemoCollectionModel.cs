using System;
using R3;

namespace Demos.CollectionView
{
    [Serializable]
    public class DemoCollectionModel
    {
        public ReactiveProperty<string> Nickname = new();
        public ReactiveProperty<int> Health = new();

        public DemoCollectionModel(int health,string name)
        {
            Nickname.Value = name;
            Health.Value = health;
        }
        public void IncreaseAmount()
        {
            Health.Value += 1;
        }

        public Observable<Unit> AnyValueChanged => Nickname.CombineLatest(Health, (_, _) => Unit.Default);
    }
}