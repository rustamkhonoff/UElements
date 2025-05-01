#if ODIN_INSPECTOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace UElements.CollectionView.Editor
{
    public class ODIN_CollectionItemRequestProcessor : OdinAttributeProcessor<CollectionItemRequest>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (IsValidPropertyName(member.Name, nameof(CollectionItemRequest.Key), nameof(CollectionItemRequest.Parent)))
                attributes.Add(new HorizontalGroupAttribute());
        }

        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        {
            attributes.Add(new InlinePropertyAttribute());
        }

        private bool IsValidPropertyName(string memberName, params string[] requiredNames) =>
            requiredNames.Any(requiredName => memberName == ToValidPropertyName(requiredName));

        private string ToValidPropertyName(string name) =>
            $"<{name}>k__BackingField";
    }
}
#endif