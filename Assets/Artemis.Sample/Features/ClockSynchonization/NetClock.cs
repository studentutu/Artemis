using System;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using Artemis.Sample.Extensions;
using Artemis.Sample.Features.ClockSynchonization;
using Artemis.Sample.Features.ReadOnlyField;

public class NetClock : MonoBehaviour
{
    [field: SerializeField, ReadOnly] public string Offset { get; private set; }
    private TimeSpan _offset;

    private void Update()
    {
        Offset = _offset.ToReadableString();
    }

    public async Task Synchronize(Client client, CancellationToken ct)
    {
        var timeAtRequest = DateTime.Now;
        var response = await GetServerTime(client, ct);
        var timeAtResponse = DateTime.Now;
        var roundTripTime = timeAtResponse - timeAtRequest;
        var latency = roundTripTime / 2f;
        var serverTimeNow = response.Time + latency;
        _offset = serverTimeNow - DateTime.Now;
        client.ServerTimeAtFirstTick = response.TimeAtFirstTick;
    }

    private Task<GetTimeResponse> GetServerTime(Client client, CancellationToken ct)
    {
        var request = new GetTimeRequest();
        var serverAddress = client.ServerAddress;
        return client._client.RequestAsync(request, serverAddress, ct)
            .ContinueWith(t => (GetTimeResponse) t.Result, ct);
    }

    public DateTime PredictServerTime()
    {
        return DateTime.Now + _offset;
    }
}