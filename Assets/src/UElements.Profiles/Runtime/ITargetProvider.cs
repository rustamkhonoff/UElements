namespace UElements.Profiles
{
    public interface ITargetProvider
    {
        public bool TryGet<T>(string id, out T target) where T : ProfileTarget;
    }
}