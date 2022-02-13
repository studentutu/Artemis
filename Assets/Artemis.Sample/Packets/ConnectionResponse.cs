using System;
using Artemis.Sample.Core;

namespace Artemis.Sample
{
    [Serializable]
    public class ConnectionResponse
    {
        public readonly PlayerData PlayerData;

        public ConnectionResponse(PlayerData playerData)
        {
            PlayerData = playerData;
        }
    }
}