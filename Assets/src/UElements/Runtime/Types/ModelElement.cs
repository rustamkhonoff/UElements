namespace UElements
{
    public class ModelElement<T> : ElementBase
    {
        public T Model { get; set; }

        internal void InitializeModel(T model)
        {
            Model = model;
        }
    }
}