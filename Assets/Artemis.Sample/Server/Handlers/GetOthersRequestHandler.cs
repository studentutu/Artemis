using System.Linq;
using Artemis.Sample.Packets;
using Artemis.Sample.Server.Core;
using Artemis.UserInterface;

namespace Artemis.Sample.Server.Handlers
{
    public class GetOthersRequestHandler : IRequestHandler<GetOthersRequest>
    {
        private readonly DapperServer _server;

        public GetOthersRequestHandler(DapperServer server)
        {
            _server = server;
        }
        
        public void Handle(Request<GetOthersRequest> request)
        {
            var others = _server._players.Where(p => p.Item1 != request.Sender);
            var response = new GetOthersResponse(others.Select(o => o.Item2).ToList());
            request.Reply(response);
        }
    }
}