using System;
using System.Collections.Generic;
using MathLibrary;

// Mesh coordinate is left-handed, Z-up, Y-forward

namespace GeometryTools
{
    /// <summary>
    /// Provides factory methods for creating standard geometric meshes.
    /// </summary>
    public class MeshLibrary
    {
        /// <summary>
        /// Creates a single grass blade mesh with specified segment count and width.
        /// </summary>
        /// <param name="segmentCount">Number of segments along the blade length.</param>
        /// <param name="width">Width of the grass blade.</param>
        public static Mesh CreateGrassBlade(int segmentCount, float width, bool useUpNormal = false)
        {
            if (segmentCount < 1 || width <= 0)
                throw new ArgumentException("segmentCount must be >= 1 and width must be > 0");

            var mesh = new Mesh();
            float segmentLength = 1.0f / segmentCount;
            float halfWidth = width * 0.5f;

            // Create blade vertices (two rows for width)
            int vertexCount = segmentCount * 2 + 1;
            mesh.vertices = new Vector3[vertexCount];
            mesh.uvs0 = new Vector2[vertexCount];
            
            for (int i = 0; i < segmentCount; i++)
            {
                float zPos = segmentLength * i;
                mesh.vertices[i * 2] = new Vector3(-halfWidth, 0, zPos);
                mesh.vertices[i * 2 + 1] = new Vector3(halfWidth, 0, zPos);

                mesh.uvs0[i * 2] = new Vector2(0.5f - halfWidth, zPos);
                mesh.uvs0[i * 2 + 1] = new Vector2(0.5f + halfWidth, zPos);
            }
            mesh.vertices[vertexCount - 1] = new Vector3(0, 0, 1.0f);
            mesh.uvs0[vertexCount - 1] = new Vector2(0.5f, 1.0f);

            // Create normals
            mesh.normals = new Vector3[vertexCount];
            for (int i = 0; i < vertexCount; i++)
            {
                if (useUpNormal)
                    mesh.normals[i] = new Vector3(0, 0, 1);
                else
                    mesh.normals[i] = new Vector3(0, 1, 0);
            }

            // Create triangles
            int triangleCount = ((segmentCount - 1) * 2 + 1) * 3;
            mesh.triangles = new int[triangleCount];
            for (int i = 0; i < segmentCount - 1; i++)
            {
                mesh.triangles[i * 2 * 3] = i * 2;
                mesh.triangles[i * 2 * 3 + 1] = i * 2 + 1;
                mesh.triangles[i * 2 * 3 + 2] = (i + 1) * 2;

                mesh.triangles[(i * 2 + 1) * 3] = i * 2 + 1;
                mesh.triangles[(i * 2 + 1) * 3 + 1] = (i + 1) * 2 + 1;
                mesh.triangles[(i * 2 + 1) * 3 + 2] = (i + 1) * 2;
            }
            mesh.triangles[(segmentCount - 1) * 2 * 3] = (segmentCount - 1) * 2;
            mesh.triangles[(segmentCount - 1) * 2 * 3 + 1] = (segmentCount - 1) * 2 + 1;
            mesh.triangles[(segmentCount - 1) * 2 * 3 + 2] = segmentCount * 2;

            return mesh;
        }

