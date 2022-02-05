using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Artemis.Exceptions;
using Artemis.Extensions;
using Artemis.Utilities;
using Artemis.ValueObjects;
using UnityEngine;

namespace Artemis.Clients
{
    public class ReliableClient : ObjectClient
    {
        private readonly Thread _resendReliableMessagesThread;
        private readonly Dictionary<string, Response> _responses = new();
        private readonly List<PendingAckMessage> _pendingAckMessages = new();
        private readonly PacketSequenceStorage _outgoingSequenceStorage = new();
        private readonly PacketSequenceStorage _incomingSequenceStorage = new();

        public ReliableClient(int port = 0) : base(port)
        {
            _resendReliableMessagesThread = new Thread(ResendPendingAckMessages);
        }

        public override void Start()
        {
            base.Start();
            _resendReliableMessagesThread.Start();
        }

        public override void Dispose()
        {
            _resendReliableMessagesThread.Abort();
            base.Dispose();
        }

        public void SendMessage<T>(T obj, Address recepient, DeliveryMethod deliveryMethod)
        {
            var message = new Packet(
                _outgoingSequenceStorage.Get(recepient, deliveryMethod, 0) + 1,
                obj, deliveryMethod);

            _outgoingSequenceStorage.Set(recepient, deliveryMethod, message.Sequence);

            if (deliveryMethod == DeliveryMethod.Reliable)
            {
                lock (_pendingAckMessages)
                {
                    _pendingAckMessages.Add(new PendingAckMessage(message, recepient));
                }
            }

            SendObject(message, recepient);
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
    
        private void ResendPendingAckMessages()
        {
            while (true)
            {
                Thread.Sleep(64);

                lock (_pendingAckMessages)
                {
                    foreach (var pam in _pendingAckMessages)
                    {
                        SendObject(pam.Packet, pam.Recepient);
                    }
                }
            }
        }

        public virtual void HandlePayload(object payload, Address sender)
        {
            Debug.Log($"Handling payload of type {payload.GetType().FullName} from {sender}");
        }

        public virtual void HandleRequest(Request request, Address sender)
        {
            Debug.Log($"Received a request of type {request.Payload.GetType().FullName} from {sender}");
            var response = new Response(request, DateTime.UtcNow) ;
            SendMessage(response, sender, DeliveryMethod.Reliable);
        }
    
        public virtual void HandleResponse(Response response, Address sender)
        {
            Debug.Log($"Received a response of type {response.Payload.GetType().FullName} from {sender}");
            _responses.Add(response.Id, response);
        }

        private void HandleMessage(Packet packet, Address sender)
        {
            var expectedSequence = _incomingSequenceStorage.Get(sender, packet.DeliveryMethod, 0) + 1;

            if (packet.Sequence != expectedSequence)
            {
                Debug.LogWarning(
                    $"Discarding reliable message #{packet.Sequence} as expected sequence is #{expectedSequence}");
                return;
            }

            if (packet.DeliveryMethod == DeliveryMethod.Reliable)
            {
                SendObject(new Acknowledgement {Sequence = packet.Sequence}, sender);
            }

            Debug.Log($"Received message #{packet.Sequence}");

            _incomingSequenceStorage.Set(sender, packet.DeliveryMethod, packet.Sequence);

            // Override HandlePayload in ArtemisClient
            // Check for request or response, if not call base
            switch (packet.Payload) 
            {
                case Request request:
                    HandleRequest(request, sender);
                    break;
                case Response response:
                    HandleResponse(response, sender);
                    break;
                default: // Then its a packet with user payload
                    HandlePayload(packet.Payload, sender);
                    break;
            }
        }

        private void HandleAcknowledgement(Acknowledgement ack, Address sender)
        {
            lock (_pendingAckMessages)
            {
                _pendingAckMessages.Remove(pam => pam.Packet.Sequence == ack.Sequence && pam.Recepient == sender);
            }
        }

        protected override void HandleObject(object obj, Address sender)
        {
            base.HandleObject(obj, sender);

            switch (obj)
            {
                case Packet message:
                    HandleMessage(message, sender);
                    break;
                case Acknowledgement acknowledgement:
                    HandleAcknowledgement(acknowledgement, sender);
                    break;
                default: throw new ObjectTypeUnhandledException(obj);
            }
        }
    }
}