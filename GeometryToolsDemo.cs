using System;
using System.IO;
using GeometryTools;
using MathLibrary;

/// <summary>
/// Demonstrates Torus and Bezier curve functionality in GeometryTools.
/// </summary>
class GeometryToolsDemo
{
    static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("========== GeometryTools Demo ==========");
            Console.WriteLine();

            // Demo 1: Create Torus
            Console.WriteLine("1. Creating Torus meshes...");
            DemoTorus();
            Console.WriteLine("   ✓ Torus meshes created successfully");
            Console.WriteLine();

            // Demo 2: Bezier Curves
            Console.WriteLine("2. Testing Bezier curves...");
            DemoBezierCurves();
            Console.WriteLine("   ✓ Bezier curve calculations completed");
            Console.WriteLine();

            // Demo 3: Bezier Extrusion
            Console.WriteLine("3. Creating Bezier extrusion mesh...");
            DemoBezierExtrusion();
            Console.WriteLine("   ✓ Bezier extrusion mesh created");
            Console.WriteLine();

            Console.WriteLine("========== All Demos Completed Successfully ==========");
            Console.WriteLine("Generated files:");
            Console.WriteLine("  - torus_smooth.obj");
            Console.WriteLine("  - torus_thick.obj");
            Console.WriteLine("  - torus_thin.obj");
            Console.WriteLine("  - bezier_tube.obj");
            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.WriteLine("========== Error ==========");
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }

    static void DemoTorus()
    {
        // Create smooth torus
        var torusSmooth = MeshLibrary.CreateTorus(
            majorRadius: 1.0f,
            minorRadius: 0.3f,
            majorSegments: 64,
            minorSegments: 32
        );
        MeshTools.WriteMeshToObj(torusSmooth, "torus_smooth.obj");
        Console.WriteLine($"   Created smooth torus: {torusSmooth.vertices?.Length ?? 0} vertices");

        // Create thick torus
        var torusThick = MeshLibrary.CreateTorus(
            majorRadius: 1.5f,
            minorRadius: 0.5f,
            majorSegments: 48,
            minorSegments: 24
        );
        MeshTools.WriteMeshToObj(torusThick, "torus_thick.obj");
        Console.WriteLine($"   Created thick torus: {torusThick.vertices?.Length ?? 0} vertices");

        // Create thin torus
        var torusThin = MeshLibrary.CreateTorus(
            majorRadius: 2.0f,
            minorRadius: 0.1f,
            majorSegments: 64,
            minorSegments: 16
        );
        MeshTools.WriteMeshToObj(torusThin, "torus_thin.obj");
        Console.WriteLine($"   Created thin torus: {torusThin.vertices?.Length ?? 0} vertices");
    }

    static void DemoBezierCurves()
    {
        // Test quadratic Bezier
        Vector3 p0 = new Vector3(0, 0, 0);
        Vector3 p1 = new Vector3(1, 2, 0);
        Vector3 p2 = new Vector3(2, 0, 0);

        Vector3 midPoint = BezierCurve.QuadraticBezier(0.5f, p0, p1, p2);
        Console.WriteLine($"   Quadratic Bezier at t=0.5: ({midPoint.x:F2}, {midPoint.y:F2}, {midPoint.z:F2})");

        // Test cubic Bezier
        Vector3 p3 = new Vector3(3, 0, 0);
        Vector3 cubicMid = BezierCurve.CubicBezier(0.5f, p0, p1, p2, p3);
        Console.WriteLine($"   Cubic Bezier at t=0.5: ({cubicMid.x:F2}, {cubicMid.y:F2}, {cubicMid.z:F2})");

        // Generate curve points
        Vector3[] quadraticCurve = BezierCurve.GenerateQuadraticBezierCurve(p0, p1, p2, 20);
        Console.WriteLine($"   Generated quadratic curve with {quadraticCurve.Length} points");

        Vector3[] cubicCurve = BezierCurve.GenerateCubicBezierCurve(p0, p1, p2, p3, 20);
        Console.WriteLine($"   Generated cubic curve with {cubicCurve.Length} points");
    }

    static void DemoBezierExtrusion()
    {
        // Create an S-shaped Bezier curve
        Vector3 start = new Vector3(-2, 0, 0);
        Vector3 ctrl1 = new Vector3(-1, 1.5f, 0);
        Vector3 ctrl2 = new Vector3(1, 1.5f, 0);
        Vector3 end = new Vector3(2, 0, 0);

        Vector3[] curvePoints = BezierCurve.GenerateCubicBezierCurve(
            start, ctrl1, ctrl2, end,
            segmentCount: 50
        );

        // Create extrusion mesh along the curve
        var tubeMesh = BezierCurve.CreateBezierExtrusionMesh(
            curvePoints: curvePoints,
            profileRadius: 0.15f,
            profileSegments: 12
        );

        MeshTools.WriteMeshToObj(tubeMesh, "bezier_tube.obj");
        Console.WriteLine($"   Created Bezier tube mesh: {tubeMesh.vertices?.Length ?? 0} vertices");
        Console.WriteLine($"   Tube triangles: {(tubeMesh.triangles?.Length ?? 0) / 3}");
    }
}
