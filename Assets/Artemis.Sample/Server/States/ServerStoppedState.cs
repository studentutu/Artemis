using System;
using Artemis.Clients;
using Artemis.Sample.Server.Core;
using Artemis.ValueObjects;

namespace Artemis.Sample.Server.States
{
    public class ServerStoppedState : AServerState
    {
        public override void OnGUI(DapperServer dapperServer)
        {
            GUILayoutUtilities.Button("Start", () => Start(dapperServer));
        }

        private static void Start(DapperServer dapperServer)
        {
            dapperServer._client = new ArtemisClient(Array.Empty<Handler>(), Configuration.ServerPort);
            dapperServer._client.Start();
            dapperServer.Switch(dapperServer.Running);
        }
    }
}