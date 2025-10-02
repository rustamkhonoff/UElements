using System;

namespace UElements.NavigationBar
{
    public interface INavigationTab : IDisposable
    {
        void RegisterRequest(Action switchRequest);
        void SetState(bool state);
        void SetInitialState(bool state);
    }

    public abstract class NavigationTab<TModel> : ModelElement<TModel>, INavigationTab where TModel : INavigationModel
    {
        public abstract void RegisterRequest(Action switchRequest);
        public abstract void SetState(bool state);
        public abstract void SetInitialState(bool state);
    }
}