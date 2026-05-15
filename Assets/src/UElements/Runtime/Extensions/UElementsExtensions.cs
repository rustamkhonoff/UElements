using System;
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UElements
{
    public static class UElementsExtensions
    {
        public static void Close<T>(this IElements elements) where T : ElementBase => elements.CloseAll<T>();

        public static void SafeDispose(this ElementBase elementBase)
        {
            if (elementBase == null) return;
            elementBase.AsCloseDisposable().Dispose();
        }

        internal static void BindCloseToLifetime(ElementBase instance, CancellationToken token)
        {
            if (!token.CanBeCanceled)
                return;

            if (token.IsCancellationRequested)
            {
                instance.Close().Forget();
                return;
            }

            CancellationTokenRegistration registration = token.Register(() =>
            {
                UniTask.Void(async () =>
                {
                    await UniTask.SwitchToMainThread();

                    try
                    {
                        if (instance != null)
                            await instance.Close();
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                });
            });
            instance.LifetimeToken.Register(() => registration.Dispose());
        }

        public static UniTask<T> Create<T>(this IElements elements, ElementRequest? request = null, CancellationToken createToken = default, CancellationToken lifetimeToken = default)
            where T : Element
        {
            return elements.Create<T>(new ElementCreateOptions(request, lifetimeToken, createToken));
        }

        public static UniTask<T> Create<T, TModel>(
            this IElements elements,
            TModel model, ElementRequest? request = null, CancellationToken createToken = default,
            CancellationToken lifetimeToken = default
        ) where T : ModelElement<TModel>
        {
            return elements.Create<T, TModel>(model, new ElementCreateOptions(request, lifetimeToken, createToken));
        }

        public static UniTask<ElementBase> Create(this IElements elements, ElementRequest request, CancellationToken createToken = default, CancellationToken lifetimeToken = default)
        {
            return elements.Create(new ElementCreateOptions(request, lifetimeToken, createToken));
        }

        public static UniTask<ElementBase> Create(
            this IElements elements,
            object model, ElementRequest request, CancellationToken createToken = default,
            CancellationToken lifetimeToken = default
        )
        {
            return elements.Create(model, new ElementCreateOptions(request, lifetimeToken, createToken));
        }

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