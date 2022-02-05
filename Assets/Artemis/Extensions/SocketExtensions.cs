using System.Net.Sockets;

public static class SocketExtensions
{
    public static void DontReportUnreachableEndPoint(this Socket socket)
    {
        // https://docs.microsoft.com/en-us/windows/win32/winsock/winsock-ioctls#sio_udp_connreset-opcode-setting-i-t3
        const int SIO_UDP_CONNRESET = -1744830452; //SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12
        socket.IOControl((IOControlCode) SIO_UDP_CONNRESET, new byte[] {0, 0, 0, 0}, null);
    }
}