namespace Artemis.UserInterface
{
    public interface IMessageHandler<T>
    {
        void Handle(Message<T> message);
    }
}