using UnityEngine;
using Artemis.Clients;
using Artemis.Sample.Core;
using Artemis.ValueObjects;

public class Client : MonoBehaviour
{
    public ArtemisClient _client;
    public Address ServerAddress;

    public string State;
    public AClientState Current;
    public readonly AClientState Disconnected = new ClientDisconnectedState();
    public readonly AClientState Connecting = new ClientConnectingState();
    public readonly AClientState Connected = new ClientConnectedState();

    private void Start()
    {
        Current = Disconnected;
    }

    private void OnGUI()
    {
        State = Current.GetType().Name;
        using (new GUILayout.AreaScope(new Rect(8, 8, 200, Screen.height)))
        {
            using (new GUILayout.VerticalScope("box"))
            {
                GUILayout.Label("Client", new GUIStyle("Label") {alignment = TextAnchor.MiddleCenter});
                Current.OnGUI(this);
            }
        }
    }

    private void OnDestroy()
    {
        Current.OnDestroy(this);
    }
    
    public void Switch(AClientState state)
    {
        (Current = state).OnStateEntered(this);
    }
}