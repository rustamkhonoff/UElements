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

        public static IDisposable BindValueWithImmediateFirst<T>(this Observable<T> source, ProfileController controller, StateNames key, Func<T, string> selector)
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

        public static IDisposable BindValueWithImmediateFirst<T>(this Observable<T> source, ProfileController controller, string key, Func<T, StateValues> selector)
        {
            bool first = true;

            return source.Subscribe(value =>
            {
                StateValues state = selector(value);

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

        public static IDisposable BindValueWithImmediateFirst<T>(this Observable<T> source, ProfileController controller, StateNames name, Func<T, StateValues> selector)
        {
            bool first = true;

            return source.Subscribe(value =>
            {
                StateValues state = selector(value);

                if (first)
                {
                    first = false;
                    controller.SetValueImmediate(name.ToString(), state.ToString());
                }
                else
                {
                    controller.SetValue(name, state).Forget();
                }
            });
        }

        public static IDisposable SubscribeBoolWithImmediateFirst(this Observable<bool> source, ProfileController controller, StateNames name)
        {
            return source.BindValueWithImmediateFirst(controller, name, a => a ? StateValues.True : StateValues.False);
        }

        public static UniTask SetValue(this ProfileController controller, StateNames name, StateValues value, CancellationToken ct = default)
        {
            return controller.SetValue(name.ToString(), value.ToString(), ct);
        }

        public static void SetValueImmediate(this ProfileController controller, StateNames name, StateValues value)
        {
            controller.SetValueImmediate(name.ToString(), value.ToString());
        }

        public static UniTask SetValue(this ProfileController controller, string name, StateValues value, CancellationToken ct = default)
        {
            return controller.SetValue(name, value.ToString(), ct);
        }

        public static void SetValueImmediate(this ProfileController controller, string name, StateValues value)
        {
            controller.SetValueImmediate(name, value.ToString());
        }

        public static UniTask SetValue(this ProfileController controller, StateNames name, string value, CancellationToken ct = default)
        {
            return controller.SetValue(name.ToString(), value, ct);
        }

        public static void SetValueImmediate(this ProfileController controller, StateNames name, string value)
        {
            controller.SetValueImmediate(name.ToString(), value);
        }

        public static UniTask SetBool(this ProfileController controller, string key, bool state, CancellationToken ct = default)
        {
            return controller.SetValue(key, state ? StateValues.True : StateValues.False, ct);
        }

        public static void SetBoolImmediate(this ProfileController controller, string key, bool state)
        {
            controller.SetValue(key, state ? StateValues.True : StateValues.False);
        }

        public static UniTask SetBool(this ProfileController controller, StateNames name, bool state, CancellationToken ct = default)
        {
            return controller.SetValue(name, state ? StateValues.True : StateValues.False, ct);
        }

        public static void SetBoolImmediate(this ProfileController controller, StateNames name, bool state)
        {
            controller.SetValueImmediate(name, state ? StateValues.True : StateValues.False);
        }
    }
}