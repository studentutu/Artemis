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
                ? Resources.LoadAll<LocalPlayer>(string.Empty).Single().gameObject
                : Resources.LoadAll<RemotePlayer>(string.Empty).Single().gameObject;
            
            var view = Object.Instantiate(prefab).GetComponentInChildren<PlayerView>();
            view.gameObject.name = $"{prefab.name} ({playerId:N})";
            view.PlayerId = playerId;
            view.Nickname.text = nickname;
            view.Sprite.color = color;
            view.transform.position = position;
        }
    }
}