using UnityEngine;

namespace Artemis.Sample.Core
{
    internal class AClientConnectingState : AClientState
    {
        private readonly string[] _labels =
        {
            "Connecting", "Connecting.", "Connecting..", "Connecting..."
        };

        public override void OnGUI(Client client)
        {
            var index = Mathf.CeilToInt(Time.frameCount * 0.005f) % _labels.Length;
            GUILayout.Label(_labels[index]);
        }
    }
}