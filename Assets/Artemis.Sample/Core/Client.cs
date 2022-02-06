using System;
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
            await _client.Request(new ConnectionRequest(), _serverAddress);
            _state = State.Connected;
        }
        catch (Exception)
        {
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