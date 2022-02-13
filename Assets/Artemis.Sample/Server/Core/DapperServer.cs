using System;
using System.Collections.Generic;
using System.Threading;
using Artemis.Clients;
using Artemis.Sample.Core;
using Artemis.Sample.Server.States;
using UnityEngine;

namespace Artemis.Sample.Server.Core
{
    public class DapperServer : MonoBehaviour
    {
        public ArtemisClient _client;
        public readonly List<PlayerData> _players = new();
        public CancellationToken CancellationTokenOnDestroy { get; private set; }

        public int Tick;
        public DateTime TimeAtFirstTick;
        public AServerState Current;
        public readonly AServerState Stopped = new ServerStoppedState();
        public readonly AServerState Running = new ServerRunningState();

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