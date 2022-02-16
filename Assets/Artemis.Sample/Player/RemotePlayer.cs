using System;
using Artemis.Sample;
using Artemis.Sample.Core;
using UnityEngine;

public class RemotePlayer : BasePlayer
{
    private readonly Artemis.Sample.Generics.Memory<Timed<PlayerData>> _snapshotBuffer = new();

    [SerializeField] private NetClock _netClock;
    [SerializeField] private DapperClient _dapperClient;

    private void Start()
    {
        _netClock = FindObjectOfType<NetClock>();
        _dapperClient = FindObjectOfType<DapperClient>();
    }

    private void Update()
    {
        var elapsed = (_netClock.PredictServerTime() - _dapperClient.ServerTimeAtFirstTick).TotalSeconds;
        var fractionalTickNow = elapsed * Configuration.FixedUpdateRate;
        var renderTime = fractionalTickNow - 2; // Interpolation window

        if (!TryFindSnapshots(renderTime, out var prev, out var next))
        {
            return;
        }
        
        var interpolationPercentage = Mathf.InverseLerp(prev.Tick, next.Tick, (float) renderTime);
        transform.position = Vector2.Lerp(prev.Value.Position, next.Value.Position, interpolationPercentage);
    }
    
    private bool TryFindSnapshots(double time, out Timed<PlayerData> prev, out Timed<PlayerData> next)
    {
        _snapshotBuffer.RemoveExpiredItems();

        for (int i = 0; i < _snapshotBuffer.Count - 1; i++)
        {
            if (_snapshotBuffer[i].Tick < time && time <= _snapshotBuffer[i + 1].Tick)
            {
                prev = _snapshotBuffer[i];
                next = _snapshotBuffer[i + 1];
                return true;
            }
        }

        prev = next = default;
        return false;
    }

    public override void OnSnapshotReceived(int tick, PlayerData snapshot)
    {
        _snapshotBuffer.Add(new Timed<PlayerData>(snapshot, tick), DateTime.Now.AddSeconds(2));
    }
}