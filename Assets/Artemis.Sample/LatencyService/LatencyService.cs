using System;
using System.Threading;
using System.Threading.Tasks;
using Artemis.Sample.Core;
using Artemis.Utilities;
using UnityEngine;

public class LatencyService : MonoBehaviour
{
    private readonly Ping _ping = new Ping();
    private readonly TimeSpan _pingTimeout = TimeSpan.FromSeconds(1);
    private readonly TimeSpan _pingInterval = TimeSpan.FromMilliseconds(250);
    
    public DapperClient _dapperClient;
    public double RoundTripTime;

    private void Start()
    {
        Loop(gameObject.GetOnDestroyCancellationToken()).Forget();
    }

    private async Task Loop(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            await Task.Delay(_pingInterval, ct);
            RoundTripTime = await CalculateRoundTrpTime(ct);
        }
    }
    
    private async Task<double> CalculateRoundTrpTime(CancellationToken ct)
    {
        if (_dapperClient.Current.GetType() != typeof(ClientConnectedState))
        {
            return await Task.FromResult(-1);
        }
        
        var timeAtRequest = DateTime.Now;
        await _dapperClient._client.RequestAsync(_ping, _dapperClient.ServerAddress, _pingTimeout, ct);
        var timeAtResponse = DateTime.Now;
        return (timeAtResponse - timeAtRequest).TotalMilliseconds;
    }
}
