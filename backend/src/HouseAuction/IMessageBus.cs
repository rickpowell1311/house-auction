namespace HouseAuction
{
    public interface IMessageBus
    {
        Task Send<T>(T message);
    }
}
