using Artemis.UserInterface;

namespace Artemis.Sample.Server.Handlers
{
    public class PingRequestHandler : IRequestHandler<Ping>
    {
        public void Handle(Request<Ping> request)
        {
            request.Reply(new Pong());
        }
    }
}