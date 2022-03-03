using System.Net;

namespace Artemis.Extensions
{
    internal static class IPEndPointExtensions
    {
        internal static IPEndPoint Copy(this IPEndPoint ipEndPoint)
        {
            return new IPEndPoint(ipEndPoint.Address, ipEndPoint.Port);
        }
    }
}