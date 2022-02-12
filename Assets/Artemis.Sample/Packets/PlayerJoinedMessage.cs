using System;
using Artemis.Sample.Player;

[Serializable]
public class PlayerJoinedMessage
{
    public float X;
    public float Y;
    public readonly Guid PlayerId;
    public readonly string Nickname;
    public readonly bool IsLocalPlayer;
    public readonly HSV Color;

    public PlayerJoinedMessage(Guid playerId, string nickname, bool isLocalPlayer, HSV color, float x, float y)
    {
        X = x;
        Y = y;
        PlayerId = playerId;
        Nickname = nickname;
        IsLocalPlayer = isLocalPlayer;
        Color = color;
    }
}