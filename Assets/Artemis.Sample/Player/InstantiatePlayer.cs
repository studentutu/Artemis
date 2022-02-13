using System.Linq;
using UnityEngine;

namespace Artemis.Sample.Player
{
    public static class InstantiatePlayer
    {
        public static void Instantiate(string nickname, Color color, Vector2 position, bool isLocalPlayer)
        {
            var viewPrefab = Resources.LoadAll<PlayerView>(string.Empty).Single();
            var view = Object.Instantiate(viewPrefab);
            view.Nickname.text = nickname;
            view.Sprite.color = color;
            view.transform.position = position;
        }
    }
}