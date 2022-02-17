using System;
using System.Net;
using UnityEngine;
using System.Net.Sockets;
using System.Threading.Tasks;
using Artemis.Extensions;
using Artemis.Threading;
using Artemis.Utilities;
using Artemis.ValueObjects;

namespace Artemis.Clients
{
    public class ByteClient : IDisposable
    {
        private readonly UdpClient _client;

        public int Port => ((IPEndPoint) _client.Client.LocalEndPoint).Port;

        protected ByteClient(int port = 0)
        {
            _client = new UdpClient(port);
            _client.Client.DontReportUnreachableEndPoint();
        }

        public virtual void Start()
        {
            _client.BeginReceive(Receive, _client);
        }

        public virtual void Dispose()
        {
            _client.Dispose();
        }

        protected async void SendBytes(byte[] bytes, Address recipient)
        {
            // Loss simulation
            const double lossProbability = 0.1;
            if (Probability.Chance(lossProbability))
            {
                return;
            }
            
            // Latency simulation
            await Task.Delay(100);
            
            // Normal flow
            try
            {
                _client.Send(bytes, bytes.Length, recipient.Ip, recipient.Port);
            }
            catch (ObjectDisposedException)
            {
            }
        }

        protected virtual void HandleBytes(byte[] bytes, Address sender)
        {
            // Debug.Log($"{nameof(ByteClient)} received {bytes.Length} bytes from {sender}");
        }

        private void Receive(IAsyncResult ar)
        {
            try
            {
                var sender = new IPEndPoint(IPAddress.Any, default);
                var bytes = _client.EndReceive(ar, ref sender);
                //UnityMainThreadDispatcher.Dispatch(() => HandleBytes(bytes, Address.FromIPEndPoint(sender)));
                HandleBytes(bytes, Address.FromIPEndPoint(sender));
                _client.BeginReceive(Receive, _client);
            }
            catch (Exception e) when (e.GetType() != typeof(ObjectDisposedException))
            {
                Debug.LogError(e);
                _client.BeginReceive(Receive, _client);
            }
        }
    }
}