using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using Object = UnityEngine.Object;
using static UElements.UElementsExtensions;

namespace UElements
{
    public static class ModelElementHelper
    {
        private static MethodInfo _modelInitializeMethodInfo;

        [RuntimeInitializeOnLoadMethod]
        private static void CacheMethodInfo()
        {
            _modelInitializeMethodInfo = typeof(ModelElementHelper).GetMethod(nameof(Initialize), BindingFlags.Static | BindingFlags.NonPublic);
        }

        private static void InitializeElementWithModel(Type modelType, object instance, object model)
        {
            _modelInitializeMethodInfo.MakeGenericMethod(modelType).Invoke(null, new[] { instance, model });
        }

        public static bool TryInitializeModel(object instance, object model)
        {
            Type modelType = GetGenericBaseTypeArgument(instance.GetType(), typeof(ModelElement<>));
            if (modelType != null && modelType == model.GetType())
            {
                InitializeElementWithModel(modelType, instance, model);
                return true;
            }

            return false;
        }

        public static async UniTask<ElementBase> CreateElementWithQueryParams(
            this IElements elements,
            ElementRequest request,
            Dictionary<string, string> modelParams)
        {
            if (modelParams.Count <= 0) return await ElementsGlobal.Instance.Create(request);

            Type type = await elements.GetElementTypeForRequest(request);

            if (type == null || !IsSubclassOfRawGeneric(type, typeof(ModelElement<>)))
                return await ElementsGlobal.Instance.Create(request);

            Type modelType = GetGenericBaseTypeArgument(type, typeof(ModelElement<>));
            if (modelType == null)
            {
                Debug.LogException(new InvalidCastException("Model from params"));
                return await ElementsGlobal.Instance.Create(request);
            }

            string json = JsonConvert.SerializeObject(modelParams);
            object model = JsonConvert.DeserializeObject(json, modelType);
            return await ElementsGlobal.Instance.Create(model, request);
        }

        private static void Initialize<T>(ModelElement<T> element, T model)
        {
            element.InitializeModel(model);
        }
    }

    public static class UElementsExtensions
    {
        public static string GetKey<T>(ElementRequest? elementRequest) where T : ElementBase
        {
            if (elementRequest.HasValue && !string.IsNullOrEmpty(elementRequest.Value.Key))
            {
                return elementRequest.Value.Key;
            }
            else if (Attribute.GetCustomAttribute(typeof(T), typeof(ElementKeyAttribute)) is { } attribute)
            {
                return ((ElementKeyAttribute)attribute).Key;
            }
            else
            {
                return typeof(T).Name;
            }
        }

        public static string GetKey(GameObject gameObject)
        {
            if (gameObject.TryGetComponent(out ElementBase elementBase))
            {
                if (Attribute.GetCustomAttribute(elementBase.GetType(), typeof(ElementKeyAttribute)) is { } attribute)
                {
                    return ((ElementKeyAttribute)attribute).Key;
                }
                else
                {
                    return elementBase.GetType().Name;
                }
            }

            return "null";
        }

        public static bool IsSubclassOfRawGeneric(Type type, Type genericBase)
        {
            while (type != null && type != typeof(object))
            {
                var cur = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
                if (cur == genericBase)
                    return true;

                type = type.BaseType;
            }

            return false;
        }

        public static Type GetGenericBaseTypeArgument(Type type, Type genericBase)
        {
            while (type != null && type != typeof(object))
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == genericBase)
                    return type.GetGenericArguments()[0];

                type = type.BaseType;
            }

            return null;
        }

        public static bool IsEmptyKey(string key)
        {
            return string.IsNullOrEmpty(key) || key == "null";
        }
#if UNITY_EDITOR
        public static string GetResourcesPath(Object asset)
        {
            string fullPath = UnityEditor.AssetDatabase.GetAssetPath(asset);
            int resourcesIndex = fullPath.IndexOf("/Resources/", StringComparison.Ordinal);

            if (resourcesIndex == -1)
            {
                Debug.LogError("Asset is not located in a Resources folder.");
                return null;
            }

            string relativePath = fullPath[(resourcesIndex + "/Resources/".Length)..];
            relativePath = Path.ChangeExtension(relativePath, null);
            return relativePath;
        }
#endif
    }
}