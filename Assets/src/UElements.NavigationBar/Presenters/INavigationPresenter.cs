using System;

namespace UElements.NavigationBar
{
    public interface INavigationPresenter
    {
        event Action<object> ContentCreated;
        bool TrySwitch(string key);
        void Dispose();
    }
}