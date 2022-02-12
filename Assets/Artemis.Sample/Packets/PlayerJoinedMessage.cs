using System;

[Serializable]
public class PlayerJoinedMessage
{
    public readonly Guid PlayerId;
    public readonly string Nickname;
    public readonly bool IsLocalPlayer;

    public PlayerJoinedMessage(Guid playerId, string nickname, bool isLocalPlayer)
    {
        PlayerId = playerId;
        Nickname = nickname;
        IsLocalPlayer = isLocalPlayer;
    }
}