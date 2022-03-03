using System;
using System.Net;
using System.Threading.Tasks;
using Artemis.Clients;
using FluentAssertions;
using NUnit.Framework;

namespace Tests
{
    public class Artemis
    {
        [Test]
        public void Request_WhenNobodyIsListening_ShouldTimeout()
        {
            using var client = new ArtemisClient();
            client.Start();
            var requestTask = client.RequestAsync(default(int), new IPEndPoint(IPAddress.Loopback, 0));
            Action request = () => requestTask.Wait();
            request.Should().Throw<TaskCanceledException>();
        }
    
        [Test]
        public void Request_WhenSomebodyIsListening_ShouldRespond()
        {
            var serverAddress = new IPEndPoint(IPAddress.Loopback, 12345);
            using var client = new ArtemisClient();
            client.Start();
            using var server = new ArtemisClient(serverAddress.Port);
            server.RegisterRequestHandler<int>(req => req.Reply(42));
            server.Start();

            client.RequestAsync(default(int), serverAddress).Result.Should().Be(42);
        }
    }
}
