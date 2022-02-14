using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Artemis.Clients;
using Artemis.Sample.Core;
using Artemis.Sample.Packets;
using Artemis.Sample.Server.States;
using Artemis.ValueObjects;
using UnityEngine;

namespace Artemis.Sample.Server.Core
{
    public class DapperServer : MonoBehaviour
    {
        public ArtemisClient _client;
        public readonly List<(Address, PlayerData)> _players = new();
        public CancellationToken CancellationTokenOnDestroy { get; private set; }

        public DateTime TimeAtFirstTick;
        public AServerState Current;
        public readonly AServerState Stopped = new ServerStoppedState();
        public readonly AServerState Running = new ServerRunningState();
        public readonly InputBuffer InputBuffer = new InputBuffer();

        private int _tick;

        public int Tick
        {
            get
            {
                return _tick;
            }
            set
            {
                if (value != _tick)
                {
                    Simulate(_tick = value);
                }
            }
        }

        private void Simulate(int tick)
        {
            // Movement players
            foreach (var tuple in _players)
            {
                var command = InputBuffer.Get(tick, tuple.Item1);
                Debug.Log(command.Horizontal);
                var axis = Vector2.ClampMagnitude(new Vector2(command.Horizontal, command.Vertical), 1f);
                tuple.Item2.Position += axis * 0.1f;
            }

            // Broadcast frame snapshot
            var snapshot = new Snapshot(tick, _players.Select(t => t.Item2).ToArray());
            foreach (var player in _players)
            {
                _client.SendUnreliableMessage(snapshot, player.Item1);
            }
        }

        private void OnDrawGizmos()
        {
            foreach (var p in _players.Select(tuple => new Vector3(tuple.Item2.Position.X, tuple.Item2.Position.Y)))
            {
                Gizmos.DrawCube(p, Vector3.one);
            }
        }

        public void Switch(AServerState state)
        {
            (Current = state).OnStateEntered(this);
        }

        private void Awake()
        {
            CancellationTokenOnDestroy = gameObject.GetOnDestroyCancellationToken();
        }

        private void Start()
        {
            Current = Stopped;
        }

        private void OnGUI()
        {
            using (new GUILayout.AreaScope(new Rect(Screen.width - 200 - 8, 8, 200, Screen.height)))
            {
                using (new GUILayout.VerticalScope("box"))
                {
                    GUILayout.Label("Server", new GUIStyle("Label") {alignment = TextAnchor.MiddleCenter});
                    Current.OnGUI(this);
                }
            }
        }

        private void OnDestroy()
        {
            Current.OnDestroy(this);
        }
    }
}