using HouseAuction.Bidding.Domain.Events;
using Microsoft.EntityFrameworkCore;
using Onwrd.EntityFrameworkCore;

namespace HouseAuction.Bidding
{
    public static class ServiceCollectionExtensions
    {
        public static void AddBidding(this IServiceCollection services)
        {
            services.AddDbContext<BiddingContext>(
                (serviceProvider, builder) =>
                {
                    builder.UseInMemoryDatabase("Bidding");
                },
                onwrdConfig =>
                {
                    onwrdConfig.UseOnwardProcessors(x =>
                    {
                        x.Register<PlayerFinishedBidding, OnPlayerFinishedBidding>();
                        x.Register<BiddingRoundComplete, OnBiddingRoundComplete>();
                    });
                },
                contextLifetime: ServiceLifetime.Transient);
        }
    }
}
