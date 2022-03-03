using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Artemis.Packets;
using Artemis.Settings;
using Artemis.UserInterface;
using Artemis.ValueObjects;
using UnityEngine;

namespace Artemis.Clients
{
    public class ArtemisClient : ReliableClient
    {
        private readonly Dictionary<Guid, TaskCompletionSource<object>> _responses = new();
        private readonly Dictionary<Type, Action<Message, IPEndPoint>> _messageHandlers = new();
        private readonly Dictionary<Type, Action<Request, IPEndPoint>> _requestHandlers = new();

        public ArtemisClient(int port = 0) : base(port)
        {
        }

        public void RegisterHandler<T>(IRequestHandler<T> handler)
        {
            RegisterRequestHandler<T>(handler.Handle);
        }
        
        public void RegisterHandler<T>(IMessageHandler<T> handler)
        {
            RegisterMessageHandler<T>(handler.Handle);
        }

        public void RegisterMessageHandler<T>(Action<Message<T>> handler)
        {
            _messageHandlers.Add(typeof(T), (message, sender) =>
            {
                handler.Invoke(new Message<T>((T) message.Payload, sender));
            });
        }
        
        public Task<object> RequestAsync<T>(T obj, IPEndPoint recepient, CancellationToken ct = default)
        {
            return RequestAsync(obj, recepient, Configuration.RequestTimeout, ct);
        }

        public Task<object> RequestAsync<T>(T obj, IPEndPoint recepient, TimeSpan timeout, CancellationToken ct = default)
        {
            var timeoutCts = new CancellationTokenSource(timeout);
            var globalCts = CancellationTokenSource.CreateLinkedTokenSource(timeoutCts.Token, ct);

            var request = new Request(obj);
            var tcs = new TaskCompletionSource<object>();
            _responses.Add(request.Id, tcs);
            SendReliableMessage(request, recepient, globalCts.Token);
            globalCts.Token.Register(() => CancelRequest(tcs, request));

            object DisposeAndReturn(Task<object> task)
            {
                timeoutCts.Dispose();
                globalCts.Dispose();
                return task.Result;
            }

            return tcs.Task.ContinueWith(DisposeAndReturn, globalCts.Token);
        }

        public void RegisterRequestHandler<T>(Action<Request<T>> handler)
        {
            _requestHandlers.Add(typeof(T), (request, sender) =>
            {
                handler.Invoke(new Request<T>(request.Id, (T) request.Payload, sender, this));
            });
        }

        protected override void HandleMessage(Message message, IPEndPoint sender)
        {
            switch (message.Payload)
            {
                case Request request:
                    //Debug.Log($"handling response of type {request.Payload.GetType().FullName} from {sender}");
                    HandleRequest(request, sender);
                    break;
                case Response response:
                    //Debug.Log($"handling response of type {response.Payload.GetType().FullName} from {sender}");
                    HandleResponse(response, sender);
                    break;
                default:
                    //Debug.Log($"handling message of type {message.Payload.GetType().FullName} from {sender}");
                    HandleUserMessage(message, sender);
                    break;
            }
        }
        
        private void HandleUserMessage(Message message, IPEndPoint sender)
        {
            if (_messageHandlers.TryGetValue(message.Payload.GetType(), out var handler))
            {
                handler.Invoke(message, sender);
            }
            else
            {
                Debug.LogError($"Client bound at {Port} has no message handler for type '{message.Payload.GetType().FullName}'.");
            }
        }

        protected virtual void HandleRequest(Request request, IPEndPoint sender)
        {
            if (_requestHandlers.TryGetValue(request.Payload.GetType(), out var handler))
            {
                handler.Invoke(request, sender);
            }
            else
            {
                Debug.LogError($"Request handler not found for type '{request.Payload.GetType().FullName}'.");
            }
        }

        protected virtual void HandleResponse(Response response, IPEndPoint sender)
        {
            if (_responses.Remove(response.Id, out var tcs))
            {
                tcs.TrySetResult(response.Payload);
            }
            else
            {
                // It can happen because of latency ¯\_(ツ)_/¯
                Debug.LogWarning("Received a response for a request the client has cancelled");
            }
        }

        private void CancelRequest(TaskCompletionSource<object> tcs, Request request)
        {
            tcs.TrySetCanceled();
            _responses.Remove(request.Id);
        }
    }
}