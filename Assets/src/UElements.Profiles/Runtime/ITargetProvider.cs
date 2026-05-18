namespace UElements.Profiles
{
    public interface ITargetProvider
    {
        public bool TryGet<T>(string key, out T target) where T : ProfileTarget;
    }
}