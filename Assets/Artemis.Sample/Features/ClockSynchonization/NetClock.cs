using System;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using Artemis.Sample.Extensions;
using Artemis.Sample.Features.ClockSynchonization;

public class NetClock : MonoBehaviour
{
    [field: SerializeField] public string Offset { get; private set; }
    private TimeSpan _offset;

    private void Update()
    {
        Offset = _offset.ToReadableString();
    }

    public async Task Synchronize(DapperClient dapperClient, CancellationToken ct)
    {
        var timeAtRequest = DateTime.Now;
        var response = await GetServerTime(dapperClient, ct);
        var timeAtResponse = DateTime.Now;
        var roundTripTime = timeAtResponse - timeAtRequest;
        var latency = roundTripTime / 2f;
        var serverTimeNow = response.Time + latency;
        _offset = serverTimeNow - DateTime.Now;
        _offset = _offset.Add(latency + TimeSpan.FromMilliseconds(16)); // TODO How much the client must be ahead of server?
        dapperClient.ServerTimeAtFirstTick = response.TimeAtFirstTick;
    }

    private Task<GetTimeResponse> GetServerTime(DapperClient dapperClient, CancellationToken ct)
    {
        var request = new GetTimeRequest();
        var serverAddress = dapperClient.ServerAddress;
        return dapperClient._client.RequestAsync(request, serverAddress, ct)
            .ContinueWith(t => (GetTimeResponse) t.Result, ct);
    }

    public DateTime PredictServerTime()
    {
        return DateTime.Now + _offset;
    }
}