using System;
using System.Threading.Tasks;

namespace Artemis.Utilities
{
    internal static class TaskUtilities
    {
        internal static async Task WaitUntil(Func<bool> condition)
        {
            while (!condition.Invoke())
            {
                await Task.Yield();
            }
        }
    }
}