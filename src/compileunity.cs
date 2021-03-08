
#define Vec3Int

#if UNITY_5_OR_NEWER

#define UNITY

#else

using System.Runtime.InteropServices;
using System.IO;
using System;

public static class Debug
{
    public static void Log(in string msg)
    {
        Console.Error.WriteLine(msg);
    }
}

public struct Quaternion
{
    public float x;
    public float y;
    public float z;
    public float w;

    public Quaternion(in float x, in float y, in float z, in float w) { this.x = x; this.y = y; this.z = z; this.w = w; }
}

public struct Vector3
{
    public float x;
    public float y;
    public float z;
    public Vector3(in float x, in float y, in float z) { this.x = x; this.y = y; this.z = z; }
}

public struct Vector2
{
    public float x;
    public float y;
    public Vector2(in float x, in float y) { this.x = x; this.y = y; }
}

[StructLayout(LayoutKind.Explicit)]
public struct Color32
{
    [FieldOffset(0)]
    public int data;
    [FieldOffset(0)]
    public byte r;
    [FieldOffset(1)]
    public byte g;
    [FieldOffset(2)]
    public byte b;
    [FieldOffset(3)]
    public byte a;
    public Color32(in byte r, in byte g, in byte b, in byte a) { this.data = 0; this.r = r; this.g = g; this.b = b; this.a = a; }
}

public struct Color
{
    public float r;
    public float g;
    public float b;
    public float a;

    public static explicit operator Color32(Color c)
    {
        return new Color32((byte)(c.r * 255), (byte)(c.g * 255), (byte)(c.b * 255), (byte)(c.a * 255));
    }
}

#endif

#if Vec3Int
public struct Vector3Int
{
    public int x;
    public int y;
    public int z;
    public Vector3Int(in int x, in int y, in int z) { this.x = x; this.y = y; this.z = z; }
}
#endif