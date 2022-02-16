using UnityEngine;
using Artemis.Threading;
using Artemis.Sample.Core;

namespace Artemis.Sample.Player
{
    public class UnpredictedLocalPlayer : MonoBehaviour
    {
        private int _lastReceivedFrame = -1;
        
        public void OnSnapshotReceived(int tick, PlayerData snapshot)
        {
            _lastReceivedFrame = tick;

            UnityMainThreadDispatcher.Dispatch(() =>
            {
                transform.position = new Vector2(snapshot.Position.X, snapshot.Position.Y);
            });
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, Vector3.one * 0.2f);
            UnityEditor.Handles.Label(transform.position, $"F{_lastReceivedFrame}");
        }
    }
}