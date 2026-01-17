# GeometryTools 库使用指南

GeometryTools 是一个全功能的3D几何生成和网格处理库，提供了丰富的工具来创建和操作3D网格数据。

## 项目结构

```
GeometryTools/
├── Src/
│   ├── Mesh.cs              # 网格数据结构定义
│   ├── MeshLibrary.cs       # 几何体生成工厂类
│   ├── MeshTools.cs         # 网格工具和导出函数
│   └── BezierCurve.cs       # 贝塞尔曲线相关功能
└── GeometryTools.csproj     # 项目配置
```

## 核心类说明

### 1. Mesh 类
表示3D网格几何数据的基础结构。

**属性：**
- `Vector3[]? vertices` - 顶点数组
- `Vector3[]? normals` - 法线数组
- `Vector2[]? uvs0` - 纹理坐标集合0
- `Vector2[]? uvs1` - 纹理坐标集合1
- `int[]? triangles` - 三角形索引数组
- `Vector3[]? tangents` - 切线数组（预留）
- `Vector4[]? colors` - 颜色数组（预留）

### 2. MeshLibrary 类
提供多种标准几何体的生成方法。

#### 基础几何体生成方法：

**立方体**
```csharp
Mesh cube = MeshLibrary.CreateCube(size: 2f);
```

**球体**
```csharp
Mesh sphere = MeshLibrary.CreateSphere(radius: 1f, widthSegments: 32, heightSegments: 16);
```

**圆柱体**
```csharp
Mesh cylinder = MeshLibrary.CreateCylinder(radius: 1f, height: 2f, segments: 32);
```

**圆锥体**
```csharp
Mesh cone = MeshLibrary.CreateCone(radius: 1f, height: 2f, segments: 32);
```

**平面**
```csharp
Mesh plane = MeshLibrary.CreatePlane(width: 2f, height: 2f, widthSegments: 1, heightSegments: 1);
```

**金字塔**
```csharp
Mesh pyramid = MeshLibrary.CreatePyramid(baseSize: 2f, height: 2f);
```

**草叶（特殊用途）**
```csharp
Mesh grassBlade = MeshLibrary.CreateSingleGrassMesh(segmentCount: 7, width: 0.04f);
```

#### 新增：环面（Torus）✨
甜甜圈形状，由两个半径定义。

```csharp
// 创建环面
Mesh torus = MeshLibrary.CreateTorus(
    majorRadius: 1f,      // 主半径（圆心到圆心）
    minorRadius: 0.3f,    // 副半径（管子厚度）
    majorSegments: 32,    // 主环分段数
    minorSegments: 16     // 管子分段数
);
```

**参数说明：**
- `majorRadius` - 环形中心到管子中心的距离
- `minorRadius` - 管子的半径（厚度）
- `majorSegments` - 环形周向的分段数（分段数越多越光滑）
- `minorSegments` - 管子周向的分段数

**示例：**
```csharp
// 粗糙环面
var roughTorus = MeshLibrary.CreateTorus(1f, 0.3f, 16, 8);

// 光滑环面
var smoothTorus = MeshLibrary.CreateTorus(1.5f, 0.4f, 64, 32);

// 细管环面
var thinTorus = MeshLibrary.CreateTorus(2f, 0.1f, 48, 16);
```

### 3. BezierCurve 类 ✨
提供二维和三维贝塞尔曲线的计算和网格生成功能。

#### 3D 贝塞尔曲线

**二次贝塞尔曲线**
```csharp
Vector3 point = BezierCurve.QuadraticBezier(
    t: 0.5f,           // 曲线参数 (0-1)
    p0: new Vector3(0, 0, 0),      // 起点
    p1: new Vector3(1, 2, 0),      // 控制点
    p2: new Vector3(2, 0, 0)       // 终点
);
```

**三次贝塞尔曲线**
```csharp
Vector3 point = BezierCurve.CubicBezier(
    t: 0.5f,
    p0: new Vector3(0, 0, 0),      // 起点
    p1: new Vector3(1, 2, 0),      // 第一控制点
    p2: new Vector3(2, 2, 0),      // 第二控制点
    p3: new Vector3(3, 0, 0)       // 终点
);
```

#### 生成贝塞尔曲线点集

**二次贝塞尔曲线段**
```csharp
Vector3[] curvePoints = BezierCurve.GenerateQuadraticBezierCurve(
    p0: new Vector3(0, 0, 0),
    p1: new Vector3(1, 2, 0),
    p2: new Vector3(2, 0, 0),
    segmentCount: 50  // 生成50个点
);
```

**三次贝塞尔曲线段**
```csharp
Vector3[] curvePoints = BezierCurve.GenerateCubicBezierCurve(
    p0: new Vector3(0, 0, 0),
    p1: new Vector3(1, 2, 0),
    p2: new Vector3(2, 2, 0),
    p3: new Vector3(3, 0, 0),
    segmentCount: 50
);
```

#### 2D 贝塞尔曲线

```csharp
// 二次2D贝塞尔曲线
Vector2 point = BezierCurve.QuadraticBezier2D(t: 0.5f, p0, p1, p2);

// 三次2D贝塞尔曲线
Vector2 point = BezierCurve.CubicBezier2D(t: 0.5f, p0, p1, p2, p3);
```

#### 贝塞尔曲线挤出网格 ✨

沿着贝塞尔曲线路径挤出圆形截面以生成网格（如弯管、绳索等）。

