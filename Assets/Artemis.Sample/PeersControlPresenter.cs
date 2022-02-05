using UnityEngine;
using System.Collections.Generic;

namespace Artemis.Sample
{
    public class PeersControlPresenter : MonoBehaviour
    {
        [SerializeField] private List<Peer> _peers = new();

        public void OnGUI()
        {
            using (new GUILayout.AreaScope(new Rect(8, 8, Screen.width - 16, Screen.height - 16)))
            {
                using (new GUILayout.HorizontalScope())
                {
                    _peers.ForEach(peer => peer.Present());
                }
            }
        }
    }
}