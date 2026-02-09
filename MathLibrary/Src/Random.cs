using System;

namespace MathLibrary
{
    /// <summary>
    /// 随机数生成函数库
    /// </summary>
    public static class RandomUtil
    {
        private static System.Random _random = new System.Random();

        /// <summary>
        /// 返回0到1之间的随机浮点数（包括0，不包括1）
        /// </summary>
        public static float Range() => (float)_random.NextDouble();

        /// <summary>
        /// 返回min到max之间的随机浮点数（包括min，不包括max）
        /// </summary>
        public static float Range(float min, float max) => min + (float)_random.NextDouble() * (max - min);

        /// <summary>
        /// 返回min到max之间的随机整数（包括min，包括max）
        /// </summary>
        public static int Range(int min, int max) => _random.Next(min, max + 1);

        /// <summary>
        /// 返回0到max之间的随机整数（包括0，不包括max）
        /// </summary>
        public static int RangeInt(int max) => _random.Next(max);

        /// <summary>
        /// 返回0.0f到1.0f之间的随机浮点数
        /// </summary>
        public static float Value => (float)_random.NextDouble();

        /// <summary>
        /// 返回一个随机颜色
        /// </summary>
        public static Color Color() => new Color(Range(), Range(), Range());

        /// <summary>
        /// 返回-1到1之间的随机浮点数
        /// </summary>
        public static float SignedRange() => Range(-1f, 1f);

        /// <summary>
        /// 返回单位球内的随机向量
        /// </summary>
        public static Vector3 InsideUnitSphere()
        {
            Vector3 point;
            do
            {
                point = new Vector3(Range(-1f, 1f), Range(-1f, 1f), Range(-1f, 1f));
            } while (point.SqrMagnitude > 1f);
            return point;
        }

        /// <summary>
        /// 返回单位圆盘内的随机向量
        /// </summary>
        public static Vector2 InsideUnitCircle()
        {
            Vector2 point;
            do
            {
                point = new Vector2(Range(-1f, 1f), Range(-1f, 1f));
            } while (point.SqrMagnitude > 1f);
            return point;
        }

        /// <summary>
        /// 返回单位球面上的随机向量
        /// </summary>
        public static Vector3 OnUnitSphere()
        {
            float theta = Range(0f, Mathf.Tau);
            float phi = (float)Math.Acos(Range(-1f, 1f));
            return new Vector3(
                Mathf.Sin(phi) * Mathf.Cos(theta),
                Mathf.Sin(phi) * Mathf.Sin(theta),
                Mathf.Cos(phi)
            );
        }

        /// <summary>
        /// 返回单位圆周上的随机向量
        /// </summary>
        public static Vector2 OnUnitCircle()
        {
            float angle = Range(0f, Mathf.Tau);
            return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }

        /// <summary>
        /// 返回随机旋转四元数
        /// </summary>
        public static Quaternion Rotation()
        {
            // 使用均匀分布的四元数生成算法
            float u1 = Range();
            float u2 = Range();
            float u3 = Range();

            float sqrt1MinusU1 = Mathf.Sqrt(1f - u1);
            float sqrtU1 = Mathf.Sqrt(u1);

            return new Quaternion(
                sqrt1MinusU1 * Mathf.Sin(Mathf.Tau * u2),
                sqrt1MinusU1 * Mathf.Cos(Mathf.Tau * u2),
                sqrtU1 * Mathf.Sin(Mathf.Tau * u3),
                sqrtU1 * Mathf.Cos(Mathf.Tau * u3)
            );
        }

        /// <summary>
        /// 设置随机种子
        /// </summary>
        public static void SetSeed(int seed)
        {
            _random = new System.Random(seed);
        }

        /// <summary>
        /// 从数组中随机选择一个元素
        /// </summary>
        public static T Choose<T>(params T[] items)
        {
            if (items.Length == 0)
                throw new ArgumentException("数组不能为空");
            return items[_random.Next(items.Length)];
        }

        /// <summary>
        /// 根据概率返回true或false
        /// </summary>
        public static bool Probability(float chance) => Range() < chance;

        /// <summary>
        /// 返回加权随机选择
        /// </summary>
        public static int WeightedChoice(params float[] weights)
        {
            if (weights.Length == 0)
                throw new ArgumentException("权重数组不能为空");

            float sum = 0f;
            foreach (float w in weights)
            {
                if (w < 0) throw new ArgumentException("权重不能为负数");
                sum += w;
            }

            if (sum <= 0) throw new ArgumentException("权重和必须大于0");

            float random = Range(0f, sum);
            float current = 0f;

            for (int i = 0; i < weights.Length; i++)
            {
                current += weights[i];
                if (random < current)
                    return i;
            }

            return weights.Length - 1;
        }
    }
}
