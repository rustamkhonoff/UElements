#if ODIN_INSPECTOR
using System;
using Sirenix.OdinInspector.Editor;
using UElements.Profiles.Editor;

[assembly: OdinVisualDesignerAttributeItem("Custom/Dropdown", typeof(ConditionalValueDropdownAttribute))]

namespace UElements.Profiles.Editor
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ConditionalValueDropdownAttribute : Attribute
    {
        public string Condition;
        public string Values;

        public bool IncludeCurrentValueIfMissing = true;

        public ConditionalValueDropdownAttribute() { }

        public ConditionalValueDropdownAttribute(string condition, string values)
        {
            Condition = condition;
            Values = values;
        }
    }
}

#endif