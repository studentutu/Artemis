using System.Linq;
using System.Threading;
using Artemis.Exceptions;
using Artemis.Packets;
using Artemis.Settings;
using Artemis.ValueObjects;
using UnityEngine;

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

        private Message EncapsulatePayloadInsideAMessage<T>(T payload, Address recipient, DeliveryMethod deliveryMethod)
        {
            var sequence = _outgoingSequenceStorage.Get(recipient, deliveryMethod, 0) + 1;
            return new Message(sequence, payload, deliveryMethod);
        }

        private void SendMessage(Message message, Address recipient)
        {
            _outgoingSequenceStorage.Set(recipient, message.DeliveryMethod, message.Sequence);
            SendObject(message, recipient);
        }

        public void SendUnreliableMessage<T>(T payload, Address recipient)
        {
            var message = EncapsulatePayloadInsideAMessage(payload, recipient, DeliveryMethod.Unreliable);
            SendMessage(message, recipient);
        }

        public void SendReliableMessage<T>(T payload, Address recipient, CancellationToken ct = default)
        {
            var message = EncapsulatePayloadInsideAMessage(payload, recipient, DeliveryMethod.Reliable);
            SendMessage(message, recipient);

            lock (_retransmissionQueue)
            {
                _retransmissionQueue.Add(recipient, message);
            }

            ct.Register(() =>
            {
                lock (_retransmissionQueue)
                {
                    Debug.Log("Removing reliable message from retransmission queue");
                    _retransmissionQueue.Remove(recipient, message.Sequence);
                }
            });
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

        protected virtual void HandleMessage(Message message, Address sender)
        {
            Debug.Log($"Received message containing {message.Payload.GetType().FullName} from {sender}");
        }

        private void HandlePacket(Message message, Address sender)
        {
            var expectedSequence = _incomingSequenceStorage.Get(sender, message.DeliveryMethod, 0) + 1;

            if (message.Sequence != expectedSequence)
            {
                //Debug.LogWarning($"Discarding reliable packet #{message.Sequence} with {message.Payload.GetType().Name} as expected sequence is #{expectedSequence}");
                return; // Discard duplicate or out or order
            }

            if (message.DeliveryMethod == DeliveryMethod.Reliable)
            {
                SendObject(new Ack {Sequence = message.Sequence}, sender);
            }

            _incomingSequenceStorage.Set(sender, message.DeliveryMethod, message.Sequence);
            HandleMessage(message, sender);
        }

        private void HandleAcknowledgement(Ack ack, Address sender)
        {
            lock (_retransmissionQueue)
            {
                _retransmissionQueue.Remove(sender, ack.Sequence);
            }
        }

        protected override void HandleObject(object obj, Address sender)
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