using UnityEngine;
using static UnityEngine.Mathf;

public static class FunctionLibrary
{
    public delegate float Function(float x, float z);
    public delegate Vector3 Function3D(float u, float v, float t);

    public enum FunctionName { Wave, MultiWave, Ripple, Sphere, Torus }

    static Function[] functions = { Wave, MultiWave, Ripple };
    static Function3D[] functions3D = { Wave, MultiWave, Ripple, Sphere, Torus };

    public static FunctionName GetNextFunctionName(FunctionName name)
    {
        return (int)name < functions3D.Length - 1 ? name + 1: 0;
    }

    public static FunctionName GetRandomFunctionNameOtherThan(FunctionName name)
    {
        var choice = (FunctionName)Random.Range(1, functions3D.Length);
        return choice == name ? 0 : choice;
    }

    public static int FunctionCount => functions3D.Length;

    public static Vector3 Morph(float u, float v, float t,
        Function3D from, Function3D to, float progress)
    {
        return Vector3.LerpUnclamped(
            from(u, v, t), to(u, v, t), SmoothStep(0f, 1f, progress));
    }

    public static Function GetFunction(FunctionName name)
    {
        Debug.Assert((int)name < functions.Length, "can't show in 2d graph");
        return functions[(int)name];
    }

    public static Function3D GetFunction3D(FunctionName name)
    {
        return functions3D[(int)name];
    }

    public static float Wave(float x, float t)
    {
        return Sin(PI * (x + t));
    }

    public static float MultiWave(float x, float t)
    {
        float y = Sin(PI * (x + 0.5f * t));
        y += 0.5f * Sin(2f * PI * (x + t));
        return y * (2f / 3f);
    }

    public static float Ripple(float x, float t)
    {
        float d = Abs(x);
        float y = Sin(PI *(4f * d - t));
        return y / (1f + 10f * d);
    }

    public static Vector3 Wave(float u, float v, float t)
    {
        Vector3 p = new Vector3(u, 0, v);
        p.y = Sin(PI * (u + v + t));
        return p;
    }

    public static Vector3 MultiWave(float u, float v, float t)
    {
        Vector3 p = new Vector3(u, 0, v);

        p.y = Sin(PI * (u + 0.5f * t));
        p.y += 0.5f * Sin(2f * PI * (v + t));
        p.y += Sin(PI * (u + v + 0.25f * t));
        p.y *= 1f / 2.5f;

        return p;
    }

    public static Vector3 Ripple(float u, float v, float t)
    {
        float d = Sqrt(u * u + v * v);
        Vector3 p = new Vector3(u, 0, v);
        p.y = Sin(PI * (4f * d - t));
        p.y /= 1f + 10f * d;
        return p;
    }

    public static Vector3 Sphere(float u, float v, float t)
    {
        float r = 0.9f + 0.1f * Sin(PI * (6f * u + 4f * v + t));
        float s = r * Cos(0.5f * PI * v);

        Vector3 p = new Vector3(s * Sin(PI * u), r * Sin(PI * 0.5f * v), s * Cos(PI * u));
        return p;
    }

    public static Vector3 Torus(float u, float v, float t)
    {
        float r1 = 0.7f + 0.1f * Sin(PI * (6f * u + 0.5f * t));
        float r2 = 0.15f + 0.05f * Sin(PI * (8f * u + 4f * v + 2f * t));
        float s = r1 + r2 * Cos(PI * v);

        Vector3 p = new Vector3(s * Sin(PI * u), r2 * Sin(PI * v), s * Cos(PI * u));
        return p;
    }
}
