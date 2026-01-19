using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MathLibrary;
using GeometryTools;
using ModelIOTools;

/// <summary>
/// OBJ file generator for creating and exporting mesh geometries.
/// </summary>
class Demo
{
    static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("========== OBJ Library Mesh Generator ==========");
            Console.WriteLine();

            // Create a grass blade mesh
            Console.WriteLine("Creating grass blade mesh...");
            var singleGrass = MeshLibrary.CreateGrassBlade(segmentCount: 8, width: 0.04f);

            // Generate random positions for 50 grass instances
            var instanceCount = 50;
            Console.WriteLine($"Generating {instanceCount} grass instances with random rotation...");
            var instancePoints = new Vector3[instanceCount];
            var instanceRotations = new Quaternion[instanceCount];
            var instanceScales = new Vector3[instanceCount];
            var instanceTransforms = new Transform[instanceCount];

            for (int i = 0; i < instanceCount; i++)
            {
                instancePoints[i] = new Vector3(
                    RandomUtil.Range(-50f, 50f),
                    RandomUtil.Range(-50f, 50f),
                    0f
                );
                
                // Random rotation around Z-axis (0-360 degrees)
                float angle = RandomUtil.Range(0f, 360f);
                instanceRotations[i] = Quaternion.AxisAngle(new Vector3(0, 0, 1), angle);

                instanceScales[i] = Vector3.one * 100f;
                instanceTransforms[i] = new Transform(instancePoints[i], instanceRotations[i], instanceScales[i]);
            }

            // Merge all instances into a single mesh
            Console.WriteLine("Merging instances into single mesh...");
            var mergedMesh = MeshTools.CopyMeshToTransforms(singleGrass, instanceTransforms);

            // Custom uvs0 for instance placement
            mergedMesh.uvs0 = new Vector2[mergedMesh.vertices!.Length];
            for (int i = 0; i < instanceCount; i++)
            {
                for (int j = 0; j < singleGrass.vertices!.Length; j++)
                {
                    int idx = i * singleGrass.vertices!.Length + j;
                    float u = instancePoints[i].x;
                    float v = instancePoints[i].y;
                    mergedMesh.uvs0[idx] = new Vector2(u, v);
                }
            }

            // Export to USD file using ModelIOTools
            string outputPath = "mergedGrass.usd";
            Console.WriteLine($"Writing mesh to {outputPath}...");
            // ObjFileHandler.WriteMesh(mergedMesh, outputPath);
            UsdFileHandler.WriteMesh(mergedMesh, outputPath);

            Console.WriteLine();
            Console.WriteLine("========== Success ==========");
            Console.WriteLine($"Exported mesh with {mergedMesh.vertices?.Length ?? 0} vertices and {(mergedMesh.triangles?.Length ?? 0) / 3} triangles");

            // Create a Bezier curve mesh
            Console.WriteLine("Creating Bezier curve mesh...");
            var startPoint = new Vector3(0, 0, 0);
            var controlPoint1 = new Vector3(0, 0, 30);
            var controlPoint2 = new Vector3(70, 0, 100);
            var endPoint = new Vector3(100, 0, 100);
            var curve = BezierCurve.GenerateCubicBezierCurve(startPoint, controlPoint1, controlPoint2, endPoint, segmentCount: 7);
            var bezierMesh = MeshTools.CreateExtrusionMesh(curve, profileRadius: 2f, profileSegments: 8);

            // Export to OBJ file using ModelIOTools
            string bezierOutputPath = "bezierCurve.obj";
            Console.WriteLine($"Writing Bezier curve mesh to {bezierOutputPath}...");
            ObjFileHandler.WriteMesh(bezierMesh, bezierOutputPath);

            Console.WriteLine();
            Console.WriteLine("========== Success ==========");
            Console.WriteLine($"Exported Bezier curve mesh with {bezierMesh.vertices?.Length ?? 0} vertices and {(bezierMesh.triangles?.Length ?? 0) / 3} triangles");

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
