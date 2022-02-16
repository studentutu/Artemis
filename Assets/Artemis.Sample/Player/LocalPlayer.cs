using System.Collections.Generic;
using Artemis.Sample;
using Artemis.Sample.Core;
using UnityEngine;
using Artemis.Sample.Packets;
using Artemis.Sample.Player;
using Artemis.Threading;

public class LocalPlayer : BasePlayer
{
    public int UnconfirmedCommandCount;

    private readonly List<PlayerCommand> _unconfirmedCommands = new();

    private void Update()
    {
        UnconfirmedCommandCount = _unconfirmedCommands.Count;
        Predict();
    }

    private void Predict()
    {
        var horizontal = Keyboard.GetAxis(Keyboard.Key.D, Keyboard.Key.A);
        var vertical = Keyboard.GetAxis(Keyboard.Key.W, Keyboard.Key.S);
        var input = new Vector2(horizontal, vertical);
        var motion = Vector2.ClampMagnitude(input, 1f) * Configuration.PlayerMovementSpeed * Time.deltaTime;
        transform.Translate(motion);
    }
    
    public void OnFixedUpdate(DapperClient client, int tick)
    {
        var command = GetCommandForTick(tick);
        client._client.SendUnreliableMessage(command, client.ServerAddress);
        _unconfirmedCommands.Add(command);
    }

    private static PlayerCommand GetCommandForTick(int tick)
    {
        var horizontal = Keyboard.GetAxis(Keyboard.Key.D, Keyboard.Key.A);
        var vertical = Keyboard.GetAxis(Keyboard.Key.W, Keyboard.Key.S);
        return new PlayerCommand(tick, horizontal, vertical);
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