namespace UElements
{
    public class ModelElement<T> : ElementBase
    {
        public T Model { get; internal set; }

        internal void InitializeModel(T model)
        {
            Model = model;
        }
    }
}