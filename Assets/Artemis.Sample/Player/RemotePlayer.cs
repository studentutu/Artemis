using System;
using Artemis.Sample;
using Artemis.Sample.Core;
using UnityEngine;

public class RemotePlayer : BasePlayer
{
    public readonly Artemis.Sample.Generics.Memory<Timed<PlayerData>> SnapshotBuffer = new();

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
        var fractionalTickNow = elapsed * Configuration.TicksPerSecond;
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
        SnapshotBuffer.RemoveExpiredItems();

        for (int i = 0; i < SnapshotBuffer.Count - 1; i++)
        {
            if (SnapshotBuffer[i].Tick < time && time <= SnapshotBuffer[i + 1].Tick)
            {
                prev = SnapshotBuffer[i];
                next = SnapshotBuffer[i + 1];
                return true;
            }
        }

        prev = next = default;
        return false;
    }

    public override void OnSnapshotReceived(int tick, PlayerData snapshot)
    {
        SnapshotBuffer.Add(new Timed<PlayerData>(snapshot, tick), DateTime.Now.AddSeconds(2));
    }
}