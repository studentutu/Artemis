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

    private Timed<Vector2> _lastConfirmedPosition;
    
    [SerializeField] private NetClock _netClock;
    [SerializeField] private DapperClient _dapperClient;

    private void Start()
    {
        _netClock = FindObjectOfType<NetClock>();
        _dapperClient = FindObjectOfType<DapperClient>();
    }

    private void Update()
    {
        UnconfirmedCommandCount = _unconfirmedCommands.Count;
        
        var vertical = (int) Input.GetAxisRaw("Vertical");
        var horizontal = (int) Input.GetAxisRaw("Horizontal");

        if (vertical != 0) _vertical = vertical;
        if (horizontal != 0) _horizontal = horizontal;
        
        // Render
        Render();
    }

    private void Render()
    {
        var elapsed = (_netClock.PredictServerTime() - _dapperClient.ServerTimeAtFirstTick).TotalSeconds;
        var fractionalTickNow = elapsed * Configuration.TicksPerSecond;
        var renderTime = fractionalTickNow - 0.5f; // Interpolation window
        
        var interpolationPercentage = Mathf.InverseLerp(_lastConfirmedPosition.Tick, (int)fractionalTickNow, (float) renderTime);
        var predictedPosition = PredictPosition();
        transform.position = Vector2.Lerp(_lastConfirmedPosition.Value, predictedPosition, interpolationPercentage);
    }
    
    public void OnFixedUpdate(DapperClient client, int tick)
    {
        var command = GetCommandForTick(tick);
        client._client.SendUnreliableMessage(command, client.ServerAddress);
        _unconfirmedCommands.Add(command);
    }

    private Vector2 PredictPosition()
    {
        var position = _lastConfirmedPosition.Value;

        foreach (var unconfirmedCommand in _unconfirmedCommands)
        {
            position = MovePlayer.Move(position, unconfirmedCommand);
        }

        return position;
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
        _lastConfirmedPosition = new Timed<Vector2>(snapshot.Position, tick);
    }
}