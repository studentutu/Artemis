using System.Linq;
using System.Threading;
using Artemis.Exceptions;
using Artemis.Packets;
using Artemis.ValueObjects;
using UnityEngine;

namespace Artemis.Clients
{
    public class ReliableClient : ObjectClient
    {
        private readonly Thread _retransmissionThread;
        private readonly PendingAckMessageQueue _pendingAckMsgQueue = new();
        private readonly PacketSequenceStorage _outgoingSequenceStorage = new();
        private readonly PacketSequenceStorage _incomingSequenceStorage = new();

        protected ReliableClient(int port = 0) : base(port)
        {
            _retransmissionThread = new Thread(ResendPendingAckPackets);
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

        public void SendMessage<T>(T obj, Address recepient, DeliveryMethod deliveryMethod)
        {
            var message = new Message(
                _outgoingSequenceStorage.Get(recepient, deliveryMethod, 0) + 1,
                obj, deliveryMethod);

            _outgoingSequenceStorage.Set(recepient, deliveryMethod, message.Sequence);

            if (deliveryMethod == DeliveryMethod.Reliable)
            {
                lock (_pendingAckMsgQueue)
                {
                    _pendingAckMsgQueue.Add(recepient, message);
                }
            }

            SendObject(message, recepient);
        }

        private void ResendPendingAckPackets()
        {
            while (true)
            {
                Thread.Sleep(32);

                lock (_pendingAckMsgQueue)
                {
                    foreach (var (address, messages) in _pendingAckMsgQueue.Get())
                    {
                        foreach (var message in messages.Take(64))
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
            lock (_pendingAckMsgQueue)
            {
                _pendingAckMsgQueue.Remove(sender, ack.Sequence);
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