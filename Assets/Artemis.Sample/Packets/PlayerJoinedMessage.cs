using System;
using Artemis.Sample.Core;

[Serializable]
public class PlayerJoinedMessage
{
    public readonly PlayerData PlayerData;

    public PlayerJoinedMessage(PlayerData playerData)
    {
        PlayerData = playerData;
    }
}