using System;
using Artemis.Clients;
using Artemis.ValueObjects;

namespace Artemis.Sample.Core
{
    public class ServerStoppedState : IServerState
    {
        void IServerState.OnStateEntered(Server server)
        {
        }

        void IServerState.OnGUI(Server server)
        {
            GUILayoutUtilities.Button("Start", () => Start(server));
        }

        void IServerState.OnDestroy(Server server)
        {
        }

        private static void Start(Server server)
        {
            server._client = new ArtemisClient(Array.Empty<Handler>(), Constants.ServerPort);
            server._client.Start();
            server.Switch(server.Running);
        }
    }
}