        /// <summary>
        /// Creates a UV sphere mesh with specified radius and segment count.
        /// </summary>
        /// <param name="radius">Radius of the sphere.</param>
        /// <param name="widthSegments">Number of horizontal segments (longitude).</param>
        /// <param name="heightSegments">Number of vertical segments (latitude).</param>
        public static Mesh CreateSphere(float radius = 1f, int widthSegments = 32, int heightSegments = 16)
        {
            Mesh mesh = new Mesh();
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();

            // Generate sphere vertices using spherical coordinates
            for (int y = 0; y <= heightSegments; y++)
            {
                float vPercent = y / (float)heightSegments;
                float vRad = Mathf.PI * vPercent;
                float sinV = Mathf.Sin(vRad);
                float cosV = Mathf.Cos(vRad);

                for (int x = 0; x <= widthSegments; x++)
                {
                    float uPercent = x / (float)widthSegments;
                    float uRad = Mathf.Tau * uPercent;

                    float xPos = radius * sinV * Mathf.Cos(uRad);
                    float yPos = radius * cosV;
                    float zPos = radius * sinV * Mathf.Sin(uRad);

                    vertices.Add(new Vector3(xPos, yPos, zPos));
                }
            }

            // Generate triangle indices for sphere faces
            for (int y = 0; y < heightSegments; y++)
            {
                for (int x = 0; x < widthSegments; x++)
                {
                    int a = y * (widthSegments + 1) + x;
                    int b = a + 1;
                    int c = a + (widthSegments + 1);
                    int d = c + 1;

                    triangles.Add(a);
                    triangles.Add(c);
                    triangles.Add(b);

                    triangles.Add(b);
                    triangles.Add(c);
                    triangles.Add(d);
                }
            }

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            return mesh;
        }

        /// <summary>
        /// Creates a cylindrical mesh with specified radius, height and segment count.
        /// </summary>
        public static Mesh CreateCylinder(float radius = 1f, float height = 2f, int segments = 32)
        {
            Mesh mesh = new Mesh();
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();

            float halfHeight = height * 0.5f;

            // Add top circle vertices
            for (int i = 0; i <= segments; i++)
            {
                float angle = (i / (float)segments) * Mathf.Tau;
                vertices.Add(new Vector3(radius * Mathf.Cos(angle), halfHeight, radius * Mathf.Sin(angle)));
            }

            // Add bottom circle vertices
            for (int i = 0; i <= segments; i++)
            {
                float angle = (i / (float)segments) * Mathf.Tau;
                vertices.Add(new Vector3(radius * Mathf.Cos(angle), -halfHeight, radius * Mathf.Sin(angle)));
            }

            // Add center vertices for caps
            int topCenterIdx = vertices.Count;
            vertices.Add(new Vector3(0, halfHeight, 0));

            int bottomCenterIdx = vertices.Count;
            vertices.Add(new Vector3(0, -halfHeight, 0));

            // Generate side faces
            for (int i = 0; i < segments; i++)
            {
                int topA = i;
                int topB = i + 1;
                int botA = segments + 1 + i;
                int botB = segments + 1 + i + 1;

                // Two triangles per quad
                triangles.Add(topA);
                triangles.Add(botA);
                triangles.Add(topB);

                triangles.Add(topB);
                triangles.Add(botA);
                triangles.Add(botB);
            }

            // Generate top cap
            for (int i = 0; i < segments; i++)
            {
                triangles.Add(topCenterIdx);
                triangles.Add(i + 1);
                triangles.Add(i);
            }

            // Generate bottom cap
            for (int i = 0; i < segments; i++)
            {
                triangles.Add(bottomCenterIdx);
                triangles.Add(segments + 1 + i);
                triangles.Add(segments + 1 + i + 1);
            }

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            return mesh;
        }

        /// <summary>
        /// Creates a conical mesh with specified radius, height and segment count.
        /// </summary>
        public static Mesh CreateCone(float radius = 1f, float height = 2f, int segments = 32)
        {
            Mesh mesh = new Mesh();
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();

            float halfHeight = height * 0.5f;

            // Add apex vertex
            int topIdx = vertices.Count;
            vertices.Add(new Vector3(0, halfHeight, 0));

            // Add base circle vertices
            for (int i = 0; i <= segments; i++)
            {
                float angle = (i / (float)segments) * Mathf.Tau;
                vertices.Add(new Vector3(radius * Mathf.Cos(angle), -halfHeight, radius * Mathf.Sin(angle)));
            }

            // Add base center vertex
            int bottomCenterIdx = vertices.Count;
            vertices.Add(new Vector3(0, -halfHeight, 0));

            // Generate side faces
            for (int i = 0; i < segments; i++)
            {
                int botA = 1 + i;
                int botB = 1 + i + 1;

                triangles.Add(topIdx);
                triangles.Add(botB);
                triangles.Add(botA);
            }

            // Generate bottom cap
            for (int i = 0; i < segments; i++)
            {
                triangles.Add(bottomCenterIdx);
                triangles.Add(1 + i);
                triangles.Add(1 + i + 1);
            }

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            return mesh;
        }

