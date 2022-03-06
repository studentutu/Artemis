using System;
using System.Net;
using Artemis.Clients;
using FluentAssertions;
using NUnit.Framework;

namespace Tests
{
    public class ArtemisClientTests
    {
        [Test]
        public void Request_WhenNobodyIsListening_ShouldTimeout()
        {
            // Client
            using var client = new ArtemisClient();
            client.Start();
            
            // Assert
            var requestTask = client.RequestAsync(default(int), new IPEndPoint(IPAddress.Loopback, 0));
            Action request = () => requestTask.Wait();
            request.Should().Throw<TimeoutException>();
        }
    
        [Test]
        public void Request_WhenSomebodyIsListening_ShouldRespond()
        {
            // Client
            using var client = new ArtemisClient();
            client.Start();
            
            // Server
            var serverAddress = new IPEndPoint(IPAddress.Loopback, 12345);
            using var server = new ArtemisClient(serverAddress.Port);
            server.RegisterRequestHandler<int>(req => req.Reply(42));
            server.Start();

            // Assert
            client.RequestAsync(default(int), serverAddress).Result.Should().Be(42);
        }
    }
}