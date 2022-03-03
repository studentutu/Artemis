using Artemis.UserInterface;

namespace Artemis.Samples.PingPong
{
    internal class PingRequestHandler : IRequestHandler<Ping>
    {
        public void Handle(Request<Ping> request) => request.Reply(Pong.Instance);
    }
}