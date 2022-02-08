using UnityEngine;
using Artemis.Clients;
using Artemis.ValueObjects;
using System.Collections.Generic;
using Artemis.Sample.Core;

public class Server : MonoBehaviour
{
    public ArtemisClient _client;
    public readonly List<Address> _connections = new List<Address>();

    public IServerState Current;
    public readonly IServerState Stopped = new ServerStoppedState();
    public readonly IServerState Running = new ServerRunningState();

    public void Switch(IServerState state)
    {
        (Current = state).OnStateEntered(this);
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