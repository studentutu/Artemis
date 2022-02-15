using System.Collections.Generic;
using Artemis.Sample;
using Artemis.Sample.Core;
using UnityEngine;
using Artemis.Sample.Packets;
using Artemis.Sample.Player;
using Artemis.Threading;

public class LocalPlayer : BasePlayer
{
    private int _vertical;
    private int _horizontal;

    public int UnconfirmedCommandCount;

    private readonly List<PlayerCommand> _unconfirmedCommands = new();

    private void Update()
    {
        UnconfirmedCommandCount = _unconfirmedCommands.Count;
        
        var vertical = (int) Input.GetAxisRaw("Vertical");
        var horizontal = (int) Input.GetAxisRaw("Horizontal");

        if (vertical != 0) _vertical = vertical;
        if (horizontal != 0) _horizontal = horizontal;
        
        // Render
        Predict();
    }

    private void Predict()
    {
        var input = new Vector2(_horizontal, _vertical);
        var motion = Vector2.ClampMagnitude(input, 1f) * Configuration.PlayerMovementSpeed * Time.deltaTime;
        transform.Translate(motion);
    }
    
    public void OnFixedUpdate(DapperClient client, int tick)
    {
        var command = GetCommandForTick(tick);
        client._client.SendUnreliableMessage(command, client.ServerAddress);
        _unconfirmedCommands.Add(command);
    }

    private PlayerCommand GetCommandForTick(int tick)
    {
        var command = new PlayerCommand(tick, _horizontal, _vertical);
        _vertical = _horizontal = 0;
        return command;
    }

    public override void OnSnapshotReceived(int tick, PlayerData snapshot)
    {
        _unconfirmedCommands.RemoveAll(uc => uc.Tick <= tick);
        
        UnityMainThreadDispatcher.Dispatch(() =>
        {
            // Snap hard to snapshot
            transform.position = new Vector2(snapshot.Position.X, snapshot.Position.Y);

            // Replay unconfirmed commands
            foreach (var unconfirmedCommand in _unconfirmedCommands)
            {
                transform.position = MovePlayer.Move(transform.position, unconfirmedCommand);
            }
        });
    }
}