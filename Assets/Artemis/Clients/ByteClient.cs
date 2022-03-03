using System;
using System.Net;
using UnityEngine;
using System.Net.Sockets;
using Artemis.Extensions;

namespace Artemis.Clients
{
    public class ByteClient : IDisposable
    {
        private readonly UdpClient _client;
        private IPEndPoint _sender = new IPEndPoint(IPAddress.Any, default);

        public int Port => ((IPEndPoint) _client.Client.LocalEndPoint).Port;
        public IPEndPoint DefaultRemote => (IPEndPoint) _client.Client.RemoteEndPoint;

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

        protected void SendBytes(byte[] bytes, IPEndPoint recipient)
        {
            _client.Send(bytes, bytes.Length, recipient);
        }

        protected virtual void HandleBytes(byte[] bytes, IPEndPoint sender)
        {
            // Debug.Log($"{nameof(ByteClient)} received {bytes.Length} bytes from {sender}");
        }

        private void Receive(IAsyncResult ar)
        {
            try
            {
                var bytes = _client.EndReceive(ar, ref _sender);
                HandleBytes(bytes, _sender.Copy());
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