using System.Runtime.InteropServices;

public static class Keyboard
{
    // https://docs.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes
    public enum Key
    {
        LMB = 0x01,
        RMB = 0x02,
        MMB = 0x04,
        A = 0x41,
        B = 0x42,
        C = 0x43,
        D = 0x44,
        E = 0x45,
        F = 0x46,
        G = 0x47,
        H = 0x48,
        I = 0x49,
        J = 0x4A,
        K = 0x4B,
        L = 0x4C,
        M = 0x4D,
        N = 0x4E,
        O = 0x4F,
        P = 0x50,
        Q = 0x51,
        R = 0x52,
        S = 0x53,
        T = 0x54,
        U = 0x55,
        V = 0x56,
        W = 0x57,
        X = 0x58,
        Y = 0x59,
        Z = 0x5A
    }

    public static bool GetKey(Key key)
    {
        return GetKeyState(key) < 0;
    }

    [DllImport("user32.dll")] private static extern short GetKeyState(Key key);
}