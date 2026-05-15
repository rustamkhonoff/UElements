// -------------------------------------------------------------------
// Author: Shokhrukhkhon Rustamkhonov
// Date: 15.05.2026
// Description:
// -------------------------------------------------------------------
using System.Threading;

namespace UElements
{
    public readonly struct ElementCreateOptions
    {
        public ElementCreateOptions(
            ElementRequest? request = null,
            CancellationToken lifetimeToken = default,
            CancellationToken creationToken = default
        )
        {
            Request = request ?? ElementRequest.Default;
            LifetimeToken = lifetimeToken;
            CreationToken = creationToken;
        }

        public ElementRequest Request { get; }
        public CancellationToken LifetimeToken { get; }
        public CancellationToken CreationToken { get; }

        public ElementCreateOptions WithRequest(ElementRequest request)
        {
            return new ElementCreateOptions(request, LifetimeToken, CreationToken);
        }

        public ElementCreateOptions WithLifetime(CancellationToken token)
        {
            return new ElementCreateOptions(Request, token, CreationToken);
        }

        public ElementCreateOptions WithCreationToken(CancellationToken token)
        {
            return new ElementCreateOptions(Request, LifetimeToken, token);
        }

        public static ElementCreateOptions Default => new();
    }
}