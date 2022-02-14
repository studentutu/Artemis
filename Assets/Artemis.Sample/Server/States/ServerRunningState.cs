using System;
using System.Threading;
using Artemis.Sample.Server.Core;
using Artemis.Sample.Server.Handlers;
using UnityEngine;

namespace Artemis.Sample.Server.States
{
    public class ServerRunningState : AServerState
    {
        private Thread _gameLoopThread;
        
        public override void OnStateEntered(DapperServer dapperServer)
        {
            dapperServer.Tick = 0;
            
            dapperServer._client.RegisterHandler(new PingRequestHandler());
            dapperServer._client.RegisterHandler(new GetTimeRequestHandler(dapperServer));
            dapperServer._client.RegisterHandler(new GetOthersRequestHandler(dapperServer));
            dapperServer._client.RegisterHandler(new ConnectionRequestHandler(dapperServer));
            dapperServer._client.RegisterHandler(new PlayerCommandMessageHandler(dapperServer));
            dapperServer._client.RegisterHandler(new ClientDisconnectionMessageHandler(dapperServer));
            
            _gameLoopThread = new Thread(() => ServerLoop(dapperServer));
            _gameLoopThread.Start();
        }
        
        private void ServerLoop(DapperServer dapperServer)
        {
            dapperServer.TimeAtFirstTick = DateTime.UtcNow;
            
            while (true)
            {
                var elapsed = (DateTime.UtcNow - dapperServer.TimeAtFirstTick).TotalSeconds;
                dapperServer.Tick = (int) (elapsed * Configuration.TicksPerSecond);
            }
        }

        public override void OnGUI(DapperServer dapperServer)
        {
            GUILayoutUtilities.Button(nameof(Shutdown), () => Shutdown(dapperServer));
            GUILayout.Label($"Tick: {dapperServer.Tick}");
        }

        public override void OnDestroy(DapperServer dapperServer)
        {
            Debug.Log("OnDestroyServer");
            Shutdown(dapperServer);
        }

        private void Shutdown(DapperServer dapperServer)
        {
            _gameLoopThread.Abort();
            
            foreach (var player in dapperServer._players)
            {
                dapperServer._client.SendUnreliableMessage(new ServerClosingMessage(), player.Item1);
            }
            
            dapperServer._client.Dispose();
            dapperServer._client = null;
            dapperServer.Switch(dapperServer.Stopped);
        }
    }
}