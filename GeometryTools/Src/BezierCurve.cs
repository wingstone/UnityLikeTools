using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Creates a mesh by extruding a profile along a Bezier curve.
        /// </summary>
        /// <param name="curvePoints">Points defining the path curve.</param>
        /// <param name="profileRadius">Radius of the circular profile at each point.</param>
        /// <param name="profileSegments">Number of segments in the circular profile.</param>
        /// <returns>Mesh created by extruding the profile along the curve.</returns>
        public static Mesh CreateBezierExtrusionMesh(Vector3[] curvePoints, float profileRadius, int profileSegments)
        {
            if (curvePoints == null || curvePoints.Length < 2)
                throw new ArgumentException("curvePoints must have at least 2 points");
            if (profileSegments < 3)
                throw new ArgumentException("profileSegments must be at least 3");

            var mesh = new Mesh();
            var vertices = new List<Vector3>();
            var triangles = new List<int>();

            // Generate vertices for each profile along the curve
            for (int i = 0; i < curvePoints.Length; i++)
            {
                Vector3 curvePoint = curvePoints[i];

                // Calculate tangent direction
                Vector3 tangent = Vector3.zero;
                if (i == 0)
                {
                    tangent = (curvePoints[1] - curvePoints[0]).Normalized;
                }
                else if (i == curvePoints.Length - 1)
                {
                    tangent = (curvePoints[i] - curvePoints[i - 1]).Normalized;
                }
                else
                {
                    tangent = ((curvePoints[i + 1] - curvePoints[i - 1]) * 0.5f).Normalized;
                }

                // Calculate perpendicular vectors for profile
                Vector3 normal = Vector3.zero;
                if (Mathf.Abs(tangent.x) < 0.9f)
                    normal = new Vector3(1, 0, 0);
                else
                    normal = new Vector3(0, 1, 0);

                Vector3 binormal = Vector3.Cross(tangent, normal).Normalized;
                normal = Vector3.Cross(binormal, tangent).Normalized;

                // Generate circular profile vertices
                for (int j = 0; j < profileSegments; j++)
                {
                    float angle = (j / (float)profileSegments) * Mathf.Tau;
                    float x = Mathf.Cos(angle) * profileRadius;
                    float y = Mathf.Sin(angle) * profileRadius;

                    Vector3 vertex = curvePoint + normal * x + binormal * y;
                    vertices.Add(vertex);
                }
            }

            // Generate triangles connecting adjacent profiles
            int profileVertexCount = profileSegments;
            for (int i = 0; i < curvePoints.Length - 1; i++)
            {
                int baseIdx = i * profileVertexCount;
                int nextBaseIdx = (i + 1) * profileVertexCount;

                for (int j = 0; j < profileSegments; j++)
                {
                    int jNext = (j + 1) % profileSegments;

                    // First triangle
                    triangles.Add(baseIdx + j);
                    triangles.Add(nextBaseIdx + j);
                    triangles.Add(baseIdx + jNext);

                    // Second triangle
                    triangles.Add(baseIdx + jNext);
                    triangles.Add(nextBaseIdx + j);
                    triangles.Add(nextBaseIdx + jNext);
                }
            }

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            return mesh;
        }
    }
}
