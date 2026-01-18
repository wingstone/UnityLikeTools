using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using MathLibrary;

namespace GeometryTools
{
    /// <summary>
    /// Provides utility methods for mesh manipulation and export.
    /// </summary>
    public class MeshTools
    {
        /// <summary>
        /// Creates an instanced copy of a mesh at specified positions with optional rotations.
        /// </summary>
        /// <param name="points">Array of positions to place mesh copies.</param>
        /// <param name="mesh">Base mesh to copy.</param>
        /// <param name="rotations">Optional per-instance rotations. If provided, must match points.Length.</param>
        /// <returns>Combined mesh with all instances.</returns>
        public static Mesh CopyMeshToPoints(Vector3[] points, Mesh mesh, Quaternion[]? rotations = null)
        {
            var outMesh = new Mesh();

            // Validate rotation array if provided
            if (rotations != null && rotations.Length != points.Length)
            {
                throw new ArgumentException("Rotation array length must match points array length if provided.");
            }

            // Allocate output arrays
            if (mesh.vertices != null)
                outMesh.vertices = new Vector3[mesh.vertices.Length * points.Length];
            if (mesh.normals != null)
                outMesh.normals = new Vector3[mesh.normals.Length * points.Length];
            if (mesh.uvs0 != null)
                outMesh.uvs0 = new Vector2[mesh.uvs0.Length * points.Length];
            if (mesh.uvs1 != null)
                outMesh.uvs1 = new Vector2[mesh.uvs1.Length * points.Length];
            if (mesh.triangles != null)
                outMesh.triangles = new int[mesh.triangles.Length * points.Length];

            // Copy mesh data for each instance
            int vertexStride = mesh.vertices?.Length ?? 0;
            
            for (int i = 0; i < points.Length; i++)
            {
                Vector3 position = points[i];
                Quaternion rotation = rotations?[i] ?? Quaternion.identity;
                int vertexOffset = i * vertexStride;

                // Copy and transform vertices
                if (mesh.vertices != null && outMesh.vertices != null)
                {
                    for (int j = 0; j < mesh.vertices.Length; j++)
                    {
                        Vector3 vertex = mesh.vertices[j];
                        if (rotations != null)
                            vertex = rotation.RotateVector(vertex);
                        outMesh.vertices[vertexOffset + j] = vertex + position;
                    }
                }

                // Copy normals (no translation, rotation only)
                if (mesh.normals != null && outMesh.normals != null)
                {
                    for (int j = 0; j < mesh.normals.Length; j++)
                    {
                        Vector3 normal = mesh.normals[j];
                        if (rotations != null)
                            normal = rotation.RotateVector(normal);
                        outMesh.normals[vertexOffset + j] = normal;
                    }
                }

                // Copy UVs (no transformation needed)
                if (mesh.uvs0 != null && outMesh.uvs0 != null)
                {
                    for (int j = 0; j < mesh.uvs0.Length; j++)
                        outMesh.uvs0[vertexOffset + j] = mesh.uvs0[j];
                }

                // Copy UVs1 (no transformation needed)
                if (mesh.uvs1 != null && outMesh.uvs1 != null)
                {
                    for (int j = 0; j < mesh.uvs1.Length; j++)
                        outMesh.uvs1[vertexOffset + j] = mesh.uvs1[j];
                }

                // Copy triangles with offset
                if (mesh.triangles != null && outMesh.triangles != null)
                {
                    for (int j = 0; j < mesh.triangles.Length; j++)
                        outMesh.triangles[i * mesh.triangles.Length + j] = mesh.triangles[j] + vertexOffset;
                }
            }

            return outMesh;
        }

        /// <summary>
        /// Creates a mesh by extruding a profile along a Bezier curve.
        /// </summary>
        /// <param name="curvePoints">Points defining the path curve.</param>
        /// <param name="profileRadius">Radius of the circular profile at each point.</param>
        /// <param name="profileSegments">Number of segments in the circular profile.</param>
        /// <returns>Mesh created by extruding the profile along the curve.</returns>
        public static Mesh CreateExtrusionMesh(Vector3[] curvePoints, float profileRadius, int profileSegments)
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

        /// <summary>
        /// Exports a mesh to OBJ file format.
        /// </summary>
        /// <param name="mesh">Mesh to export.</param>
        /// <param name="filePath">Output file path.</param>
        public static void WriteMeshToObj(Mesh mesh, string filePath)
        {
            if (mesh == null)
                throw new ArgumentNullException(nameof(mesh));

            using (var writer = new StreamWriter(filePath))
            {
                var sb = new StringBuilder();
                writer.WriteLine("# Generated by GeometryTools");
                
                if (mesh != null)
                {
                    // Write vertices
                    if (mesh.vertices != null)
                    {
                        foreach (Vector3 v in mesh.vertices)
                            sb.AppendFormat("v {0} {1} {2}\n", v.x, v.y, v.z);
                        sb.AppendLine();
                    }

                    // Write normals
                    if (mesh.normals != null)
                    {
                        foreach (Vector3 n in mesh.normals)
                            sb.AppendFormat("vn {0} {1} {2}\n", n.x, n.y, n.z);
                        sb.AppendLine();
                    }

                    // Write texture coordinates
                    if (mesh.uvs0 != null)
                    {
                        foreach (Vector2 uv in mesh.uvs0)
                            sb.AppendFormat("vt {0} {1}\n", uv.x, uv.y);
                        sb.AppendLine();
                    }

                    // Write faces
                    if (mesh.triangles != null)
                    {
                        bool hasNormals = mesh.normals != null;
                        bool hasUVs = mesh.uvs0 != null;

                        for (int i = 0; i < mesh.triangles.Length; i += 3)
                        {
                            // OBJ format uses 1-based indices
                            int v0 = mesh.triangles[i] + 1;
                            int v1 = mesh.triangles[i + 1] + 1;
                            int v2 = mesh.triangles[i + 2] + 1;

                            if (hasUVs && hasNormals)
                                sb.AppendFormat("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n", v0, v1, v2);
                            else if (hasUVs)
                                sb.AppendFormat("f {0}/{0} {1}/{1} {2}/{2}\n", v0, v1, v2);
                            else if (hasNormals)
                                sb.AppendFormat("f {0}//{0} {1}//{1} {2}//{2}\n", v0, v1, v2);
                            else
                                sb.AppendFormat("f {0} {1} {2}\n", v0, v1, v2);
                        }
                        sb.AppendLine();
                    }
                }

                writer.Write(sb.ToString());
            }
        }
    }
}
