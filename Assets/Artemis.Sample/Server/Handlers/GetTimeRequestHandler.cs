using System;
using Artemis.UserInterface;
using Artemis.Sample.Features.ClockSynchonization;

public class GetTimeRequestHandler : IRequestHandler<GetTimeRequest>
{
    private readonly Server _server;

    public GetTimeRequestHandler(Server server)
    {
        _server = server;
    }
    
    public void Handle(Request<GetTimeRequest> request)
    {
        var response = new GetTimeResponse(_server.Tick, DateTime.UtcNow, _server.TimeAtFirstTick);
        request.Reply(response);
    }
}