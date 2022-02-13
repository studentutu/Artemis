using System;
using Artemis.Sample.Core;
using Artemis.Sample.Player;
using Artemis.Sample.Server.Core;
using Artemis.Sample.ValueObjects;
using Artemis.UserInterface;
using UnityEngine;
using Color = Artemis.Sample.ValueObjects.Color;

namespace Artemis.Sample.Server.Handlers
{
    public class ConnectionRequestHandler : IRequestHandler<ConnectionRequest>
    {
        private readonly DapperServer _dapperServer;

        private bool IsSomethingEnabled;
    
        public ConnectionRequestHandler(DapperServer dapperServer)
        {
            _dapperServer = dapperServer;
        }

        public void Handle(Request<ConnectionRequest> request)
        {
            var position = new Float2(Dice.RollRange(-1f, +1f), Dice.RollRange(-1f, +1f));
            var hsv = Color.FromHSV(Dice.RollRange(0f, 1f), 1, 1);
            var playerData = new PlayerData(Guid.NewGuid(), request.Payload.Nickname, hsv, position);
            BroadcastPlayerJoinedNotification(playerData);
            request.Reply(new ConnectionResponse(playerData));
            _dapperServer._players.Add((request.Sender,playerData));

            Debug.Log($"<b>[S]</b> Client {request.Sender} Connected!");
        }

        private void BroadcastPlayerJoinedNotification(PlayerData playerData)
        {
            var notification = new PlayerJoinedMessage(playerData);
        
            foreach (var player in _dapperServer._players)
            {
                _dapperServer._client.SendReliableMessage(notification, player.Item1);
            }
        }
    }
}