using System;
using Cysharp.Threading.Tasks;

namespace UElements.NavigationBar
{
    public interface INavigationTab
    {
        event Action SwitchRequested;
        void SetInitialState(bool state);
        void SetState(bool state);
        UniTask Close();
    }
}