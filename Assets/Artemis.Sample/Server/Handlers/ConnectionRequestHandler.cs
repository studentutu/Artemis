using System;
using System.Threading.Tasks;
using UnityEngine;
using Artemis.Sample;
using Artemis.Sample.Core;
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
        var playerData = new PlayerData(Guid.NewGuid(), request.Sender, request.Payload.Nickname);
        _server._players.Add(playerData);
        request.Reply(new ConnectionResponse(playerData.Id));
        
        BroadcastPlayerJoinedNotification(playerData.Id, playerData.Nickname);
        Debug.Log($"<b>[S]</b> Client {request.Sender} Connected!");
    }

    private void BroadcastPlayerJoinedNotification(Guid playerId, string nickname)
    {
        foreach (var player in _server._players)
        {
            _server._client.SendReliableMessage(
                new PlayerJoinedMessage(playerId, nickname, player.Id == playerId), player.Address);
        }
    }
}