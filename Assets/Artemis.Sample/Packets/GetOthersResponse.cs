using System;
using Artemis.Sample.Core;
using System.Collections.Generic;

namespace Artemis.Sample.Packets
{
    [Serializable]
    public class GetOthersResponse
    {
        public readonly List<PlayerData> Others;

        public GetOthersResponse(List<PlayerData> others)
        {
            Others = others;
        }
    }
}