using System;

namespace MathLibrary
{
    /// <summary>
    /// 表示RGBA颜色
    /// </summary>
    public struct Color : IEquatable<Color>
    {
        public float x => r;
        public float y => g;
        public float z => b;
        public float w => a;

        public float r;
        public float g;
        public float b;
        public float a;

        public Color(float r = 1f, float g = 1f, float b = 1f, float a = 1f)
        {
            this.r = Mathf.Clamp01(r);
            this.g = Mathf.Clamp01(g);
            this.b = Mathf.Clamp01(b);
            this.a = Mathf.Clamp01(a);
        }

        /// <summary>
        /// 从32位整数创建颜色
        /// </summary>
        public static Color FromInt32(uint color)
        {
            float r = ((color >> 24) & 0xFF) / 255f;
            float g = ((color >> 16) & 0xFF) / 255f;
            float b = ((color >> 8) & 0xFF) / 255f;
            float a = (color & 0xFF) / 255f;
            return new Color(r, g, b, a);
        }

        /// <summary>
        /// 转换为32位整数
        /// </summary>
        public uint ToInt32()
        {
            uint ir = (uint)(r * 255);
            uint ig = (uint)(g * 255);
            uint ib = (uint)(b * 255);
            uint ia = (uint)(a * 255);
            return (ir << 24) | (ig << 16) | (ib << 8) | ia;
        }

        /// <summary>
        /// 线性插值
        /// </summary>
        public static Color Lerp(Color a, Color b, float t)
        {
            t = Mathf.Clamp01(t);
            return new Color(
                a.r + (b.r - a.r) * t,
                a.g + (b.g - a.g) * t,
                a.b + (b.b - a.b) * t,
                a.a + (b.a - a.a) * t
            );
        }

        public static Color operator +(Color a, Color b) => new Color(a.r + b.r, a.g + b.g, a.b + b.b, a.a + b.a);
        public static Color operator -(Color a, Color b) => new Color(a.r - b.r, a.g - b.g, a.b - b.b, a.a - b.a);
        public static Color operator *(Color a, float b) => new Color(a.r * b, a.g * b, a.b * b, a.a * b);
        public static Color operator /(Color a, float b) => new Color(a.r / b, a.g / b, a.b / b, a.a / b);
        public static bool operator ==(Color a, Color b) => a.r == b.r && a.g == b.g && a.b == b.b && a.a == b.a;
        public static bool operator !=(Color a, Color b) => !(a == b);

        public override bool Equals(object? obj) => obj is Color c && Equals(c);
        public bool Equals(Color other) => r == other.r && g == other.g && b == other.b && a == other.a;
        public override int GetHashCode() => HashCode.Combine(r, g, b, a);
        public override string ToString() => $"RGBA({r:F2}, {g:F2}, {b:F2}, {a:F2})";

        // 常见颜色
        public static Color white = new Color(1f, 1f, 1f, 1f);
        public static Color black = new Color(0f, 0f, 0f, 1f);
        public static Color red = new Color(1f, 0f, 0f, 1f);
        public static Color green = new Color(0f, 1f, 0f, 1f);
        public static Color blue = new Color(0f, 0f, 1f, 1f);
        public static Color yellow = new Color(1f, 1f, 0f, 1f);
        public static Color cyan = new Color(0f, 1f, 1f, 1f);
        public static Color magenta = new Color(1f, 0f, 1f, 1f);
        public static Color gray = new Color(0.5f, 0.5f, 0.5f, 1f);
        public static Color clear = new Color(0f, 0f, 0f, 0f);
    }
}
