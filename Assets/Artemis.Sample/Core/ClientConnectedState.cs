using UnityEngine;
using System.Threading;
using UnityEngine.Assertions;

namespace Artemis.Sample.Core
{
    public class ClientConnectedState : AClientState
    {
        private NetClock _netClock;
        private Thread _gameLoopThread;
        
        public override void OnStateEntered(Client client)
        {
            Debug.Log("OnClientConnectedStateEntered");
            _netClock = Object.FindObjectOfType<NetClock>();
            client._client.RegisterHandler(new ServerClosingMessageHandler(() => Disconnect(client))); // TODO I didn't very liked this, its leaking ClientConnectedState behaviour
            _gameLoopThread = new Thread(() => ServerLoop(client));
            _gameLoopThread.Start();
        }
        
        private void ServerLoop(Client client)
        {
            while (true)
            {
                var elapsed = (_netClock.PredictServerTime() - client.ServerTimeAtFirstTick).TotalSeconds;
                client.Tick = (int) (elapsed * Configuration.TicksPerSecond);
                Thread.Sleep(Configuration.TickInterval / 4);
            }
        }

        public override void OnGUI(Client client)
        {
            GUILayoutUtilities.Button("Disconnect", () => Disconnect(client));
            GUILayout.Label($"Tick: {client.Tick}");
        }

        public override void OnDestroy(Client client)
        {
            Debug.Log("OnDestroyClient");
            Disconnect(client);
        }

        private void Disconnect(Client client)
        {
            _gameLoopThread.Abort();
            Debug.Log($"Disco: {client.Current.GetType().Name}");
            Assert.IsNotNull(client);
            Assert.IsNotNull(client._client);
            client._client.SendUnreliableMessage(new ClientDisconnectionMessage(), client.ServerAddress);
            client._client.Dispose();
            client._client = null;
            client.Switch(client.Disconnected);
        }
    }
}