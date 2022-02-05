using System;
using EasyButtons;
using rUDP.Sample;
using rUDP.Sandbox;
using rUDP.ValueObjects;
using UnityEngine;

public class Peer : MonoBehaviour
{
    public int BindTo;
    public int SendTo;
    public ReliableClient Client;
    public string VehicleBrand;

    [Button]
    public void Send()
    {
        var vehicle = new Vehicle {Brand = VehicleBrand};
        var recipient = Address.FromHostname("localhost", SendTo);
        Client.SendMessage(vehicle, recipient, DeliveryMethod.Reliable);
    }

    [Button]
    public async void Request()
    {
        var recipient = Address.FromHostname("localhost", SendTo);
        var response = await Client.Request(new DateTime(), recipient);
        Debug.Log(response);
    }

    private void Start()
    {
        Client = new ReliableClient(BindTo);
        Client.Start();
    }

    private void OnDestroy()
    {
        Client.Dispose();
    }
}