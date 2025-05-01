using R3;

namespace UElements.NavigationBar
{
    public abstract class NavigationSwitcherView<TPageModel> : ModelElement<TPageModel>
        where TPageModel : INavigationPageModel
    {
        public ReactiveCommand<TPageModel> OnSwitchRequest { get; } = new();

        protected void Switch()
        {
            OnSwitchRequest.Execute(Model);
        }

        public void SetSelected(bool state)
        {
            OnSetSelected(state);
        }

        protected virtual void OnSetSelected(bool state)
        {
            //Ignore 
        }
    }
}