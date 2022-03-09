using System;
using System.Linq;
using System.Net;
using System.Threading;
using Artemis.Exceptions;
using Artemis.Packets;
using Artemis.Settings;
using Artemis.ValueObjects;
using UnityEngine;
using UnityEngine.Assertions;

namespace Artemis.Clients
{
    public class ReliableClient : ObjectClient
    {
        private readonly Thread _retransmissionThread;
        private readonly RetransmissionQueue _retransmissionQueue = new();
        private readonly PacketSequenceStorage _outgoingSequenceStorage = new();
        private readonly PacketSequenceStorage _incomingSequenceStorage = new();

        protected ReliableClient(int port = 0) : base(port)
        {
            _retransmissionThread = new Thread(RetransmitReliableMessages);
        }

        public override void Start()
        {
            base.Start();
            _retransmissionThread.Start();
        }

        public override void Dispose()
        {
            _retransmissionThread.Abort();
            base.Dispose();
        }

        private Message EncapsulatePayloadInsideAMessage<T>(T payload, IPEndPoint recipient, DeliveryMethod deliveryMethod)
        {
            var sequence = _outgoingSequenceStorage.Get(recipient, deliveryMethod, 0) + 1;
            return new Message(sequence, payload, deliveryMethod);
        }

        private void SendMessage(Message message, IPEndPoint recipient)
        {
            _outgoingSequenceStorage.Set(recipient, message.DeliveryMethod, message.Sequence);
            SendObject(message, recipient);
        }

        public void SendUnreliableMessage<T>(T payload, IPEndPoint recipient)
        {
            var message = EncapsulatePayloadInsideAMessage(payload, recipient, DeliveryMethod.Unreliable);
            SendMessage(message, recipient);
        }

        public void SendReliableMessage<T>(T payload, IPEndPoint recipient, CancellationToken ct = default)
        {
            var message = EncapsulatePayloadInsideAMessage(payload, recipient, DeliveryMethod.Reliable);
            SendMessage(message, recipient);

            var registration = ct.Register(() =>
            {
                lock (_retransmissionQueue)
                {
                    Debug.Log("Removing reliable message from retransmission queue");
                    _retransmissionQueue.Remove(recipient, message.Sequence);
                }
            });
            
            lock (_retransmissionQueue)
            {
                _retransmissionQueue.Add(recipient, message, registration);
            }
        }

        private void RetransmitReliableMessages()
        {
            while (true)
            {
                Thread.Sleep(Configuration.RetransmissionInterval);

                lock (_retransmissionQueue)
                {
                    foreach (var (address, messages) in _retransmissionQueue.Get())
                    {
                        foreach (var message in messages.Take(Configuration.RetransmissionCapacity))
                        {
                            SendObject(message, address);
                        }
                    }
                }
            }
        }

        internal virtual void HandleMessage(Message message, IPEndPoint sender)
        {
            Debug.Log($"Received message containing {message.Payload.GetType().FullName} from {sender}");
        }

        private void HandlePacket(Message message, IPEndPoint sender)
        {
            switch (message.DeliveryMethod)
            {
                case DeliveryMethod.Reliable: HandleRealiablePacket(message, sender); break;
                case DeliveryMethod.Unreliable: HandleUnrealiablePacket(message, sender); break;
                default: throw new ArgumentOutOfRangeException(message.DeliveryMethod.ToString());
            }
        }
        
        private void HandleUnrealiablePacket(Message message, IPEndPoint sender)
        {
            Assert.IsTrue(message.DeliveryMethod == DeliveryMethod.Unreliable);
            var lastReceivedSequence = _incomingSequenceStorage.Get(sender, DeliveryMethod.Unreliable, 0);
            var isOrdered = message.Sequence > lastReceivedSequence;

            if (!isOrdered)
            {
                Debug.LogWarning("Discarding out of order unreliable message");
                return; // Discard because is duplicate or out of order
            }
            
            _incomingSequenceStorage.Set(sender, DeliveryMethod.Unreliable, message.Sequence);
            HandleMessage(message, sender);
        }

        private void HandleRealiablePacket(Message message, IPEndPoint sender)
        {
            Assert.IsTrue(message.DeliveryMethod == DeliveryMethod.Reliable);
            var expectedSequence = _incomingSequenceStorage.Get(sender, DeliveryMethod.Reliable, 0) + 1;
            var isSequenced = message.Sequence == expectedSequence;

            if (!isSequenced)
            {
                return; // Discard because is duplicate or out of sequence
            }
            
            SendObject(new Ack(message.Sequence), sender);
            _incomingSequenceStorage.Set(sender, DeliveryMethod.Reliable, message.Sequence);
            HandleMessage(message, sender);
        }

        private void HandleAcknowledgement(Ack ack, IPEndPoint sender)
        {
            lock (_retransmissionQueue)
            {
                _retransmissionQueue.Remove(sender, ack.Sequence);
            }
        }

        protected override void HandleObject(object obj, IPEndPoint sender)
        {
            base.HandleObject(obj, sender);

            switch (obj)
            {
                case Message message:
                    HandlePacket(message, sender);
                    break;
                case Ack acknowledgement:
                    HandleAcknowledgement(acknowledgement, sender);
                    break;
                default: throw new ObjectTypeUnhandledException(obj);
            }
        }
    }
}