using System;
using Artemis.Sample.Features.ClockSynchonization;
using Artemis.Sample.Server.Core;
using Artemis.UserInterface;

namespace Artemis.Sample.Server.Handlers
{
    public class GetTimeRequestHandler : IRequestHandler<GetTimeRequest>
    {
        private readonly DapperServer _dapperServer;

        public GetTimeRequestHandler(DapperServer dapperServer)
        {
            _dapperServer = dapperServer;
        }
    
        public void Handle(Request<GetTimeRequest> request)
        {
            var response = new GetTimeResponse(_dapperServer.Tick, DateTime.UtcNow, _dapperServer.TimeAtFirstTick);
            request.Reply(response);
        }
    }
}