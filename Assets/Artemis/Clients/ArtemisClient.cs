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
        private readonly Dictionary<string, ResponseContainer> _responses = new();
        private readonly Dictionary<Type, Action<MessageContainer, Address>> _messageHandlers = new();
        private readonly Dictionary<Type, Action<RequestContainer, Address>> _requestHandlers = new();

        public ArtemisClient(IEnumerable<Handler> handlers, int port = 0) : base(port)
        {
            foreach (var handler in handlers)
            {
                handler.Bind(this);
            }
        }

        public void RegisterMessageHandler<T>(Action<Message<T>> handler)
        {
            _messageHandlers.Add(typeof(T), (messageContainer, address) =>
            {
                var msg = new Message<T>((T) messageContainer.Payload, address);
                handler.Invoke(msg);
            });
        }

        public void RegisterRequestHandler<T>(Action<Request<T>> handler)
        {
            _requestHandlers.Add(typeof(T), (requestDto, addr) =>
            {
                var req = new Request<T>(requestDto.Id, (T) requestDto.Payload, addr, this);
                handler.Invoke(req);
            });
        }

        protected override void HandleMessage(MessageContainer messageContainer, Address sender)
        {
            switch (messageContainer.Payload)
            {
                case RequestContainer request:
                    HandleRequest(request, sender);
                    break;
                case ResponseContainer response:
                    HandleResponse(response, sender);
                    break;
                default:
                    HandleUserMessage(messageContainer, sender);
                    break;
            }
        }
        
        private void HandleUserMessage(MessageContainer messageContainer, Address sender)
        {
            if (_messageHandlers.TryGetValue(messageContainer.Payload.GetType(), out var handler))
            {
                handler.Invoke(messageContainer, sender);
            }
            else
            {
                Debug.LogError($"Message handler not found for type '{messageContainer.Payload.GetType().FullName}'.");
            }
        }

        protected virtual void HandleRequest(RequestContainer requestContainer, Address sender)
        {
            if (_requestHandlers.TryGetValue(requestContainer.Payload.GetType(), out var handler))
            {
                handler.Invoke(requestContainer, sender);
            }
            else
            {
                Debug.LogError($"Request handler not found for type '{requestContainer.Payload.GetType().FullName}'.");
            }
        }

        protected virtual void HandleResponse(ResponseContainer responseContainer, Address sender)
        {
            Debug.Log($"Received a response of type {responseContainer.Payload.GetType().FullName} from {sender}");
            _responses.Add(responseContainer.Id, responseContainer);
        }

        public async Task<object> Request<T>(T obj, Address recepient)
        {
            var request = new RequestContainer(obj);
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