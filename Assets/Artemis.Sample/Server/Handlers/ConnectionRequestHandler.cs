using System;
using UnityEngine;
using Artemis.Sample;
using Artemis.Sample.Core;
using Artemis.Sample.Player;
using Artemis.Sample.ValueObjects;
using Artemis.UserInterface;
using Color = Artemis.Sample.ValueObjects.Color;
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
        var position = new Float2(Dice.RollRange(-1f, +1f), Dice.RollRange(-1f, +1f));
        var hsv = Color.FromHSV(Dice.RollRange(0f, 1f), 1, 1);
        var playerData = new PlayerData(Guid.NewGuid(), request.Sender, request.Payload.Nickname, hsv, position);
        BroadcastPlayerJoinedNotification(playerData.Id, playerData.Nickname, playerData.Color, playerData.Position);
        request.Reply(new ConnectionResponse(playerData.Id, playerData.Nickname, playerData.Color, position));
        _server._players.Add(playerData);

        Debug.Log($"<b>[S]</b> Client {request.Sender} Connected!");
    }

    private void BroadcastPlayerJoinedNotification(Guid playerId, string nickname, Color color, Float2 position)
    {
        var notification = new PlayerJoinedMessage(playerId, nickname, color, position);
        
        foreach (var player in _server._players)
        {
            _server._client.SendReliableMessage(notification, player.Address);
        }
    }
}