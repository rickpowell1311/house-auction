using Microsoft.AspNetCore.SignalR;
using System.Reflection;

namespace HouseAuction.Infrastructure.HubContext
{
    public static class ClientProxyExtensions
    {
        public static T AsReceiver<T>(
            this IClientProxy clientProxy)
            where T : class
        {
            return ClientDispatchProxy<T>.Decorate(clientProxy);
        }

        public class ClientDispatchProxy<T> : DispatchProxy
            where T : class
        {
            private IClientProxy _clientProxy;

            public static T Decorate(IClientProxy target)
            {
                var proxy = Create<T, ClientDispatchProxy<T>>()
                    as ClientDispatchProxy<T>;

                proxy._clientProxy = target;

                return proxy as T;
            }

            protected override object Invoke(MethodInfo targetMethod, object[] args)
            {
                return _clientProxy.SendCoreAsync(targetMethod.Name, args, CancellationToken.None);
            }
        }
    }
}
