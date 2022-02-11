using System;
using Artemis.Clients;
using Artemis.ValueObjects;

namespace Artemis.Sample.Core
{
    public class ServerStoppedState : AServerState
    {
        public override void OnGUI(Server server)
        {
            GUILayoutUtilities.Button("Start", () => Start(server));
        }

        private static void Start(Server server)
        {
            server._client = new ArtemisClient(Array.Empty<Handler>(), Configuration.ServerPort);
            server._client.Start();
            server.Switch(server.Running);
        }
    }
}