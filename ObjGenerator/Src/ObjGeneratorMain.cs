using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MathLibrary;
using GeometryTools;

/// <summary>
/// OBJ file generator for creating and exporting mesh geometries.
/// </summary>
class ObjGenerator
{
    /// <summary>
    /// Main entry point for ObjGenerator demonstrations.
    /// </summary>
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("========== OBJ Library Mesh Generator ==========");
                Console.WriteLine();

                // Create a grass blade mesh
                Console.WriteLine("Creating grass blade mesh...");
                var singleGrass = MeshLibrary.CreateGrassBlade(segmentCount: 7, width: 0.04f);

                // Generate random positions for 25 grass instances
                Console.WriteLine("Generating 25 grass instances with random rotation...");
                var instancePoints = new Vector3[25];
                var instanceRotations = new Quaternion[25];

                for (int i = 0; i < 25; i++)
                {
                    instancePoints[i] = new Vector3(
                        RandomUtil.Range(-1f, 1f),
                        RandomUtil.Range(-1f, 1f),
                        0f
                    );
                    
                    // Random rotation around Z-axis (0-360 degrees)
                    float angle = RandomUtil.Range(0f, 360f);
                    instanceRotations[i] = Quaternion.AxisAngle(new Vector3(0, 0, 1), angle);
                }

                // Merge all instances into a single mesh
                Console.WriteLine("Merging instances into single mesh...");
                var mergedMesh = MeshTools.CopyMeshToPoints(instancePoints, singleGrass, instanceRotations);

                // Export to OBJ file
                string outputPath = "mergedGrass.obj";
                Console.WriteLine($"Writing mesh to {outputPath}...");
                MeshTools.WriteMeshToObj(mergedMesh, outputPath);

                Console.WriteLine();
                Console.WriteLine("========== Success ==========");
                Console.WriteLine($"Exported mesh with {mergedMesh.vertices?.Length ?? 0} vertices and {(mergedMesh.triangles?.Length ?? 0) / 3} triangles");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("========== Error ==========");
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine();
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}
