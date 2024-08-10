namespace HouseAuction
{
    public interface IMessageSubscriber { }

    public interface IMessageSubscriber<T> : IMessageSubscriber
    {
        Task Handle(T message);
    }
}
