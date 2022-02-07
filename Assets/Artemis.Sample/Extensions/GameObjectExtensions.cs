using UnityEngine;
using System.Threading;
using Artemis.Sample.Extensions;

public static class GameObjectExtensions
{
    internal static CancellationToken GetOnDestroyCancellationToken(this GameObject gameObject)
    {
        return gameObject.GetOrAddComponent<OnDestroyAsyncHook>().OnDestroyCancellationToken;
    }

    internal static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        return gameObject.TryGetComponent<T>(out var component) ? component : gameObject.AddComponent<T>();
    }
}