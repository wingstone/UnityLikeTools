using System;
using System.Collections.Generic;
using System.Linq;
using MathLibrary;
using GeometryTools;
using SharpGLTF.Schema2;
using SharpGLTF.Geometry;
using SharpGLTF.Geometry.VertexTypes;
using System.Runtime.CompilerServices;
using SharpGLTF.Memory;

namespace ModelIOTools
{
    public static class GltfFileHandlerExtensions
    {
        /// <summary>
        /// Adds a UVS accessor to the mesh primitive.
        /// </summary>
        /// <param name="primitive">The mesh primitive to modify.</param>
        /// <param name="attribute">The attribute name (e.g., "TEXCOORD_0").</param>
        /// <param name="values">The UV coordinate values.</param>
        /// <returns>The modified mesh primitive.</returns> 
        public static MeshPrimitive WithUVSsAccessor(this MeshPrimitive primitive, string attribute, IReadOnlyList<System.Numerics.Vector2> values)
        {
            var root = primitive.LogicalParent.LogicalParent;

            // create an index buffer and fill it
            var byteSize = Unsafe.SizeOf<System.Numerics.Vector2>();
            var view = root.CreateBufferView(byteSize * values.Count, 0, BufferMode.ARRAY_BUFFER);
            var array = new Vector2Array(view.Content);
            array.Fill(values);

            var accessor = root.CreateAccessor();
            accessor.SetVertexData(view, 0, values.Count, new AttributeFormat(DimensionType.VEC2, EncodingType.FLOAT, false));
            primitive.SetVertexAccessor(attribute, accessor);
            
            return primitive;
        }
    }

    /// <summary>
    /// Handles reading and writing glTF 2.0 file format for mesh data.
    /// Uses SharpGLTF library for glTF format handling.
    /// </summary>
    public class GltfFileHandler
    {


        /// <summary>
        /// Exports a mesh to glTF 2.0 file format (.gltf or .glb).
        /// </summary>
        /// <param name="mesh">Mesh to export.</param>
        /// <param name="filePath">Output file path (.gltf for JSON or .glb for binary).</param>
        public static void WriteMesh(GeometryTools.Mesh mesh, string filePath)
        {
            if (mesh == null)
                throw new ArgumentNullException(nameof(mesh));

            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));

