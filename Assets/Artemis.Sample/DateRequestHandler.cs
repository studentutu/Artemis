using System;
using Artemis.Clients;
using Artemis.UserInterface;
using Artemis.ValueObjects;

namespace Artemis.Sample
{
    public class DateRequestHandler : Handler
    {
        public override void Bind(ArtemisClient ac)
        {
            ac.RegisterRequestHandler<DateTime>(Handle);
        }

        private static void Handle(Request<DateTime> request)
        {
            request.Reply(DateTime.UtcNow);
        }
    }
}