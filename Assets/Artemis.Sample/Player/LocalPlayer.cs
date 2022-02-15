using UnityEngine;
using Artemis.Sample.Packets;

public class LocalPlayer : Player
{
    [SerializeField] private int _horizontal;
    [SerializeField] private int _vertical;
    
    private void Update()
    {
        var horizontal = (int) Input.GetAxisRaw("Horizontal");
        if (horizontal != 0)
        {
            _horizontal = horizontal;
        }
        
        var vertical = (int) Input.GetAxisRaw("Vertical");
        if (vertical != 0)
        {
            _vertical = vertical;
        }
    }

    public void OnNetFixedUpdate(DapperClient client, int tick)
    {
        var playerCommand = new PlayerCommand(tick, _horizontal, _vertical);
        client._client.SendUnreliableMessage(playerCommand, client.ServerAddress);
        _horizontal = _vertical = 0;
    }
}