            try
            {
                // Create a glTF model
                var model = ModelRoot.CreateModel();

                // Create a scene
                var scene = model.UseScene(0);
                scene.Name = "Default";

                // Create a node for the mesh
                var node = scene.CreateNode("MeshNode");

                // Create mesh and primitives
                var glMesh = CreateGltfMesh(mesh, model, "Mesh");
                node.Mesh = glMesh;

                // Save the model
                bool isBinary = filePath.EndsWith(".glb", StringComparison.OrdinalIgnoreCase);
                if (isBinary)
                {
                    model.SaveGLB(filePath);
                }
                else
                {
                    model.SaveGLTF(filePath);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to write glTF file: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Converts a GeometryTools.Mesh into a glTF mesh inside the provided model.
        /// </summary>
        /// <param name="mesh">Source mesh to convert.</param>
        /// <param name="model">Target glTF model that will own the mesh.</param>
        /// <param name="meshName">Optional mesh name.</param>
        /// <returns>Created glTF mesh instance.</returns>
        public static SharpGLTF.Schema2.Mesh CreateGltfMesh(GeometryTools.Mesh mesh, ModelRoot model, string meshName = "Mesh")
        {
            if (mesh == null)
                throw new ArgumentNullException(nameof(mesh));
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            if (mesh.vertices == null || mesh.vertices.Length == 0)
                throw new InvalidOperationException("Mesh has no vertices to export.");

            var glMesh = model.CreateMesh(meshName);

            // Convert vertices to System.Numerics.Vector3
            var vertices = new System.Numerics.Vector3[mesh.vertices.Length];
            for (int i = 0; i < mesh.vertices.Length; i++)
            {
                vertices[i] = new System.Numerics.Vector3(
                    mesh.vertices[i].x,
                    mesh.vertices[i].y,
                    mesh.vertices[i].z
                );
            }

            // Convert normals if available
            System.Numerics.Vector3[]? normals = null;
            if (mesh.normals != null && mesh.normals.Length > 0)
            {
                normals = new System.Numerics.Vector3[mesh.normals.Length];
                for (int i = 0; i < mesh.normals.Length; i++)
                {
                    normals[i] = new System.Numerics.Vector3(
                        mesh.normals[i].x,
                        mesh.normals[i].y,
                        mesh.normals[i].z
                    );
                }
            }

            // Convert UV coordinates if available
            System.Numerics.Vector2[]? uvs = null;
            if (mesh.uvs0 != null && mesh.uvs0.Length > 0)
            {
                uvs = new System.Numerics.Vector2[mesh.uvs0.Length];
                for (int i = 0; i < mesh.uvs0.Length; i++)
                {
                    uvs[i] = new System.Numerics.Vector2(
                        mesh.uvs0[i].x,
                        mesh.uvs0[i].y
                    );
                }
            }

            // Convert int array to uint array
            int[]? indices = null;
            if (mesh.triangles != null && mesh.triangles.Length > 0)
            {
                indices = new int[mesh.triangles.Length];
                for (int i = 0; i < mesh.triangles.Length; i++)
                {
                    indices[i] = (int)mesh.triangles[i];
                }
            }
            // var builder = new MeshBuilder<VertexPosition

            // Create primitive with vertex and index data
            var primitive = glMesh.CreatePrimitive();
            primitive.WithVertexAccessor("POSITION", vertices);
            primitive.WithIndicesAccessor(PrimitiveType.TRIANGLES, indices);

            if (normals != null)
                primitive.WithVertexAccessor("NORMAL", normals);

            if (uvs != null)
                primitive.WithUVSsAccessor("TEXCOORD_0", uvs);

            return glMesh;
        }

        /// <summary>
        /// Reads a mesh from glTF 2.0 file format.
        /// </summary>
        /// <param name="filePath">Path to the glTF or glb file.</param>
        /// <returns>Mesh loaded from the file.</returns>
        public static GeometryTools.Mesh ReadMesh(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));

            try
            {
                // Load the glTF model
                var model = ModelRoot.Load(filePath);

                // Extract mesh from the first mesh
                var mesh = new GeometryTools.Mesh();

                if (model.LogicalMeshes.Count > 0)
                {
                    var logicalMesh = model.LogicalMeshes[0];
                    
                    if (logicalMesh.Primitives.Count > 0)
                    {
                        var primitive = logicalMesh.Primitives[0];

                        // Extract vertices
                        var positionAccessor = primitive.GetVertexAccessor("POSITION");
                        if (positionAccessor != null)
                        {
                            var positions = positionAccessor.AsVector3Array();
                            mesh.vertices = ConvertVector3Array(positions);
                        }

                        // Extract normals
                        var normalAccessor = primitive.GetVertexAccessor("NORMAL");
                        if (normalAccessor != null)
                        {
                            var normals = normalAccessor.AsVector3Array();
                            mesh.normals = ConvertVector3Array(normals);
                        }

                        // Extract UV coordinates (layer 0)
                        var texCoordAccessor0 = primitive.GetVertexAccessor("TEXCOORD_0");
                        if (texCoordAccessor0 != null)
                        {
                            var uvs = texCoordAccessor0.AsVector2Array();
                            mesh.uvs0 = ConvertVector2Array(uvs);
                        }

                        // Extract UV coordinates (layer 1)
                        var texCoordAccessor1 = primitive.GetVertexAccessor("TEXCOORD_1");
                        if (texCoordAccessor1 != null)
                        {
                            var uvs = texCoordAccessor1.AsVector2Array();
                            mesh.uvs1 = ConvertVector2Array(uvs);
                        }

                        // Extract triangle indices
                        var indexAccessor = primitive.IndexAccessor;
                        if (indexAccessor != null)
                        {
                            var indices = indexAccessor.AsIndicesArray();
                            mesh.triangles = new int[indices.Count];
                            for (int i = 0; i < indices.Count; i++)
                            {
                                mesh.triangles[i] = (int)indices[i];
                            }
                        }
                    }
                }

                return mesh;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to read glTF file: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Reads all meshes from a glTF file.
        /// </summary>
        /// <param name="filePath">Path to the glTF or glb file.</param>
        /// <returns>Array of meshes loaded from the file.</returns>
        public static GeometryTools.Mesh[] ReadAllMeshes(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));

            try
            {
                var model = ModelRoot.Load(filePath);
                var meshes = new List<GeometryTools.Mesh>();

                foreach (var logicalMesh in model.LogicalMeshes)
                {
                    var mesh = ExtractMesh(logicalMesh);
                    if (mesh != null)
                    {
                        meshes.Add(mesh);
                    }
                }

                return meshes.ToArray();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to read glTF file: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Converts System.Numerics.Vector3 array to MathLibrary.Vector3 array.
        /// </summary>
        private static Vector3[] ConvertVector3Array(IEnumerable<System.Numerics.Vector3> source)
        {
            return source.Select(v => new Vector3(v.X, v.Y, v.Z)).ToArray();
        }

        /// <summary>
        /// Converts System.Numerics.Vector2 array to MathLibrary.Vector2 array.
        /// </summary>
        private static Vector2[] ConvertVector2Array(IEnumerable<System.Numerics.Vector2> source)
        {
            return source.Select(v => new Vector2(v.X, v.Y)).ToArray();
        }

        /// <summary>
        /// Extracts a mesh from a glTF logical mesh.
        /// </summary>
        private static GeometryTools.Mesh? ExtractMesh(SharpGLTF.Schema2.Mesh logicalMesh)
        {
            if (logicalMesh.Primitives.Count == 0)
                return null;

            var mesh = new GeometryTools.Mesh();
            var primitive = logicalMesh.Primitives[0];

            // Extract vertices
            var positionAccessor = primitive.GetVertexAccessor("POSITION");
            if (positionAccessor != null)
            {
                var positions = positionAccessor.AsVector3Array();
                mesh.vertices = ConvertVector3Array(positions);
            }

            // Extract normals
            var normalAccessor = primitive.GetVertexAccessor("NORMAL");
            if (normalAccessor != null)
            {
                var normals = normalAccessor.AsVector3Array();
                mesh.normals = ConvertVector3Array(normals);
            }

            // Extract UV coordinates (layer 0)
            var texCoordAccessor0 = primitive.GetVertexAccessor("TEXCOORD_0");
            if (texCoordAccessor0 != null)
            {
                var uvs = texCoordAccessor0.AsVector2Array();
                mesh.uvs0 = ConvertVector2Array(uvs);
            }

            // Extract UV coordinates (layer 1)
            var texCoordAccessor1 = primitive.GetVertexAccessor("TEXCOORD_1");
            if (texCoordAccessor1 != null)
            {
                var uvs = texCoordAccessor1.AsVector2Array();
                mesh.uvs1 = ConvertVector2Array(uvs);
            }

            // Extract triangle indices
            var indexAccessor = primitive.IndexAccessor;
            if (indexAccessor != null)
            {
                var indices = indexAccessor.AsIndicesArray();
                mesh.triangles = new int[indices.Count];
                for (int i = 0; i < indices.Count; i++)
                {
                    mesh.triangles[i] = (int)indices[i];
                }
            }

            return mesh;
        }
    }
}
