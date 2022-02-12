using Artemis.UserInterface;

public class PingRequestHandler : IRequestHandler<Ping>
{
    public void Handle(Request<Ping> request)
    {
        request.Reply(new Pong());
    }
}