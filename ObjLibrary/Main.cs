using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjLibrary
{
    class Vector2
    {
        public float x, y;
        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        // override + operator
        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x + b.x, a.y + b.y);
        }

        // override - operator
        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x - b.x, a.y - b.y);
        }

        // override * operator
        public static Vector2 operator *(Vector2 a, float b)
        {
            return new Vector2(a.x * b, a.y * b);
        }
    }

    class Vector3
    {
        public float x, y, z;
        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        // override + operator
        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        // override - operator
        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        // override * operator
        public static Vector3 operator *(Vector3 a, float b)
        {
            return new Vector3(a.x * b, a.y * b, a.z * b);
        }
    }

    class Vector4
    {
        public float x, y, z, w;
        public Vector4(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        // override + operator
        public static Vector4 operator +(Vector4 a, Vector4 b)
        {
            return new Vector4(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
        }

        // override - operator
        public static Vector4 operator -(Vector4 a, Vector4 b)
        {
            return new Vector4(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
        }

        // override * operator
        public static Vector4 operator *(Vector4 a, float b)
        {
            return new Vector4(a.x * b, a.y * b, a.z * b, a.w * b);
        }
    }

    class Mesh
    {
        public Vector3[] vertices;
        public Vector3[] normals;
        public Vector3[] tangents;
        public Vector4[] colors;
        public Vector2[] uvs0;
        public Vector2[] uvs1;
        public int[] triangles;

        public Mesh()
        {
            vertices = null;
            normals = null;
            tangents = null;
            colors = null;
            uvs0 = null;
            uvs1 = null;
            triangles = null;
        }

        public Mesh(Vector3[] vertices, Vector3[] normals, Vector3[] tangents, Vector4[] colors, Vector2[] uvs0, Vector2[] uvs1, int[] triangles)
        {
            this.vertices = vertices;
            this.normals = normals;
            this.tangents = tangents;
            this.colors = colors;
            this.uvs0 = uvs0;
            this.uvs1 = uvs1;
            this.triangles = triangles;

        }
    }

    class MeshLibrary
    {
        public static Mesh Cube;

        static MeshLibrary()
        {
            Cube = new Mesh();
            Cube.vertices = new Vector3[8] {
                new Vector3(-1, -1, -1),
                new Vector3(1, -1, -1),
                new Vector3(1, 1, -1),
                new Vector3(-1, 1, -1),
                new Vector3(-1, -1, 1),
                new Vector3(1, -1, 1),
                new Vector3(1, 1, 1),
                new Vector3(-1, 1, 1) };

            Dictionary<int, int> ControlPointToVertexId = new Dictionary<int, int>();
            // Face 1
            ControlPointToVertexId.Add(0, 3);
            ControlPointToVertexId.Add(1, 2);
            ControlPointToVertexId.Add(2, 1);
            ControlPointToVertexId.Add(3, 0);
            // Face 2
            ControlPointToVertexId.Add(4, 4);
            ControlPointToVertexId.Add(5, 7);
            ControlPointToVertexId.Add(6, 3);
            ControlPointToVertexId.Add(7, 0);
            // Face 3
            ControlPointToVertexId.Add(8, 0);
            ControlPointToVertexId.Add(9, 1);
            ControlPointToVertexId.Add(10, 5);
            ControlPointToVertexId.Add(11, 4);
            // Face 4
            ControlPointToVertexId.Add(12, 7);
            ControlPointToVertexId.Add(13, 6);
            ControlPointToVertexId.Add(14, 2);
            ControlPointToVertexId.Add(15, 3);
            // Face 5
            ControlPointToVertexId.Add(16, 6);
            ControlPointToVertexId.Add(17, 5);
            ControlPointToVertexId.Add(18, 1);
            ControlPointToVertexId.Add(19, 2);
            // Face 6
            ControlPointToVertexId.Add(20, 4);
            ControlPointToVertexId.Add(21, 5);
            ControlPointToVertexId.Add(22, 6);
            ControlPointToVertexId.Add(23, 7);


            Cube.triangles = new int[36] {
                0,1,2,0,2,3,
                4,5,6,4,6,7,
                8,9,10,8,10,11,
                12,13,14,12,14,15,
                16,17,18,16,18,19,
                20,21,22,20,22,23
            };

            for (int i = 0; i < Cube.triangles.Length; i++)
            {
                Cube.triangles[i] = ControlPointToVertexId[Cube.triangles[i]];
            }
        }

        public static Mesh CreateSingleGrassMesh(int segmentCount, float width)
        {
            Mesh mesh = new Mesh();
            float segmentLength = segmentCount > 0 ? 1.0f / segmentCount : 0;
            float halfWidth = 0.5f * width;

            // vertices
            mesh.vertices = new Vector3[segmentCount * 2 + 1];
            for (int i = 0; i < segmentCount; i++)
            {
                mesh.vertices[i * 2] = new Vector3(-halfWidth, 0, segmentLength * i);
                mesh.vertices[i * 2 + 1] = new Vector3(halfWidth, 0, segmentLength * i);
            }
            mesh.vertices[segmentCount * 2] = new Vector3(0, 0, 1.0f);

            // indices
            mesh.triangles = new int[(((segmentCount - 1) * 2) + 1) * 3];
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
    }

    class MeshTools
    {
        public static Mesh CopyMeshToPoints(Vector3[] points, Mesh mesh)
        {
            Mesh outMesh = new Mesh();
            // Implementation for copying mesh to points
            if (mesh.vertices != null)
            {
                outMesh.vertices = new Vector3[mesh.vertices.Length * points.Length];
            }
            if (mesh.normals != null)
            {
                outMesh.normals = new Vector3[mesh.normals.Length * points.Length];
            }
            if (mesh.uvs0 != null)
            {
                outMesh.uvs0 = new Vector2[mesh.uvs0.Length * points.Length];
            }
            if (mesh.uvs1 != null)
            {
                outMesh.uvs1 = new Vector2[mesh.uvs1.Length * points.Length];
            }
            if (mesh.triangles != null)
            {
                outMesh.triangles = new int[mesh.triangles.Length * points.Length];
            }
            for (int i = 0; i < points.Length; i++)
            {
                // Copy vertices
                if (mesh.vertices != null)
                {
                    for (int j = 0; j < mesh.vertices.Length; j++)
                    {
                        outMesh.vertices[i * mesh.vertices.Length + j] = mesh.vertices[j] + points[i];
                    }
                }

                // Copy normals
                if (mesh.normals != null)
                {
                    for (int j = 0; j < mesh.normals.Length; j++)
                    {
                        outMesh.normals[i * mesh.normals.Length + j] = mesh.normals[j];
                    }
                }

                // Copy UVs
                if (mesh.uvs0 != null)
                {
                    for (int j = 0; j < mesh.uvs0.Length; j++)
                    {
                        outMesh.uvs0[i * mesh.uvs0.Length + j] = mesh.uvs0[j];
                    }
                }

                // Copy UVs1
                if (mesh.uvs1 != null)
                {
                    for (int j = 0; j < mesh.uvs1.Length; j++)
                    {
                        outMesh.uvs1[i * mesh.uvs1.Length + j] = mesh.uvs1[j];
                    }
                }

                // Copy triangles
                if (mesh.triangles != null)
                {
                    for (int j = 0; j < mesh.triangles.Length; j++)
                    {
                        outMesh.triangles[i * mesh.triangles.Length + j] = mesh.triangles[j] + i * mesh.vertices.Length;
                    }
                }
            }

            return outMesh;
        }

        public static void WriteMeshToObj(Mesh mesh, string filePath)
        {
            int vertexOffset = 0;
            // Implementation for writing mesh to OBJ file
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                StringBuilder sb = new StringBuilder();
                if (mesh != null)
                {
                    int formatCount = 0;
                    if (mesh.vertices != null)
                    {
                        foreach (Vector3 vertex in mesh.vertices)
                        {
                            sb.AppendFormat("v {0} {1} {2}\n", vertex.x, vertex.y, vertex.z);
                        }
                        sb.AppendLine();
                        formatCount++;
                    }

                    if (mesh.normals != null)
                    {
                        foreach (Vector3 normal in mesh.normals)
                        {
                            sb.AppendFormat("vn {0} {1} {2}\n", normal.x, normal.y, normal.z);
                        }
                        sb.AppendLine();
                        formatCount++;
                    }

                    //if (mesh.tangents != null)
                    //{
                    //    foreach (Vector3 tangent in mesh.tangents)
                    //    {
                    //        sb.AppendFormat("vtan {0} {1} {2}\n", tangent.x, tangent.y, tangent.z);
                    //    }
                    //    sb.AppendLine();
                    //formatCount++;
                    //}

                    if (mesh.uvs0 != null)
                    {
                        foreach (Vector2 uv in mesh.uvs0)
                        {
                            sb.AppendFormat("vt {0} {1}\n", uv.x, uv.y);
                        }
                        sb.AppendLine();
                        formatCount++;
                    }

                    if (mesh.triangles != null)
                    {
                        string formatString = "";
                        string formatPrefix = "f ";
                        string fornatIndex0Str = "";
                        string fornatIndex1Str = "";
                        string fornatIndex2Str = "";
                        for (int i = 0; i < formatCount; i++)
                        {
                            fornatIndex0Str += (i == 0 ? "" : "/") + "{" + 0 + "}";
                        }
                        for (int i = 0; i < formatCount; i++)
                        {
                            fornatIndex1Str += (i == 0 ? "" : "/") + "{" + 1 + "}";
                        }
                        for (int i = 0; i < formatCount; i++)
                        {
                            fornatIndex2Str += (i == 0 ? "" : "/") + "{" + 2 + "}";
                        }
                        formatString = formatPrefix + fornatIndex0Str + " " + fornatIndex1Str + " " + fornatIndex2Str;

                        for (int i = 0; i < mesh.triangles.Length; i += 3)
                        {
                            int[] triangles = mesh.triangles;
                            int index0 = triangles[i] + 1 + vertexOffset;
                            int index1 = triangles[i + 1] + 1 + vertexOffset;
                            int index2 = triangles[i + 2] + 1 + vertexOffset;
                            sb.AppendFormat(formatString,
                                index0,
                                index1,
                                index2);
                            sb.AppendLine();
                        }
                        sb.AppendLine();
                    }
                }

                writer.WriteLine("# whtie by ObjLibrary");
                sb.AppendLine();
                writer.Write(sb.ToString());
            }
        }
    }

    class RandomTools
    {
        static Random random = new Random();

        public static float RandomFloat(float min, float max)
        {
            return (float)random.NextDouble() * (max - min) + min;
        }

        public static Vector2 RandomVector2(Vector2 min, Vector2 max)
        {
            return new Vector2(
                RandomFloat(min.x, max.x),
                RandomFloat(min.y, max.y)
            );
        }

        public static Vector3 RandomVector3(Vector3 min, Vector3 max)
        {
            return new Vector3(
                RandomFloat(min.x, max.x),
                RandomFloat(min.y, max.y),
                RandomFloat(min.z, max.z)
            );
        }
    }

    internal class MainClass
    {
        static void Main(string[] args)
        {
            Console.WriteLine("----------ObjLibrary Main test begin------");
            // MeshTools.WriteMeshToObj(MeshLibrary.Cube, "Cube.obj");

            Mesh singleGrass = MeshLibrary.CreateSingleGrassMesh(7, 0.04f);
            // MeshTools.WriteMeshToObj(singleGrass, "singleGrass.obj");

            Vector3[] instancePoints = new Vector3[10];
            for (int i = 0; i < instancePoints.Length; i++)
            {
                instancePoints[i] = RandomTools.RandomVector3(new Vector3(-1, -1, 0), new Vector3(1, 1, 0));
            }
            Mesh mergedMesh = MeshTools.CopyMeshToPoints(instancePoints, singleGrass);
            MeshTools.WriteMeshToObj(mergedMesh, "mergedGrass.obj");

            Console.WriteLine("----------ObjLibrary Main test end------");
            Console.ReadKey();
        }
    }
}
