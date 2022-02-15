using UnityEngine;
using Artemis.Sample.Packets;
using Artemis.Sample.Player;
using Artemis.Threading;

public class LocalPlayer : Player
{
    private int _vertical;
    private int _horizontal;

    private void Update()
    {
        var vertical = (int) Input.GetAxisRaw("Vertical");
        var horizontal = (int) Input.GetAxisRaw("Horizontal");

        if (vertical != 0) _vertical = vertical;
        if (horizontal != 0) _horizontal = horizontal;
    }

    public void OnNetFixedUpdate(DapperClient client, int tick)
    {
        var command = GetCommandForTick(tick);
        client._client.SendUnreliableMessage(command, client.ServerAddress);

        UnityMainThreadDispatcher.Dispatch(() =>
        {
            transform.position = MovePlayer.Move(transform.position, command);
        });
    }

    private PlayerCommand GetCommandForTick(int tick)
    {
        var command = new PlayerCommand(tick, _horizontal, _vertical);
        _vertical = _horizontal = 0;
        return command;
    }
}