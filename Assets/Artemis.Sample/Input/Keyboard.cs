using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;

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
        if (ApplicationIsActivated())
        {
            return GetKeyState(key) < 0;
        }

        return false;
    }
    
    public static int GetAxis(Key positive, Key negative)
    {
        if (GetKey(positive))
        {
            return +1;
        }

        if (GetKey(negative))
        {
            return -1;
        }

        return 0;
    }
    
    public static bool ApplicationIsActivated()
    {
        var activatedHandle = GetForegroundWindow();
        if (activatedHandle == IntPtr.Zero) {
            return false;       // No window is currently activated
        }

        GetWindowThreadProcessId(activatedHandle, out var activeProcId);
        return activeProcId == Process.GetCurrentProcess().Id;
    }


    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);

    [DllImport("user32.dll")] private static extern short GetKeyState(Key key);
}