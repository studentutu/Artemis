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
        _server._players.Add(playerData);
        request.Reply(new ConnectionResponse(playerData.Id));

        BroadcastPlayerJoinedNotification(playerData.Id, playerData.Nickname, playerData.Color, x, y);
        Debug.Log($"<b>[S]</b> Client {request.Sender} Connected!");
    }

    private void BroadcastPlayerJoinedNotification(Guid playerId, string nickname, HSV color, float x, float y)
    {
        foreach (var player in _server._players)
        {
            _server._client.SendReliableMessage(
                new PlayerJoinedMessage(playerId, nickname, player.Id == playerId, color, x, y), player.Address);
        }
    }
}