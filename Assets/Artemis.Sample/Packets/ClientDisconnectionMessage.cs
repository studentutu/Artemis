using System;

[Serializable]
public class ClientDisconnectionMessage
{
    public readonly Guid PlayerId;

    public ClientDisconnectionMessage(Guid playerId)
    {
        PlayerId = playerId;
    }
}