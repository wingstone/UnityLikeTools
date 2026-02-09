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
    static void GenerateGrassMesh()
    {
        Console.WriteLine();
        Console.WriteLine("========== GLTF Mesh Generator ==========");

        // Create a grass blade mesh
        Console.WriteLine("Creating grass blade mesh...");
        var singleGrass = MeshLibrary.CreateGrassBlade(segmentCount: 8, width: 0.04f, useUpNormal: false);

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
                RandomUtil.Range(-0.5f, 0.5f),
                RandomUtil.Range(-0.5f, 0.5f),
                0f
            );

            // Random rotation around Z-axis (0-360 degrees)
            float angle = RandomUtil.Range(0f, 360f);
            instanceRotations[i] = Quaternion.AxisAngle(new Vector3(0, 0, 1), angle);

            instanceScales[i] = Vector3.one;
            instanceTransforms[i] = new Transform(instancePoints[i], instanceRotations[i], instanceScales[i]);
        }

        // Merge all instances into a single mesh
        Console.WriteLine("Merging instances into single mesh...");
        var mergedMesh = MeshTools.CopyMeshToTransforms(singleGrass, instanceTransforms);

        // Custom uvs1 for instance placement
        mergedMesh.uvs1 = new Vector2[mergedMesh.vertices!.Length];
        for (int i = 0; i < instanceCount; i++)
        {
            for (int j = 0; j < singleGrass.vertices!.Length; j++)
            {
                int idx = i * singleGrass.vertices!.Length + j;
                float u = instancePoints[i].x;
                float v = instancePoints[i].y;
                mergedMesh.uvs1[idx] = new Vector2(u, v);
            }
        }

        // Custom vertex color for each instance
        mergedMesh.colors = new Color[mergedMesh.vertices!.Length];
        for (int i = 0; i < instanceCount; i++)
        {
            Color instanceColor = RandomUtil.Color(); // Random color for each instance
            for (int j = 0; j < singleGrass.vertices!.Length; j++)
            {
                int idx = i * singleGrass.vertices!.Length + j;
                mergedMesh.colors[idx] = instanceColor;
            }
        }

        // Export to GLTF file using ModelIOTools
        string outputPath = "mergedGrass.gltf";
        Console.WriteLine($"Writing mesh to {outputPath}...");
        // ObjFileHandler.WriteMesh(mergedMesh, outputPath);
        GltfFileHandler.WriteMesh(mergedMesh, outputPath);

        Console.WriteLine();
        Console.WriteLine("========== Success ==========");
        Console.WriteLine($"Exported mesh with {mergedMesh.vertices?.Length ?? 0} vertices and {(mergedMesh.triangles?.Length ?? 0) / 3} triangles");

    }

    static void GenerateBezierCurveMesh()
    {
        Console.WriteLine();
        Console.WriteLine("========== Obj Mesh Generator ==========");

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
    }

    static void Main(string[] args)
    {
        try
        {
            GenerateGrassMesh();

            GenerateBezierCurveMesh();

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
