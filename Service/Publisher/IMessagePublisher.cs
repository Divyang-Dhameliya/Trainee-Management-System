public interface IMessagePublisher
{
    Task PublishAsync<T>(T message);
}