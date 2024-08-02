using Microsoft.AspNetCore.SignalR;

namespace HouseAuction
{
    public class HubExceptionNotifierFilter : IHubFilter
    {
        public async ValueTask<object> InvokeMethodAsync(
            HubInvocationContext invocationContext, Func<HubInvocationContext, ValueTask<object>> next)
        {
            try
            {
                return await next(invocationContext);
            }
            catch (HubException ex)
            {
                var hub = (invocationContext.Hub as Hub<IHouseAuctionReceiver>);
                hub?.Clients.Caller.NotifyError(ex.Message);
                throw;
            }
        }
    }
}
