using System;
using MathLibrary;

namespace GeometryTools
{
    /// <summary>
    /// Provides utilities for working with Bezier curves.
    /// </summary>
    public class BezierCurve
    {
        /// <summary>
        /// Computes a point on a quadratic Bezier curve.
        /// </summary>
        /// <param name="t">Parameter from 0 to 1 along the curve.</param>
        /// <param name="p0">Start control point.</param>
        /// <param name="p1">Middle control point.</param>
        /// <param name="p2">End control point.</param>
        /// <returns>Point on the quadratic Bezier curve.</returns>
        public static Vector3 QuadraticBezier(float t, Vector3 p0, Vector3 p1, Vector3 p2)
        {
            if (t < 0f || t > 1f)
                throw new ArgumentException("Parameter t must be between 0 and 1");

            float mt = 1f - t;
            float mt2 = mt * mt;
            float t2 = t * t;

            return p0 * mt2 + p1 * (2f * mt * t) + p2 * t2;
        }

        /// <summary>
        /// Computes a point on a cubic Bezier curve.
        /// </summary>
        /// <param name="t">Parameter from 0 to 1 along the curve.</param>
        /// <param name="p0">First control point.</param>
        /// <param name="p1">Second control point.</param>
        /// <param name="p2">Third control point.</param>
        /// <param name="p3">Fourth control point.</param>
        /// <returns>Point on the cubic Bezier curve.</returns>
        public static Vector3 CubicBezier(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            if (t < 0f || t > 1f)
                throw new ArgumentException("Parameter t must be between 0 and 1");

            float mt = 1f - t;
            float mt2 = mt * mt;
            float mt3 = mt2 * mt;
            float t2 = t * t;
            float t3 = t2 * t;

            return p0 * mt3 + p1 * (3f * mt2 * t) + p2 * (3f * mt * t2) + p3 * t3;
        }

        /// <summary>
        /// Generates points along a quadratic Bezier curve.
        /// </summary>
        /// <param name="p0">Start control point.</param>
        /// <param name="p1">Middle control point.</param>
        /// <param name="p2">End control point.</param>
        /// <param name="segmentCount">Number of segments (points) to generate.</param>
        /// <returns>Array of points along the Bezier curve.</returns>
        public static Vector3[] GenerateQuadraticBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, int segmentCount)
        {
            if (segmentCount < 2)
                throw new ArgumentException("segmentCount must be at least 2");

            var points = new Vector3[segmentCount];
            for (int i = 0; i < segmentCount; i++)
            {
                float t = i / (float)(segmentCount - 1);
                points[i] = QuadraticBezier(t, p0, p1, p2);
            }
            return points;
        }

        /// <summary>
        /// Generates points along a cubic Bezier curve.
        /// </summary>
        /// <param name="p0">First control point.</param>
        /// <param name="p1">Second control point.</param>
        /// <param name="p2">Third control point.</param>
        /// <param name="p3">Fourth control point.</param>
        /// <param name="segmentCount">Number of segments (points) to generate.</param>
        /// <returns>Array of points along the Bezier curve.</returns>
        public static Vector3[] GenerateCubicBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, int segmentCount)
        {
            if (segmentCount < 2)
                throw new ArgumentException("segmentCount must be at least 2");

            var points = new Vector3[segmentCount];
            for (int i = 0; i < segmentCount; i++)
            {
                float t = i / (float)(segmentCount - 1);
                points[i] = CubicBezier(t, p0, p1, p2, p3);
            }
            return points;
        }

        /// <summary>
        /// Computes a 2D point on a quadratic Bezier curve.
        /// </summary>
        /// <param name="t">Parameter from 0 to 1 along the curve.</param>
        /// <param name="p0">Start control point.</param>
        /// <param name="p1">Middle control point.</param>
        /// <param name="p2">End control point.</param>
        /// <returns>Point on the quadratic Bezier curve.</returns>
        public static Vector2 QuadraticBezier2D(float t, Vector2 p0, Vector2 p1, Vector2 p2)
        {
            if (t < 0f || t > 1f)
                throw new ArgumentException("Parameter t must be between 0 and 1");

            float mt = 1f - t;
            float mt2 = mt * mt;
            float t2 = t * t;

            return p0 * mt2 + p1 * (2f * mt * t) + p2 * t2;
        }

        /// <summary>
        /// Computes a 2D point on a cubic Bezier curve.
        /// </summary>
        /// <param name="t">Parameter from 0 to 1 along the curve.</param>
        /// <param name="p0">First control point.</param>
        /// <param name="p1">Second control point.</param>
        /// <param name="p2">Third control point.</param>
        /// <param name="p3">Fourth control point.</param>
        /// <returns>Point on the cubic Bezier curve.</returns>
        public static Vector2 CubicBezier2D(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
        {
            if (t < 0f || t > 1f)
                throw new ArgumentException("Parameter t must be between 0 and 1");

            float mt = 1f - t;
            float mt2 = mt * mt;
            float mt3 = mt2 * mt;
            float t2 = t * t;
            float t3 = t2 * t;

            return p0 * mt3 + p1 * (3f * mt2 * t) + p2 * (3f * mt * t2) + p3 * t3;
        }
    }
}
