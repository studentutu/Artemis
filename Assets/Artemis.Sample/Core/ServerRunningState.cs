using System;
using System.Threading;
using Artemis.UserInterface;
using UnityEngine;

namespace Artemis.Sample.Core
{
    public class ServerRunningState : AServerState
    {
        private Thread _gameLoopThread;
        
        public override void OnStateEntered(Server server)
        {
            server.Tick = 0;
            
            server._client.RegisterHandler(new PingRequestHandler());
            server._client.RegisterHandler(new GetTimeRequestHandler(server));
            server._client.RegisterHandler(new ConnectionRequestHandler(server));
            server._client.RegisterHandler(new ClientDisconnectionMessageHandler(server));
            
            _gameLoopThread = new Thread(() => ServerLoop(server));
            _gameLoopThread.Start();
        }
        
        private void ServerLoop(Server server)
        {
            server.TimeAtFirstTick = DateTime.UtcNow;
            
            while (true)
            {
                var elapsed = (DateTime.UtcNow - server.TimeAtFirstTick).TotalSeconds;
                server.Tick = (int) (elapsed * Configuration.TicksPerSecond);
                Thread.Sleep(Configuration.TickInterval / 4);
            }
        }

        public override void OnGUI(Server server)
        {
            GUILayoutUtilities.Button(nameof(Shutdown), () => Shutdown(server));
            GUILayout.Label($"Tick: {server.Tick}");
        }

        public override void OnDestroy(Server server)
        {
            Debug.Log("OnDestroyServer");
            Shutdown(server);
        }

        private void Shutdown(Server server)
        {
            _gameLoopThread.Abort();
            
            foreach (var connection in server._connections)
            {
                server._client.SendUnreliableMessage(new ServerClosingMessage(), connection);
            }
            
            server._client.Dispose();
            server._client = null;
            server.Switch(server.Stopped);
        }
    }
}