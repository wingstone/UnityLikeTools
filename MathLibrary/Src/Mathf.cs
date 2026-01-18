using System;

namespace MathLibrary
{
    /// <summary>
    /// 数学函数辅助类
    /// </summary>
    public static class Mathf
    {
        public const float PI = 3.14159265358979323846f;
        public const float Tau = 2f * PI;
        public const float Epsilon = 0.00000001f;
        public const float Deg2Rad = PI / 180f;
        public const float Rad2Deg = 180f / PI;

        /// <summary>
        /// 返回绝对值
        /// </summary>
        public static float Abs(float value) => MathF.Abs(value);

        /// <summary>
        /// 返回平方根
        /// </summary>
        public static float Sqrt(float value) => MathF.Sqrt(value);

        /// <summary>
        /// 返回绝对值
        /// </summary>
        public static int Abs(int value) => Math.Abs(value);

        /// <summary>
        /// 返回最小值
        /// </summary>
        public static float Min(float a, float b) => a < b ? a : b;
        public static int Min(int a, int b) => a < b ? a : b;

        /// <summary>
        /// 返回最大值
        /// </summary>
        public static float Max(float a, float b) => a > b ? a : b;
        public static int Max(int a, int b) => a > b ? a : b;

        /// <summary>
        /// 将值限制在min和max之间
        /// </summary>
        public static float Clamp(float value, float min, float max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        public static int Clamp(int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        /// <summary>
        /// 将值限制在0和1之间
        /// </summary>
        public static float Clamp01(float value) => Clamp(value, 0f, 1f);

        /// <summary>
        /// 线性插值
        /// </summary>
        public static float Lerp(float a, float b, float t) => a + (b - a) * Clamp01(t);

        /// <summary>
        /// 无限制的线性插值
        /// </summary>
        public static float LerpUnclamped(float a, float b, float t) => a + (b - a) * t;

        /// <summary>
        /// 反向线性插值
        /// </summary>
        public static float InverseLerp(float a, float b, float value)
        {
            if (a == b) return 0f;
            return (value - a) / (b - a);
        }

        /// <summary>
        /// 平滑阶跃插值 (Smoothstep)
        /// </summary>
        public static float SmoothStep(float a, float b, float t)
        {
            float x = Clamp01((t - a) / (b - a));
            return x * x * (3f - 2f * x);
        }

        /// <summary>
        /// 舍入到最近的整数
        /// </summary>
        public static float Round(float value) => MathF.Round(value);
        public static int RoundToInt(float value) => (int)MathF.Round(value);

        /// <summary>
        /// 向下取整
        /// </summary>
        public static float Floor(float value) => MathF.Floor(value);
        public static int FloorToInt(float value) => (int)MathF.Floor(value);

        /// <summary>
        /// 向上取整
        /// </summary>
        public static float Ceil(float value) => MathF.Ceiling(value);
        public static int CeilToInt(float value) => (int)MathF.Ceiling(value);

        /// <summary>
        /// 返回remainder / divisor的余数
        /// </summary>
        public static float Repeat(float t, float length) => t - MathF.Floor(t / length) * length;

        /// <summary>
        /// 返回pingpong值（在0和length之间来回摆动）
        /// </summary>
        public static float PingPong(float t, float length)
        {
            float mod = Repeat(t, length * 2f);
            return length - MathF.Abs(mod - length);
        }

        /// <summary>
        /// 正弦函数（弧度）
        /// </summary>
        public static float Sin(float value) => MathF.Sin(value);

        /// <summary>
        /// 余弦函数（弧度）
        /// </summary>
        public static float Cos(float value) => MathF.Cos(value);

        /// <summary>
        /// 正切函数（弧度）
        /// </summary>
        public static float Tan(float value) => MathF.Tan(value);

        /// <summary>
        /// 反正弦函数（弧度）
        /// </summary>
        public static float Asin(float value) => MathF.Asin(value);

        /// <summary>
        /// 反余弦函数（弧度）
        /// </summary>
        public static float Acos(float value) => MathF.Acos(value);

        /// <summary>
        /// 反正切函数（弧度）
        /// </summary>
        public static float Atan(float value) => MathF.Atan(value);

        /// <summary>
        /// Atan2函数（弧度）
        /// </summary>
        public static float Atan2(float y, float x) => MathF.Atan2(y, x);

        /// <summary>
        /// 返回value的幂
        /// </summary>
        public static float Pow(float value, float power) => MathF.Pow(value, power);

        /// <summary>
        /// 返回e的power次方
        /// </summary>
        public static float Exp(float power) => MathF.Exp(power);

        /// <summary>
        /// 返回自然对数
        /// </summary>
        public static float Log(float value) => MathF.Log(value);

        /// <summary>
        /// 返回以10为底的对数
        /// </summary>
        public static float Log10(float value) => MathF.Log10(value);

        /// <summary>
        /// 返回以base为底的对数
        /// </summary>
        public static float Log(float value, float baseValue) => MathF.Log(value, baseValue);

        /// <summary>
        /// 检查两个浮点数是否近似相等
        /// </summary>
        public static bool Approximately(float a, float b) => Abs(b - a) <= Max(0.000001f * Max(Abs(a), Abs(b)), Epsilon * 8f);

        /// <summary>
        /// 将角度转换为弧度
        /// </summary>
        public static float DegreesToRadians(float degrees) => degrees * Deg2Rad;

        /// <summary>
        /// 将弧度转换为角度
        /// </summary>
        public static float RadiansToDegrees(float radians) => radians * Rad2Deg;

        /// <summary>
        /// 符号函数（返回-1, 0或1）
        /// </summary>
        public static float Sign(float value)
        {
            if (value > 0) return 1f;
            if (value < 0) return -1f;
            return 0f;
        }

        public static int Sign(int value)
        {
            if (value > 0) return 1;
            if (value < 0) return -1;
            return 0;
        }

        /// <summary>
        /// 平方
        /// </summary>
        public static float Sqr(float value) => value * value;

        /// <summary>
        /// 立方
        /// </summary>
        public static float Cube(float value) => value * value * value;

        /// <summary>
        /// 判断一个数是否为质数
        /// </summary>
        private static bool IsPrime(int n)
        {
            if (n <= 1) return false;
            if (n <= 3) return true;
            if (n % 2 == 0 || n % 3 == 0) return false;
            
            for (int i = 5; i * i <= n; i += 6)
            {
                if (n % i == 0 || n % (i + 2) == 0)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 获取第n个质数
        /// </summary>
        private static int GetPrime(int n)
        {
            if (n <= 0) return 2;
            
            int count = 0;
            int candidate = 1;
            
            while (count <= n)
            {
                candidate++;
                if (IsPrime(candidate))
                {
                    count++;
                }
            }
            return candidate;
        }

        /// <summary>
        /// Van der Corput序列生成（核心算法）
        /// </summary>
        private static float VanDerCorput(int n, int baseNumber)
        {
            float result = 0.0f;
            float fraction = 1.0f / baseNumber;
            int index = n;
            
            while (index > 0)
            {
                result += (index % baseNumber) * fraction;
                index /= baseNumber;
                fraction /= baseNumber;
            }
            
            return result;
        }

        /// <summary>
        /// 生成Halton序列
        /// </summary>
        /// <param name="index">点的索引（从0开始）</param>
        /// <param name="baseNumberX">第一维的基数（必须是质数）</param>
        /// <param name="baseNumberY">第二维的基数（必须是质数）</param>
        /// <returns>Halton序列点</returns>
        public static Vector2 Halton(int index, int baseNumberX, int baseNumberY)
        {
            if (baseNumberX <= 1 || !IsPrime(baseNumberX))
                throw new ArgumentException("基数必须是大于1的质数", nameof(baseNumberX));
            if (baseNumberY <= 1 || !IsPrime(baseNumberY))
                throw new ArgumentException("基数必须是大于1的质数", nameof(baseNumberY));

            Vector2 result = Vector2.zero;
            int id = index + 1; // Halton序列通常从1开始

            result.x = VanDerCorput(id, baseNumberX);
            result.y = VanDerCorput(id, baseNumberY);

            return result;
        }
    
        /// <summary>
        /// 生成二维Hammersley序列点（基础版本）
        /// </summary>
        /// <param name="index">点的索引（从0开始）</param>
        /// <param name="sampleCount">采样数量</param>
        /// <returns>二维Hammersley序列点</returns>
        public static Vector2 Hammersley(int index, int sampleCount)
        {
            if (sampleCount <= 0) throw new ArgumentException("采样数量必须大于0");
            if (index < 0 || index >= sampleCount) throw new ArgumentException("索引必须在0到采样数量之间");

            Vector2 result = Vector2.zero;

            result.x = (index + 0.5f) / sampleCount; // 偏移0.5以避免边界问题
            result.y = VanDerCorput(index + 1, 2); // 通常使用基数2，从1开始，避免0的边界问题

            return result;
        }
    }
}
