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

        public ArtemisClient(int port = 0) : base(port)
        {
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
                    base.HandlePayload(payload, sender);
                    break;
            }
        }

        protected virtual void HandleRequest(Request request, Address sender)
        {
            Debug.Log($"Received a request of type {request.Payload.GetType().FullName} from {sender}");
            var response = new Response(request, DateTime.UtcNow);
            SendMessage(response, sender, DeliveryMethod.Reliable);
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