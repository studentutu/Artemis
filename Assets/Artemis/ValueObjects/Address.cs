using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using UnityEngine.Assertions;

namespace Artemis.ValueObjects
{
    public readonly struct Address
    {
        public readonly string Ip;
        public readonly int Port;

        public Address(string ip, int port)
        {
            Assert.IsTrue(IPAddress.TryParse(ip, out _), $"IPAddress '{ip}' its not a valid!");
            Ip = ip;
            Port = port;
        }

        public static Address FromHostname(string hostname, int port)
        {
            var addresses = Dns.GetHostAddresses(hostname);
            var ipAddress = addresses.First(addr => addr.AddressFamily == AddressFamily.InterNetwork);
            return new Address(ipAddress.ToString(), port);
        }
        
        public static Address FromIPEndPoint(IPEndPoint ipEndPoint)
        {
            return new Address(ipEndPoint.Address.ToString(), ipEndPoint.Port);
        }

        public static bool operator ==(Address x, Address y)
        {
            return x.Port == y.Port &&
                   string.Equals(x.Ip, y.Ip, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool operator !=(Address x, Address y) => !(x == y);

        public override bool Equals(object obj)
        {
            return obj is Address other && this == other;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Ip, Port);
        }

        public override string ToString()
        {
            return $"{Ip}:{Port}";
        }
    }
}