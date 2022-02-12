namespace Artemis.UserInterface
{
    public interface IRequestHandler<T>
    {
        void Handle(Request<T> request);
    }
}