using UnityEngine;
using Artemis.Sample;
using Artemis.UserInterface;

public class ConnectionRequestHandler : IRequestHandler<ConnectionRequest>
{
    private readonly Server _server;

    public ConnectionRequestHandler(Server server)
    {
        _server = server;
    }

    public void Handle(Request<ConnectionRequest> request)
    {
        _server._connections.Add(request.Sender);
        request.Reply(new ConnectionResponse());
        Debug.Log($"<b>[S]</b> Client {request.Sender} Connected!");
    }
}