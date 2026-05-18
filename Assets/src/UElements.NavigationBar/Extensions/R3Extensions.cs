#if R3
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using R3;

// -------------------------------------------------------------------
// Author: Shokhrukhkhon Rustamkhonov
// Date: 18.05.2026
// Description:
// -------------------------------------------------------------------
namespace UElements.NavigationBar.Extensions
{
    public static class R3Extensions
    {
        public static Observable<TModel> ObservePage<TModel>(this INavigation<TModel> navigation, CancellationToken ct)
            where TModel : INavigationModel
        {
            return Observable.FromEvent<TModel>(a => navigation.PageChanged += a, a => navigation.PageChanged -= a, ct);
        }

        public static CancellationTokenRegistration SubscribeToPageWithInitial<TModel>(this INavigation<TModel> navigation, Action<TModel> onNext, CancellationToken ct)
            where TModel : INavigationModel
        {
            Observable<TModel> observable = navigation.ObservePage(ct);
            CancellationTokenRegistration cancellationTokenRegistration = observable.Subscribe(onNext).AddTo(ct);
            onNext(navigation.ActivePage);
            return cancellationTokenRegistration;
        }
    }
}
#endif