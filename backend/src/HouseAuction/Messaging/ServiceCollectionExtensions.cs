using System.Reflection;

namespace HouseAuction.Messaging
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMessaging(this IServiceCollection services)
        {
            services.AddSingleton<IMessageBus, MessageBus>();
            services.AddSingleton<Exchange>();

            var subscriberTypes = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() 
                        == typeof(IMessageSubscriber<>)));

            foreach (var subscriberType in subscriberTypes)
            {
                services.AddTransient(typeof(IMessageSubscriber), subscriberType);
            }
        }
    }
}
