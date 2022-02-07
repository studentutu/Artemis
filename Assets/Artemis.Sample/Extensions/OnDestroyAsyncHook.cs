using System.Threading;
using UnityEngine;

namespace Artemis.Sample.Extensions
{
    internal class OnDestroyAsyncHook : MonoBehaviour
    {
        private readonly CancellationTokenSource _cts = new();
        public CancellationToken OnDestroyCancellationToken => _cts.Token;

        private void OnDestroy()
        {
            _cts.Cancel();
        }
    }
}