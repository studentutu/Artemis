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
            var viewPrefab = Resources.LoadAll<PlayerView>(string.Empty).Single();
            var view = Object.Instantiate(viewPrefab);
            view.gameObject.name = isLocalPlayer ? $"LocalPlayer ({playerId:N})" : $"RemotePlayer ({playerId:N})";
            view.PlayerId = playerId;
            view.Nickname.text = nickname;
            view.Sprite.color = color;
            view.transform.position = position;
        }
    }
}