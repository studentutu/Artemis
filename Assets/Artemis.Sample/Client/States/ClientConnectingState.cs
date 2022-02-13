using UnityEngine;

namespace Artemis.Sample.Core
{
    internal class ClientConnectingState : AClientState
    {
        private readonly string[] _labels =
        {
            "Connecting", "Connecting.", "Connecting..", "Connecting..."
        };

        public override void OnStateEntered(DapperClient dapperClient)
        {
            Debug.Log($"[C] OnStateEntered {GetType().Name}");
        }

        public override void OnGUI(DapperClient dapperClient)
        {
            var index = Mathf.CeilToInt(Time.frameCount * 0.005f) % _labels.Length;
            GUILayout.Label(_labels[index]);
        }
    }
}