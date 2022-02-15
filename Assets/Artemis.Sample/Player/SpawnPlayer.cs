using System;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Artemis.Sample.Player
{
    public static class SpawnPlayer
    {
        public static void Spawn(Guid playerId, string nickname, Color color, Vector2 position, bool isLocalPlayer)
        {
            var prefab = isLocalPlayer
                ? (global::Player) Resources.LoadAll<LocalPlayer>(string.Empty).Single()
                : (global::Player) Resources.LoadAll<RemotePlayer>(string.Empty).Single();
            
            var player = Object.Instantiate(prefab).GetComponent<global::Player>();
            player.Initialize(playerId, nickname, color, position);
            player.gameObject.name = $"{prefab.name} ({playerId:N})";
        }
    }
}