```csharp
// 生成贝塞尔曲线路径
Vector3[] curvePoints = BezierCurve.GenerateCubicBezierCurve(
    new Vector3(-2, 0, 0),
    new Vector3(-1, 2, 0),
    new Vector3(1, 2, 0),
    new Vector3(2, 0, 0),
    segmentCount: 30
);

// 沿曲线挤出圆形截面
Mesh tubeMesh = BezierCurve.CreateBezierExtrusionMesh(
    curvePoints: curvePoints,
    profileRadius: 0.2f,     // 管子半径
    profileSegments: 12      // 截面分段数
);
```

### 4. MeshTools 类
提供网格操作和导出功能。

**网格复制到多个点**
```csharp
// 创建基础网格
Mesh baseMesh = MeshLibrary.CreateCube(1f);

// 定义放置位置
Vector3[] positions = new Vector3[] {
    new Vector3(-2, 0, 0),
    new Vector3(0, 0, 0),
    new Vector3(2, 0, 0)
};

// 复制到多个位置
Mesh mergedMesh = MeshTools.CopyMeshToPoints(positions, baseMesh);

// 带旋转的复制
Quaternion[] rotations = new Quaternion[] {
    Quaternion.AxisAngle(Vector3.up, 0),
    Quaternion.AxisAngle(Vector3.up, 45),
    Quaternion.AxisAngle(Vector3.up, 90)
};

Mesh rotatedMesh = MeshTools.CopyMeshToPoints(positions, baseMesh, rotations);
```

**导出到OBJ格式**
```csharp
Mesh mesh = MeshLibrary.CreateSphere();
MeshTools.WriteMeshToObj(mesh, "sphere.obj");
```

## 完整使用示例

### 示例1：创建和导出环面

```csharp
using GeometryTools;
using MathLibrary;

// 创建环面
Mesh torus = MeshLibrary.CreateTorus(
    majorRadius: 1f,
    minorRadius: 0.3f,
    majorSegments: 32,
    minorSegments: 16
);

// 导出为OBJ文件
MeshTools.WriteMeshToObj(torus, "torus.obj");
```

### 示例2：贝塞尔曲线管

```csharp
using GeometryTools;
using MathLibrary;

// 定义S形贝塞尔曲线
Vector3[] curve = BezierCurve.GenerateCubicBezierCurve(
    new Vector3(0, 0, 0),
    new Vector3(1, 1, 0),
    new Vector3(2, 1, 0),
    new Vector3(3, 0, 0),
    segmentCount: 50
);

// 沿曲线创建管状网格
Mesh tubeMesh = BezierCurve.CreateBezierExtrusionMesh(
    curvePoints: curve,
    profileRadius: 0.15f,
    profileSegments: 12
);

// 导出
MeshTools.WriteMeshToObj(tubeMesh, "bezier_tube.obj");
```

### 示例3：组合多个几何体

```csharp
using GeometryTools;
using MathLibrary;

// 创建多个环面
Mesh torus1 = MeshLibrary.CreateTorus(0.8f, 0.2f, 32, 16);

Vector3[] positions = new Vector3[] {
    new Vector3(-2, 0, 0),
    new Vector3(0, 0, 0),
    new Vector3(2, 0, 0)
};

// 在不同位置复制
Mesh combined = MeshTools.CopyMeshToPoints(positions, torus1);

// 导出组合网格
MeshTools.WriteMeshToObj(combined, "torus_array.obj");
```

## API 参考

### MeshLibrary 方法列表

| 方法名 | 参数 | 返回值 | 说明 |
|--------|------|--------|------|
| `CreateCube` | size | Mesh | 创建立方体 |
| `CreateSphere` | radius, widthSegments, heightSegments | Mesh | 创建球体 |
| `CreateCylinder` | radius, height, segments | Mesh | 创建圆柱体 |
| `CreateCone` | radius, height, segments | Mesh | 创建圆锥体 |
| `CreatePlane` | width, height, widthSegments, heightSegments | Mesh | 创建平面 |
| `CreatePyramid` | baseSize, height | Mesh | 创建金字塔 |
| `CreateTorus` | majorRadius, minorRadius, majorSegments, minorSegments | Mesh | 创建环面 |
| `CreateSingleGrassMesh` | segmentCount, width | Mesh | 创建草叶 |

### BezierCurve 方法列表

| 方法名 | 说明 |
|--------|------|
| `QuadraticBezier` | 计算3D二次贝塞尔曲线上的点 |
| `CubicBezier` | 计算3D三次贝塞尔曲线上的点 |
| `GenerateQuadraticBezierCurve` | 生成二次贝塞尔曲线的点集 |
| `GenerateCubicBezierCurve` | 生成三次贝塞尔曲线的点集 |
| `QuadraticBezier2D` | 计算2D二次贝塞尔曲线上的点 |
| `CubicBezier2D` | 计算2D三次贝塞尔曲线上的点 |
| `CreateBezierExtrusionMesh` | 沿贝塞尔曲线挤出截面生成网格 |

### MeshTools 方法列表

| 方法名 | 说明 |
|--------|------|
| `CopyMeshToPoints` | 复制网格到多个位置（可选旋转） |
| `WriteMeshToObj` | 导出网格为OBJ文件 |

## 常见问题

**Q: 如何创建光滑的环面？**
A: 增加 `majorSegments` 和 `minorSegments` 的值，例如使用64和32而不是默认的32和16。

**Q: 贝塞尔曲线的t参数代表什么？**
A: t是从0到1的参数，表示沿着曲线的位置。t=0表示起点，t=1表示终点。

**Q: 如何导出网格为其他格式？**
A: 当前仅支持OBJ格式。可以修改 `MeshTools.WriteMeshToObj` 方法来支持其他格式。

## 许可证

该库是ObjLibrary项目的一部分，遵循项目许可证。
