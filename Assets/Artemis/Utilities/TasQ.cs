using System;
using System.Threading;
using System.Threading.Tasks;

namespace Artemis.Utilities
{
    public static class TasQ
    {
        public static Task<T> TimeoutAfter<T>(this Task<T> task, TimeSpan timeout)
        {
            var proxy = new TaskCompletionSource<T>();

            void TimeoutTask(object _)
            {
                proxy.SetException(new TimeoutException());
            }

            var timer = new Timer(TimeoutTask, null, timeout, Timeout.InfiniteTimeSpan);

            task.ContinueWith(t =>
            {
                timer.Dispose();
                ForwardResult(t, proxy);
            });

            return proxy.Task;
        }
        
        public static void ForwardResult<T>(this Task<T> task, TaskCompletionSource<T> proxy)
        {
            switch (task.Status)
            {
                case TaskStatus.RanToCompletion: proxy.TrySetResult(task.Result); break;
                case TaskStatus.Canceled: proxy.TrySetCanceled(); break;
                case TaskStatus.Faulted: proxy.TrySetException(task.Exception); break;
                default: throw new Exception(task.Status.ToString());
            }
        }
        
        public static void Forget(this Task task)
        {
            // Suppresses compiler warning
        }
    }
}