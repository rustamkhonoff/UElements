using System;

namespace UElements
{
    public class ElementKeyAttribute : Attribute
    {
        public string Key { get; }

        public ElementKeyAttribute(string key)
        {
            Key = key;
        }
    }
}