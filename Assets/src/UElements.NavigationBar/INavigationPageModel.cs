namespace UElements.NavigationBar
{
    public interface INavigationPageModel
    {
        string Key { get; }
        ElementRequest ContentRequest { get; }
    }
}