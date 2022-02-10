using UnityEngine;

namespace Artemis.Sample.Core
{
    internal class ClientConnectingState : AClientState
    {
        private readonly string[] _labels =
        {
            "Connecting", "Connecting.", "Connecting..", "Connecting..."
        };

        public override void OnStateEntered(Client client)
        {
            Debug.Log($"[C] OnStateEntered {GetType().Name}");
        }

        public override void OnGUI(Client client)
        {
            var index = Mathf.CeilToInt(Time.frameCount * 0.005f) % _labels.Length;
            GUILayout.Label(_labels[index]);
        }
    }
}