using UnityEngine;

namespace Artemis.Sample.Core
{
    internal class ClientConnectingState : IClientState
    {
        private readonly string[] _labels =
        {
            "Connecting", "Connecting.", "Connecting..", "Connecting..."
        };
        
        void IClientState.OnStateEntered(Client client)
        {
        }

        void IClientState.OnGUI(Client client)
        {
            var index = Mathf.CeilToInt(Time.frameCount * 0.005f) % _labels.Length;
            GUILayout.Label(_labels[index]);
        }

        void IClientState.OnDestroy(Client client)
        {
            
        }
    }
}