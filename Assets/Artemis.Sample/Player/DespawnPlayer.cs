using System;
using System.Linq;

namespace Artemis.Sample.Player
{
    public static class DespawnPlayer
    {
        public static void Despawn(Guid playerId)
        {
            var views = UnityEngine.Object.FindObjectsOfType<global::Player>();
            var view = views.Single(v => v.Id == playerId);
            UnityEngine.Object.Destroy(view.gameObject);
        }
        
        public static void DespawnAll()
        {
            foreach (var view in UnityEngine.Object.FindObjectsOfType<PlayerView>())
            {
                UnityEngine.Object.Destroy(view.gameObject);
            }
        }
    }
}