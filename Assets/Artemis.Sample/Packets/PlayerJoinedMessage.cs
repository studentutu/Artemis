using System;
using Artemis.Sample.ValueObjects;

[Serializable]
public class PlayerJoinedMessage
{
    public readonly Guid PlayerId;
    public readonly string Nickname;
    public readonly Color Color;
    public Float2 Position;

    public PlayerJoinedMessage(Guid playerId, string nickname, Color color, Float2 position)
    {
        PlayerId = playerId;
        Nickname = nickname;
        Color = color;
        Position = position;
    }
}