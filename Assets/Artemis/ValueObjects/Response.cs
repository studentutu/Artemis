﻿using System;

namespace Artemis.ValueObjects
{
    [Serializable]
    public class Response
    {
        public readonly string Id;
        public readonly object Payload;

        public Response(string id, object payload)
        {
            Id = id;
            Payload = payload;
        }
    }
}