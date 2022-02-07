using System;
using System.Threading;
using System.Threading.Tasks;

namespace Artemis.Utilities
{
    internal static class TasQ
    {
        public static async Task<T> TimeoutAfter<T>(this Task<T> task, TimeSpan timeout, CancellationToken ct)
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            var timeoutTask = Task.Delay(timeout, cts.Token);

            if (await Task.WhenAny(task, timeoutTask) == timeoutTask)
            {
                throw new TaskCanceledException();
            }

            cts.Cancel();
            return await task;
        }
    }
}