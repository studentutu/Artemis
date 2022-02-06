using System;
using UnityEngine;
using Artemis.Sample;
using Artemis.Clients;
using Artemis.ValueObjects;
using UnityEngine.Assertions;
using System.Collections.Generic;

public partial class Server : MonoBehaviour
{
    private enum State { Closed, Open }
    private State _state;
    private ArtemisClient _client;
    private readonly List<Address> _connections = new List<Address>();

    private void OnDestroy()
    {
        Close();
    }

    private void Open()
    {
        Assert.IsTrue(_state == State.Closed);
        _client = new ArtemisClient(Array.Empty<Handler>(), Constants.ServerPort);
        _client.RegisterRequestHandler<ConnectionRequest>(HandleConnectionRequest);
        _client.RegisterMessageHandler<ClientDisconnectionMessage>(HandleClientDisconnectionMessage);
        _client.Start();
        _state = State.Open;
    }

    private void Close()
    {
        if (_state == State.Open)
        {
            _connections.ForEach(connection => _client.SendMessage(new ServerClosingMessage(), connection, DeliveryMethod.Unreliable));
        }
        
        _client?.Dispose();
        _client = null;
        _state = State.Closed;
    }
}