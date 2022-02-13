using UnityEngine;
using System.Threading;
using Artemis.Sample.Player;
using Artemis.Threading;
using UnityEngine.Assertions;

namespace Artemis.Sample.Core
{
    public class ClientConnectedState : AClientState
    {
        private NetClock _netClock;
        private Thread _gameLoopThread;
        
        public override void OnStateEntered(DapperClient dapperClient)
        {
            Debug.Log("OnClientConnectedStateEntered");
            _netClock = Object.FindObjectOfType<NetClock>();
            dapperClient._client.RegisterHandler(new ServerClosingMessageHandler(() => Disconnect(dapperClient))); // TODO I didn't very liked this, its leaking ClientConnectedState behaviour
            _gameLoopThread = new Thread(() => ServerLoop(dapperClient));
            _gameLoopThread.Start();
        }
        
        private void ServerLoop(DapperClient dapperClient)
        {
            while (true)
            {
                var elapsed = (_netClock.PredictServerTime() - dapperClient.ServerTimeAtFirstTick).TotalSeconds;
                dapperClient.Tick = (int) (elapsed * Configuration.TicksPerSecond);
                Thread.Sleep(Configuration.TickInterval / 4);
            }
        }

        public override void OnGUI(DapperClient dapperClient)
        {
            GUILayoutUtilities.Button("Disconnect", () => Disconnect(dapperClient));
            GUILayout.Label($"Tick: {dapperClient.Tick}");
        }

        public override void OnDestroy(DapperClient dapperClient)
        {
            Debug.Log("OnDestroyClient");
            Disconnect(dapperClient);
        }

        private void Disconnect(DapperClient dapperClient)
        {
            UnityMainThreadDispatcher.Dispatch(() => DespawnPlayer.Despawn(dapperClient.PlayerId));
            _gameLoopThread.Abort();
            Debug.Log($"Disco: {dapperClient.Current.GetType().Name}");
            Assert.IsNotNull(dapperClient);
            Assert.IsNotNull(dapperClient._client);
            dapperClient._client.SendUnreliableMessage(new ClientDisconnectionMessage(dapperClient.PlayerId), dapperClient.ServerAddress);
            dapperClient._client.Dispose();
            dapperClient._client = null;
            dapperClient.Switch(dapperClient.Disconnected);
        }
    }
}