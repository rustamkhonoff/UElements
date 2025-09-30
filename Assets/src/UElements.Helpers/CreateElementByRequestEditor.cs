#if UNITY_EDITOR
using UnityEditor;

namespace UElements.Helpers
{
    [CustomEditor(typeof(CreateElementByRequest))]
    internal class CreateElementByRequestEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            SerializedProperty request = serializedObject.FindProperty(nameof(CreateElementByRequest._request));
            SerializedProperty modelType = serializedObject.FindProperty(nameof(CreateElementByRequest._modelType));
            SerializedProperty paramsProp = serializedObject.FindProperty(nameof(CreateElementByRequest._params));
            SerializedProperty query = serializedObject.FindProperty(nameof(CreateElementByRequest._query));
            SerializedProperty start = serializedObject.FindProperty(nameof(CreateElementByRequest._createOnStart));

            EditorGUILayout.PropertyField(request);
            EditorGUILayout.PropertyField(modelType);
            EditorGUILayout.PropertyField(start);

            CreateElementByRequest.ModelType type =
                (CreateElementByRequest.ModelType)modelType.enumValueIndex;

            switch (type)
            {
                case CreateElementByRequest.ModelType.Params:
                    EditorGUILayout.PropertyField(paramsProp, true);
                    break;

                case CreateElementByRequest.ModelType.Query:
                    EditorGUILayout.PropertyField(query);
                    break;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif