using Artemis.Sample.Packets;
using Artemis.Sample.Server.Core;
using Artemis.UserInterface;

namespace Artemis.Sample.Server.Handlers
{
    public class PlayerCommandMessageHandler : IMessageHandler<PlayerCommand>
    {
        private readonly DapperServer _server;

        public PlayerCommandMessageHandler(DapperServer server)
        {
            _server = server;
        }
        
        public void Handle(Message<PlayerCommand> message)
        {
            _server.InputBuffer.Enqueue(message);
        }
    }
}