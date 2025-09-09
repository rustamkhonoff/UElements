#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace UElements.Helpers
{
    [CustomPropertyDrawer(typeof(Param))]
    internal class ParamDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            float half = position.width / 2f;
            Rect keyRect = new(position.x, position.y, half - 2, position.height);
            Rect valueRect = new(position.x + half + 2, position.y, half - 2, position.height);

            EditorGUI.PropertyField(keyRect, property.FindPropertyRelative("<Key>k__BackingField"), GUIContent.none);
            EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("<Value>k__BackingField"), GUIContent.none);

            EditorGUI.EndProperty();
        }
    }
}
#endif