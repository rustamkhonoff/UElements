#if ODIN_INSPECTOR
using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine.Scripting;

namespace UElements.Editor
{
    [Preserve]
    public class ElementRequestOdinEditor : OdinAttributeProcessor<TypedElementRequest>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            string fieldName = nameof(TypedElementRequest.RequestType);

            if (member.Name == nameof(TypedElementRequest.RequestType))
            {
                attributes.Add(new HorizontalGroupAttribute("1"));
            }


            if (member.Name == nameof(TypedElementRequest.Key))
            {
                attributes.Add(new HorizontalGroupAttribute("1"));
                attributes.Add(new ShowIfAttribute(fieldName, TypedElementRequest.Type.Key));
            }

            if (member.Name == nameof(TypedElementRequest.CustomPrefabReference))
            {
                attributes.Add(new HorizontalGroupAttribute("1"));
                attributes.Add(new ShowIfAttribute(fieldName, TypedElementRequest.Type.Reference));
            }

            if (member.Name == nameof(TypedElementRequest.Parent) || member.Name == nameof(TypedElementRequest.OnlyOneInstance))
            {
                attributes.Add(new HorizontalGroupAttribute("2"));
            }
        }
    }
}
#endif