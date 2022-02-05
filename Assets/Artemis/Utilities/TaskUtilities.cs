using System;
using System.Threading.Tasks;

namespace Artemis.Utilities
{
    public static class TaskUtilities
    {
        public static async Task WaitUntil(Func<bool> condition)
        {
            while (!condition.Invoke())
            {
                await Task.Yield();
            }
        }
    }
}