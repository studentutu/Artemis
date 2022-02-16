using UnityEngine;
using Artemis.Sample.Core;
using Artemis.Sample.Input;
using Artemis.Sample.Packets;
using Artemis.Sample.Player;

public class LocalPlayer : BasePlayer
{
    [SerializeField] private PredictedLocalPlayer _predictedLocalPlayer;
    [SerializeField] private UnpredictedLocalPlayer _unpredictedLocalPlayer;

    public void OnFixedUpdate(DapperClient client, int tick)
    {
        var command = new PlayerCommand(tick, DapperInput.GetMovementInput());
        client._client.SendUnreliableMessage(command, client.ServerAddress);
        _predictedLocalPlayer.OnCommandSent(command);
    }

    public override void OnSnapshotReceived(int tick, PlayerData snapshot)
    {
        _predictedLocalPlayer.OnSnapshotReceived(tick, snapshot);
        _unpredictedLocalPlayer.OnSnapshotReceived(tick, snapshot);
        
    }
}