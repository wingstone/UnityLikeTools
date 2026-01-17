using MathLibrary;

namespace GeometryTools
{
    /// <summary>
    /// Represents 3D mesh geometry with vertices, normals, colors, and texture coordinates.
    /// </summary>
    public class Mesh
    {
        public Vector3[]? vertices;
        public Vector3[]? normals;
        public Vector2[]? uvs0;
        public Vector2[]? uvs1;
        public int[]? triangles;

#pragma warning disable CS0649 // Unused fields reserved for future use
        public Vector3[]? tangents;
        public Vector4[]? colors;
#pragma warning restore CS0649
    }
}
