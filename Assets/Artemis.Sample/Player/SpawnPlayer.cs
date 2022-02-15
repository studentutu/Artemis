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
                ? (global::BasePlayer) Resources.LoadAll<LocalPlayer>(string.Empty).Single()
                : (global::BasePlayer) Resources.LoadAll<RemotePlayer>(string.Empty).Single();
            
            var player = Object.Instantiate(prefab).GetComponent<global::BasePlayer>();
            player.Initialize(playerId, nickname, color, position);
            player.gameObject.name = $"{prefab.name} ({playerId:N})";
        }
    }
}