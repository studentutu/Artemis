using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Artemis.Utilities;
using Artemis.ValueObjects;
using UnityEngine;

namespace Artemis.Clients
{
    public class ArtemisClient : ReliableClient
    {
        private readonly Dictionary<string, Response> _responses = new();
        private readonly Dictionary<Type, Action<object, Address>> _messageHandlers = new();
        private readonly Dictionary<Type, Action<Request, object, Address>> _requestHandlers = new();

        public ArtemisClient(int port = 0) : base(port)
        {
        }

        public void RegisterMessageHandler<T>(Action<Message<T>> handler)
        {
            _messageHandlers.Add(typeof(T), (payload, address) =>
            {
                var msg = new Message<T>
                {
                    Sender = address,
                    Payload = (T) payload
                };

                handler.Invoke(msg);
            });
        }

        public void RegisterRequestHandler<T>(Action<Request, T, Address> handler)
        {
            _requestHandlers.Add(typeof(T), (req, obj, addr) => handler.Invoke(req, (T) obj, addr));
        }

        protected override void HandlePayload(object payload, Address sender)
        {
            switch (payload)
            {
                case Request request:
                    HandleRequest(request, sender);
                    break;
                case Response response:
                    HandleResponse(response, sender);
                    break;
                default:
                    HandleUserMessage(payload, sender);
                    break;
            }
        }
        
        private void HandleUserMessage(object payload, Address sender)
        {
            if (_messageHandlers.TryGetValue(payload.GetType(), out var handler))
            {
                handler.Invoke(payload, sender);
            }
            else
            {
                Debug.LogError($"Message handler not found for type '{payload.GetType().FullName}'.");
            }
        }

        protected virtual void HandleRequest(Request request, Address sender)
        {
            if (_requestHandlers.TryGetValue(request.Payload.GetType(), out var handler))
            {
                handler.Invoke(request, request.Payload, sender);
            }
            else
            {
                Debug.LogError($"Request handler not found for type '{request.Payload.GetType().FullName}'.");
            }
        }

        protected virtual void HandleResponse(Response response, Address sender)
        {
            Debug.Log($"Received a response of type {response.Payload.GetType().FullName} from {sender}");
            _responses.Add(response.Id, response);
        }

        public async Task<object> Request<T>(T obj, Address recepient)
        {
            var request = new Request(obj);
            SendMessage(request, recepient, DeliveryMethod.Reliable);

            var timeoutTask = Task.Delay(3000);
            var responseTask = TaskUtilities.WaitUntil(() => _responses.ContainsKey(request.Id));
            var completed = await Task.WhenAny(timeoutTask, responseTask);

            if (completed == responseTask)
            {
                var response = _responses[request.Id];
                _responses.Remove(request.Id);
                return response.Payload;
            }

            throw new TimeoutException();
        }
    }
}