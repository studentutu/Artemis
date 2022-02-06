using UnityEngine;
using Artemis.Sample;

public partial class Server
{
    private void OnGUI()
    {
        using (new GUILayout.AreaScope(new Rect(Screen.width - 200 - 8, 8, 200, Screen.height)))
        {
            using (new GUILayout.VerticalScope("box"))
            {
                GUILayout.Label("Server", new GUIStyle("Label") {alignment = TextAnchor.MiddleCenter});
                PresentOpenSection();
                PresentCloseSection();
            }
        }
    }

    private void PresentOpenSection()
    {
        if (_state == State.Closed)
        {
            GUILayoutUtilities.Button(nameof(Open), Open);
        }
    }

    private void PresentCloseSection()
    {
        if (_state == State.Open)
        {
            GUILayoutUtilities.Button(nameof(Close), Close);
        }
    }
}