        /// <summary>
        /// Creates a planar mesh with specified dimensions and segment count.
        /// </summary>
        public static Mesh CreatePlane(float width = 2f, float height = 2f, int widthSegments = 1, int heightSegments = 1)
        {
            Mesh mesh = new Mesh();
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();

            float halfWidth = width * 0.5f;
            float halfHeight = height * 0.5f;

            // Generate vertices
            for (int y = 0; y <= heightSegments; y++)
            {
                float yPercent = y / (float)heightSegments;
                float yPos = halfHeight - yPercent * height;

                for (int x = 0; x <= widthSegments; x++)
                {
                    float xPercent = x / (float)widthSegments;
                    float xPos = -halfWidth + xPercent * width;

                    vertices.Add(new Vector3(xPos, 0, yPos));
                }
            }

            // Generate triangle indices
            for (int y = 0; y < heightSegments; y++)
            {
                for (int x = 0; x < widthSegments; x++)
                {
                    int a = y * (widthSegments + 1) + x;
                    int b = a + 1;
                    int c = a + (widthSegments + 1);
                    int d = c + 1;

                    triangles.Add(a);
                    triangles.Add(c);
                    triangles.Add(b);

                    triangles.Add(b);
                    triangles.Add(c);
                    triangles.Add(d);
                }
            }

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            return mesh;
        }

        /// <summary>
        /// Creates a square pyramid mesh with specified base size and height.
        /// </summary>
        public static Mesh CreatePyramid(float baseSize = 2f, float height = 2f)
        {
            var mesh = new Mesh();
            float halfSize = baseSize * 0.5f;
            float halfHeight = height * 0.5f;

            // Create vertices: 4 base corners + 1 apex
            var vertices = new Vector3[5]
            {
                new Vector3(-halfSize, -halfHeight, -halfSize),  // Base corner 0
                new Vector3(halfSize, -halfHeight, -halfSize),   // Base corner 1
                new Vector3(halfSize, -halfHeight, halfSize),    // Base corner 2
                new Vector3(-halfSize, -halfHeight, halfSize),   // Base corner 3
                new Vector3(0, halfHeight, 0)                    // Apex
            };

            // Create triangles: 2 for base + 4 sides = 18 indices (6 triangles)
            var triangles = new int[18]
            {
                // Base (bottom)
                0, 2, 1,
                0, 3, 2,
                
                // Side faces (apex connected to base edges)
                0, 4, 1,  // Front
                1, 4, 2,  // Right
                2, 4, 3,  // Back
                3, 4, 0   // Left
            };

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            return mesh;
        }

