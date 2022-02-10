using System;
using System.Collections.Concurrent;
using Artemis.Utilities;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Artemis.Threading
{
    public class UnityMainThreadDispatcher
    {
        private static readonly ConcurrentQueue<Action> _queue = new();

        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            _queue.Clear();
            PlayerLoopUtilities.AddSubSystem<Update, UnityMainThreadDispatcher>(Invoke);
        }

        private static void Invoke()
        {
            while (_queue.TryDequeue(out var action))
            {
                action.Invoke();
            }
        }

        public static void Dispatch(Action action)
        {
            _queue.Enqueue(action);
        }
    }
}