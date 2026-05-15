using System;

namespace UElements.NavigationBar
{
    public abstract class ElementNavigationTabBase<TModel> : ModelElement<TModel>, INavigationTab where TModel : INavigationModel
    {
        public abstract event Action SwitchRequested;
        public abstract void SetState(bool state);
        public abstract void SetInitialState(bool state);
    }
}