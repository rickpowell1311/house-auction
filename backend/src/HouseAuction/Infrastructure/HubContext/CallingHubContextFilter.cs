using Microsoft.AspNetCore.SignalR;

namespace HouseAuction.Infrastructure.HubContext
{
    public class CallingHubContextFilter : IHubFilter
    {
        private readonly CallingHubContext _context;

        public CallingHubContextFilter(CallingHubContext context)
        {
            _context = context;
        }

        public async ValueTask<object> InvokeMethodAsync(
            HubInvocationContext invocationContext, Func<HubInvocationContext, ValueTask<object>> next)
        {
            _context.Hub = invocationContext.Hub;

            return await next(invocationContext);
        }

        public async Task OnConnectedAsync(HubLifetimeContext context, Func<HubLifetimeContext, Task> next)
        {
            _context.Hub = context.Hub;

            await next(context);
        }

        public async Task OnDisconnectedAsync(HubLifetimeContext context, Exception exception, Func<HubLifetimeContext, Exception, Task> next)
        {
            _context.Hub = context.Hub;

            await next(context, exception);
        }
    }
}
