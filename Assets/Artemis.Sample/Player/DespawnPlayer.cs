using System;
using System.Linq;

namespace Artemis.Sample.Player
{
    public static class DespawnPlayer
    {
        public static void Despawn(Guid playerId)
        {
            var views = UnityEngine.Object.FindObjectsOfType<PlayerView>();
            var view = views.Single(v => v.PlayerId == playerId);
            UnityEngine.Object.Destroy(view.gameObject);
        }
    }
}