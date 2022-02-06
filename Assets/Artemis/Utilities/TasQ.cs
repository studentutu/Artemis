using System;
using System.Threading;
using System.Threading.Tasks;

namespace Artemis.Utilities
{
    internal static class TasQ
    {
        internal static async Task WaitUntil(Func<bool> condition, CancellationToken ct = default)
        {
            while (!condition.Invoke())
            {
                ct.ThrowIfCancellationRequested();
                if (ct.IsCancellationRequested)
                {
                    throw new TaskCanceledException();
                }

                await Task.Yield();
            }
        }
    }
}