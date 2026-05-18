using System;

namespace UElements.NavigationBar
{
    public interface INavigationPresenter : IDisposable
    {
        event Action<object> ContentCreated;
        bool TrySwitch(string key);
    }
}