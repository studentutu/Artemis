using System;
using UnityEngine;
using Artemis.Threading;
using Artemis.Sample.Core;

namespace Artemis.Sample.Player
{
    public class UnpredictedLocalPlayer : MonoBehaviour
    {
        private readonly Generics.Memory<Timed<PlayerData>> _snapshotBuffer = new();

        public void OnSnapshotReceived(int tick, PlayerData snapshot)
        {
            _snapshotBuffer.Add(new Timed<PlayerData>(snapshot, tick), DateTime.Now.AddSeconds(1));

            UnityMainThreadDispatcher.Dispatch(() =>
            {
                transform.position = new Vector2(snapshot.Position.X, snapshot.Position.Y);
            });
        }

        private void OnDrawGizmos()
        {
            _snapshotBuffer.RemoveExpiredItems();
            
            foreach (var snapshot in _snapshotBuffer)
            {
                DrawSnapshot(snapshot);
            }
        }

        private void DrawSnapshot(Timed<PlayerData> snapshot)
        {
            UnityEditor.Handles.color = GUI.color = Color.green;
            UnityEditor.Handles.DrawWireDisc(snapshot.Value.Position, Vector3.back, 0.1f);
            var style = new GUIStyle("label") {fontStyle = FontStyle.Bold};
            UnityEditor.Handles.Label(snapshot.Value.Position + Vector2.left * 0.1f, $"F{snapshot.Tick}", style);
        }
    }
}