using Artemis.Clients;

namespace Artemis.ValueObjects
{
    public abstract class Handler
    {
        public abstract void Bind(ArtemisClient ac);
    }
}