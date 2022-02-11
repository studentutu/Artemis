using System;
using System.Threading;
using Artemis.Sample.Features.ClockSynchonization;
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
            server._client.RegisterRequestHandler<ConnectionRequest>(r => HandleConnectionRequest(r, server));
            server._client.RegisterMessageHandler<ClientDisconnectionMessage>(m => HandleDisconnectionMessage(m, server));
            server._client.RegisterRequestHandler<Ping>(HandlePingRequest);
            server._client.RegisterRequestHandler<GetTimeRequest>((r) => HandleTimeRequest(r, server));
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

        private static void HandleConnectionRequest(Request<ConnectionRequest> request, Server server)
        {
            server._connections.Add(request.Sender);
            request.Reply(new ConnectionResponse());
            Debug.Log($"<b>[S]</b> Client {request.Sender} Connected!");
        }

        private static void HandleDisconnectionMessage(Message<ClientDisconnectionMessage> message, Server server)
        {
            server._connections.Remove(message.Sender);
            Debug.Log($"<b>[S]</b> Client {message.Sender} has disconnected gracefully :)");
        }
        
        private void HandlePingRequest(Request<Ping> request)
        {
            request.Reply(new Pong());
        }
        
        private void HandleTimeRequest(Request<GetTimeRequest> request, Server server)
        {
            request.Reply(new GetTimeResponse(server.Tick, DateTime.UtcNow, server.TimeAtFirstTick));
        }
    }
}