        /// <summary>
        /// Creates a cube mesh with specified size.
        /// Note: Uses 24 vertices (6 per face) to support per-face normals and UV mapping.
        /// </summary>
        public static Mesh CreateCube(float size = 2f)
        {
            var mesh = new Mesh();
            float hs = size * 0.5f;  // halfSize

            // Generate 24 vertices (6 per face for independent UV/normal mapping)
            var vertices = new Vector3[24]
            {
                // Front face (Z+)
                new Vector3(-hs, -hs, hs), new Vector3(hs, -hs, hs), new Vector3(hs, hs, hs), new Vector3(-hs, hs, hs),
                // Back face (Z-)
                new Vector3(-hs, -hs, -hs), new Vector3(-hs, hs, -hs), new Vector3(hs, hs, -hs), new Vector3(hs, -hs, -hs),
                // Top face (Y+)
                new Vector3(-hs, hs, -hs), new Vector3(-hs, hs, hs), new Vector3(hs, hs, hs), new Vector3(hs, hs, -hs),
                // Bottom face (Y-)
                new Vector3(-hs, -hs, -hs), new Vector3(hs, -hs, -hs), new Vector3(hs, -hs, hs), new Vector3(-hs, -hs, hs),
                // Right face (X+)
                new Vector3(hs, -hs, -hs), new Vector3(hs, hs, -hs), new Vector3(hs, hs, hs), new Vector3(hs, -hs, hs),
                // Left face (X-)
                new Vector3(-hs, -hs, -hs), new Vector3(-hs, -hs, hs), new Vector3(-hs, hs, hs), new Vector3(-hs, hs, -hs)
            };

            // Generate 36 triangle indices (6 faces * 6 indices per face)
            var triangles = new int[36]
            {
                // Front
                0, 2, 1, 0, 3, 2,
                // Back
                4, 6, 5, 4, 7, 6,
                // Top
                8, 10, 9, 8, 11, 10,
                // Bottom
                12, 14, 13, 12, 15, 14,
                // Right
                16, 18, 17, 16, 19, 18,
                // Left
                20, 22, 21, 20, 23, 22
            };

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            return mesh;
        }

        /// <summary>
        /// Creates a torus (donut) mesh with specified major and minor radii.
        /// </summary>
        /// <param name="majorRadius">Radius of the main ring (center to center of the tube).</param>
        /// <param name="minorRadius">Radius of the tube (thickness).</param>
        /// <param name="majorSegments">Number of segments around the major ring.</param>
        /// <param name="minorSegments">Number of segments around the tube.</param>
        public static Mesh CreateTorus(float majorRadius = 1f, float minorRadius = 0.3f, int majorSegments = 32, int minorSegments = 16)
        {
            if (majorRadius <= 0 || minorRadius <= 0)
                throw new ArgumentException("Radii must be positive");
            if (majorSegments < 3 || minorSegments < 3)
                throw new ArgumentException("Segment counts must be at least 3");

            var mesh = new Mesh();
            var vertices = new List<Vector3>();
            var triangles = new List<int>();

            // Generate torus vertices
            for (int i = 0; i < majorSegments; i++)
            {
                float majorAngle = (i / (float)majorSegments) * Mathf.Tau;
                float majorCos = Mathf.Cos(majorAngle);
                float majorSin = Mathf.Sin(majorAngle);

                for (int j = 0; j < minorSegments; j++)
                {
                    float minorAngle = (j / (float)minorSegments) * Mathf.Tau;
                    float minorCos = Mathf.Cos(minorAngle);
                    float minorSin = Mathf.Sin(minorAngle);

                    // Position on the major ring
                    float x = (majorRadius + minorRadius * minorCos) * majorCos;
                    float y = minorRadius * minorSin;
                    float z = (majorRadius + minorRadius * minorCos) * majorSin;

                    vertices.Add(new Vector3(x, y, z));
                }
            }

            // Generate torus triangles
            for (int i = 0; i < majorSegments; i++)
            {
                int nextMajor = (i + 1) % majorSegments;

                for (int j = 0; j < minorSegments; j++)
                {
                    int nextMinor = (j + 1) % minorSegments;

                    int idx0 = i * minorSegments + j;
                    int idx1 = i * minorSegments + nextMinor;
                    int idx2 = nextMajor * minorSegments + j;
                    int idx3 = nextMajor * minorSegments + nextMinor;

                    // First triangle
                    triangles.Add(idx0);
                    triangles.Add(idx2);
                    triangles.Add(idx1);

                    // Second triangle
                    triangles.Add(idx1);
                    triangles.Add(idx2);
                    triangles.Add(idx3);
                }
            }

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            return mesh;
        }
    }
}
