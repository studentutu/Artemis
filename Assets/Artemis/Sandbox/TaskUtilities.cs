using System;
using System.Threading.Tasks;

namespace rUDP.Sandbox
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