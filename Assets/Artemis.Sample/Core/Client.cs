using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Artemis.Sample;
using Artemis.Clients;
using Artemis.ValueObjects;
using UnityEngine.Assertions;

public partial class Client : MonoBehaviour
{
    private enum State { Null, Connecting, Connected }
    private ArtemisClient _client;
    private State _state = State.Null;
    private string _serverHostname = "localhost";
    private Address _serverAddress;

    private void OnDestroy()
    {
        Disconnect();
    }

    private async void Connect()
    {
        Assert.IsTrue(_state == State.Null);

        try
        {
            _state = State.Connecting;
            _client = new ArtemisClient(Array.Empty<Handler>());
            _client.RegisterMessageHandler<ServerClosingMessage>(HandleServerClosingMessage);
            _client.Start();
            _serverAddress = Address.FromHostname(_serverHostname, Constants.ServerPort);
            
            var timeout = TimeSpan.FromSeconds(3);
            var timeoutCt = new CancellationTokenSource(timeout).Token;
            var onDestroyCt = gameObject.GetOnDestroyCancellationToken();

            await _client.Request(
                new ConnectionRequest(),
                _serverAddress,
                CancellationTokenSource.CreateLinkedTokenSource(timeoutCt, onDestroyCt).Token);
            
            _state = State.Connected;
        }
        catch (Exception e) // It could have been a timeout or the user canceled the attempt
        {
            Debug.Log(e.GetType().FullName);
            
            if (e is TaskCanceledException)
            {
                // If user cancelled the attempt, do nothing
            }
            else if(e is TimeoutException)
            {
                
            }
            
            // If timeout, show server is unreachable
            Debug.LogWarning($"Connecting to {_serverHostname} failed");
            Disconnect();
        }
    }

    private void Disconnect()
    {
        if (_state == State.Connected)
        {
            _client.SendMessage(new ClientDisconnectionMessage(), _serverAddress, DeliveryMethod.Unreliable);
        }

        _state = State.Null;
        _client?.Dispose();
        _client = null;
    }
}