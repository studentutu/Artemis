using System;
using Artemis.Sample.Player;

[Serializable]
public class PlayerJoinedMessage
{
    public readonly Guid PlayerId;
    public readonly string Nickname;
    public readonly HSV Color;
    public float X;
    public float Y;

    public PlayerJoinedMessage(Guid playerId, string nickname, HSV color, float x, float y)
    {
        PlayerId = playerId;
        Nickname = nickname;
        Color = color;
        X = x;
        Y = y;
    }
}