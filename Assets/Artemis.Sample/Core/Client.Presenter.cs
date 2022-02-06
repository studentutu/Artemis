using Artemis.Sample;
using UnityEngine;

public partial class Client
{
    private void OnGUI()
    {
        using (new GUILayout.AreaScope(new Rect(8, 8, 200, Screen.height)))
        {
            using (new GUILayout.VerticalScope("box"))
            {
                GUILayout.Label("Client", new GUIStyle("Label") {alignment = TextAnchor.MiddleCenter});
                PresentConnectSection();
                PresentConnectingSection();
                PresentDisconnectSection();
            }
        }
    }
    
    private void PresentConnectSection()
    {
        if (_state == State.Null)
        {
            using (new GUILayout.HorizontalScope())
            {
                GUILayoutUtilities.Button(nameof(Connect), Connect, GUILayout.Width(64));
                _serverHostname = GUILayout.TextField(_serverHostname);
            }
        }
    }

    private void PresentConnectingSection()
    {
        if (_state == State.Connecting)
        {
            var labels = new[] {"Connecting", "Connecting.", "Connecting..", "Connecting..."};
            GUILayout.Label(labels[Mathf.CeilToInt(Time.frameCount * 0.005f) % labels.Length]);
        }
    }

    private void PresentDisconnectSection()
    {
        if (_state == State.Connected)
        {
            GUILayoutUtilities.Button(nameof(Disconnect), Disconnect);
        }
    }
}
