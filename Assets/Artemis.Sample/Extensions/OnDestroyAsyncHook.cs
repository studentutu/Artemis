using System.Threading;
using UnityEngine;

namespace Artemis.Sample.Extensions
{
    internal class OnDestroyAsyncHook : MonoBehaviour
    {
        public CancellationTokenSource CancellationTokenSource { get; private set; } = new();

        private void OnDestroy()
        {
            CancellationTokenSource.Cancel();
            CancellationTokenSource.Dispose();
            CancellationTokenSource = null;
        }
    }
}