using System.Collections.Generic;
using System.Threading;
using Artemis.Exceptions;
using Artemis.Extensions;
using Artemis.ValueObjects;
using UnityEngine;

namespace Artemis.Clients
{
    public class ReliableClient : ObjectClient
    {
        private readonly Thread _resendReliablePacketsThread;
        private readonly List<PendingAckPacket> _pendingAckPackets = new();
        private readonly PacketSequenceStorage _outgoingSequenceStorage = new();
        private readonly PacketSequenceStorage _incomingSequenceStorage = new();

        protected ReliableClient(int port = 0) : base(port)
        {
            _resendReliablePacketsThread = new Thread(ResendPendingAckPackets);
        }

        public override void Start()
        {
            base.Start();
            _resendReliablePacketsThread.Start();
        }

        public override void Dispose()
        {
            _resendReliablePacketsThread.Abort();
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
                lock (_pendingAckPackets)
                {
                    _pendingAckPackets.Add(new PendingAckPacket(message, recepient));
                }
            }

            SendObject(message, recepient);
        }

        private void ResendPendingAckPackets()
        {
            while (true)
            {
                Thread.Sleep(64);

                lock (_pendingAckPackets)
                {
                    foreach (var pam in _pendingAckPackets)
                    {
                        SendObject(pam.Packet, pam.Recepient);
                    }
                }
            }
        }

        protected virtual void HandlePayload(object payload, Address sender)
        {
            Debug.Log($"Handling payload of type {payload.GetType().FullName} from {sender}");
        }

        private void HandlePacket(Packet packet, Address sender)
        {
            var expectedSequence = _incomingSequenceStorage.Get(sender, packet.DeliveryMethod, 0) + 1;

            if (packet.Sequence != expectedSequence)
            {
                Debug.LogWarning($"Discarding reliable packet #{packet.Sequence} with {packet.Payload.GetType().Name} as expected sequence is #{expectedSequence}");
                return; // Discard duplicate or out or order
            }

            if (packet.DeliveryMethod == DeliveryMethod.Reliable)
            {
                SendObject(new Acknowledgement {Sequence = packet.Sequence}, sender);
            }

            Debug.Log($"Received packet #{packet.Sequence}");
            _incomingSequenceStorage.Set(sender, packet.DeliveryMethod, packet.Sequence);
            HandlePayload(packet.Payload, sender);
        }

        private void HandleAcknowledgement(Acknowledgement ack, Address sender)
        {
            lock (_pendingAckPackets)
            {
                _pendingAckPackets.Remove(pam => pam.Packet.Sequence == ack.Sequence && pam.Recepient == sender);
            }
        }

        protected override void HandleObject(object obj, Address sender)
        {
            base.HandleObject(obj, sender);

            switch (obj)
            {
                case Packet message:
                    HandlePacket(message, sender);
                    break;
                case Acknowledgement acknowledgement:
                    HandleAcknowledgement(acknowledgement, sender);
                    break;
                default: throw new ObjectTypeUnhandledException(obj);
            }
        }
    }
}