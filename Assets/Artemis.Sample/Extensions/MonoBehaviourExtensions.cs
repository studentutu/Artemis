using UnityEngine;
using System.Threading;

internal static class MonoBehaviourExtensions
{
    private class OnDestroyAsyncHook : MonoBehaviour
    {
        private readonly CancellationTokenSource _cts = new();
        public CancellationToken OnDestroyCancellationToken => _cts.Token;

        private void OnDestroy()
        {
            _cts.Cancel();
        }
    }

    internal static CancellationToken GetOnDestroyCancellationToken(this GameObject monoBehaviour)
    {
        var hook = monoBehaviour.gameObject.AddComponent<OnDestroyAsyncHook>();
        return hook.OnDestroyCancellationToken;
    }
}