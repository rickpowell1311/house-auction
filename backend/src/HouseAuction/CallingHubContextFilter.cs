using Microsoft.AspNetCore.SignalR;

namespace HouseAuction
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
    }
}
