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

        public Mesh() {
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

        //public static Mesh CreateSingleGrassMesh(int strioCount, float )
        //{
        //    Mesh mesh = new Mesh();

        //    mesh.m

        //    return mesh;
        //}
    }

    internal class Directory<T1, T2>
    {
    }

    class MeshTools
    {
        public static void CopyMeshToPoints(Vector3[] points, Mesh mesh)
        {
            // Implementation for copying mesh to points
            Mesh OutMesh = new Mesh();
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
    internal class MainClass    
    {
        static void Main(string[] args)
        {
            Console.WriteLine("----------ObjLibrary Main test begin------");

            MeshTools.WriteMeshToObj(MeshLibrary.Cube, "Cube.obj");

            Console.WriteLine("----------ObjLibrary Main test end------");
            Console.ReadKey();
        }
    }
}
