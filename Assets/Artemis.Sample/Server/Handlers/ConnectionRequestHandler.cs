using System;
using UnityEngine;
using Artemis.Sample;
using Artemis.Sample.Core;
using Artemis.Sample.Player;
using Artemis.UserInterface;
using Random = System.Random;

public class ConnectionRequestHandler : IRequestHandler<ConnectionRequest>
{
    private readonly Server _server;

    private bool IsSomethingEnabled;
    
    public ConnectionRequestHandler(Server server)
    {
        _server = server;
    }

    public void Handle(Request<ConnectionRequest> request)
    {
        var x = Dice.RollRange(-1f, +1f);
        var y = Dice.RollRange(-1f, +1f);
        var hsv = new HSV(Dice.RollRange(0f, 1f), 1, 1);
        var playerData = new PlayerData(Guid.NewGuid(), request.Sender, request.Payload.Nickname, hsv, x, y);
        BroadcastPlayerJoinedNotification(playerData.Id, playerData.Nickname, playerData.Color, x, y);
        request.Reply(new ConnectionResponse(playerData.Id, playerData.Nickname, playerData.Color, x, y));
        _server._players.Add(playerData);

        Debug.Log($"<b>[S]</b> Client {request.Sender} Connected!");
    }

    private void BroadcastPlayerJoinedNotification(Guid playerId, string nickname, HSV color, float x, float y)
    {
        var notification = new PlayerJoinedMessage(playerId, nickname, color, x, y);
        
        foreach (var player in _server._players)
        {
            _server._client.SendReliableMessage(notification, player.Address);
        }
    }
}