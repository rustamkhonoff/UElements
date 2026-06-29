// -------------------------------------------------------------------
// Author: Shokhrukhkhon Rustamkhonov
// Date: 01.05.2026
// Description:
// -------------------------------------------------------------------
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;

namespace UElements.Profiles
{
    public static class ProfileControllerExtensions
    {
        public static IDisposable SubscribeBool(this Observable<bool> source, ProfileController controller, string key)
        {
            return source.Subscribe(controller, (s, c) => c.SetBool(key, s));
        }

        public static IDisposable BindValueWithImmediateFirst<T>(this Observable<T> source, ProfileController controller, string key, Func<T, string> selector)
        {
            bool first = true;

            return source.Subscribe(value =>
            {
                string state = selector(value);

                if (first)
                {
                    first = false;
                    controller.SetValueImmediate(key, state);
                }
                else
                {
                    controller.SetValue(key, state).Forget();
                }
            });
        }

        public static IDisposable SubscribeBoolWithImmediateFirst(this Observable<bool> source, ProfileController controller, string key)
        {
            return source.BindValueWithImmediateFirst(controller, key, a => a ? "True" : "False");
        }

        public static UniTask SetValue(this ProfileController controller, StateNames name, StateValues value, CancellationToken ct)
        {
            return controller.SetValue(name.ToString(), value.ToString(), ct);
        }

        public static UniTask SetValue(this ProfileController controller, string name, StateValues value, CancellationToken ct)
        {
            return controller.SetValue(name, value.ToString(), ct);
        }

        public static UniTask SetValue(this ProfileController controller, StateNames name, string value, CancellationToken ct)
        {
            return controller.SetValue(name.ToString(), value, ct);
        }

        public static UniTask SetBool(this ProfileController controller, string key, bool state, CancellationToken ct = default)
        {
            string value = state ? "True" : "False";
            return controller.SetValue(key, value, ct);
        }

        public static void SetBoolImmediate(this ProfileController controller, string key, bool state)
        {
            string value = state ? "True" : "False";
            controller.SetValueImmediate(key, value);
        }
    }
}