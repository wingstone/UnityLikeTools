# GeometryTools 快速参考卡

## 📌 新增功能概览

### 1️⃣ Torus（环面）
```csharp
// 创建甜甜圈形状
var torus = MeshLibrary.CreateTorus(
    majorRadius: 1f,       // 环半径
    minorRadius: 0.3f,     // 管子厚度
    majorSegments: 32,     // 环分段
    minorSegments: 16      // 管分段
);
```

### 2️⃣ Bezier Curves（贝塞尔曲线）

#### 2D 曲线计算
```csharp
// 二次贝塞尔曲线
Vector2 point = BezierCurve.QuadraticBezier2D(0.5f, p0, p1, p2);

// 三次贝塞尔曲线
Vector2 point = BezierCurve.CubicBezier2D(0.5f, p0, p1, p2, p3);
```

#### 3D 曲线计算
```csharp
// 二次贝塞尔曲线
Vector3 point = BezierCurve.QuadraticBezier(0.5f, p0, p1, p2);

// 三次贝塞尔曲线
Vector3 point = BezierCurve.CubicBezier(0.5f, p0, p1, p2, p3);
```

#### 曲线生成
```csharp
// 生成点集
Vector3[] curve = BezierCurve.GenerateCubicBezierCurve(
    new Vector3(-1, 0, 0),
    new Vector3(0, 1, 0),
    new Vector3(1, 1, 0),
    new Vector3(2, 0, 0),
    segmentCount: 50
);
```

#### 曲线挤出（★ 特色功能）
```csharp
// 沿曲线路径挤出管子
Mesh tubeMesh = BezierCurve.CreateBezierExtrusionMesh(
    curvePoints: curve,
    profileRadius: 0.2f,
    profileSegments: 12
);
```

## 💾 文件导出

```csharp
// 导出网格为OBJ
MeshTools.WriteMeshToObj(mesh, "output.obj");
```

## 📊 完整方法列表

### MeshLibrary
| 方法 | 参数 | 说明 |
|------|------|------|
| `CreateTorus` | majorRadius, minorRadius, majorSegments, minorSegments | 环面 |
| `CreateCube` | size | 立方体 |
| `CreateSphere` | radius, widthSegments, heightSegments | 球体 |
| `CreateCylinder` | radius, height, segments | 圆柱 |
| `CreateCone` | radius, height, segments | 圆锥 |
| `CreatePlane` | width, height, widthSegments, heightSegments | 平面 |
| `CreatePyramid` | baseSize, height | 金字塔 |

### BezierCurve
| 方法 | 说明 |
|------|------|
| `QuadraticBezier` | 3D二次贝塞尔 |
| `CubicBezier` | 3D三次贝塞尔 |
| `QuadraticBezier2D` | 2D二次贝塞尔 |
| `CubicBezier2D` | 2D三次贝塞尔 |
| `GenerateQuadraticBezierCurve` | 生成二次曲线点集 |
| `GenerateCubicBezierCurve` | 生成三次曲线点集 |
| `CreateBezierExtrusionMesh` | 曲线挤出网格 |

### MeshTools
| 方法 | 说明 |
|------|------|
| `CopyMeshToPoints` | 复制网格到多个位置 |
| `WriteMeshToObj` | 导出为OBJ文件 |

## 🎨 常用示例

### 示例1: 创建环面数组
```csharp
Mesh torus = MeshLibrary.CreateTorus(1f, 0.3f, 32, 16);

Vector3[] positions = new Vector3[] {
    new Vector3(-2, 0, 0),
    new Vector3(0, 0, 0),
    new Vector3(2, 0, 0)
};

Mesh array = MeshTools.CopyMeshToPoints(positions, torus);
MeshTools.WriteMeshToObj(array, "torus_array.obj");
```

### 示例2: S形弯管
```csharp
Vector3[] bezierPath = BezierCurve.GenerateCubicBezierCurve(
    new Vector3(-2, 0, 0),
    new Vector3(-1, 1, 0),
    new Vector3(1, 1, 0),
    new Vector3(2, 0, 0),
    segmentCount: 100
);

Mesh tube = BezierCurve.CreateBezierExtrusionMesh(
    bezierPath, 0.15f, 12
);

MeshTools.WriteMeshToObj(tube, "s_pipe.obj");
```

### 示例3: 贝塞尔曲线采样
```csharp
// 获取曲线上的特定点
Vector3 start = BezierCurve.CubicBezier(0.0f, p0, p1, p2, p3);    // 起点
Vector3 quarter = BezierCurve.CubicBezier(0.25f, p0, p1, p2, p3); // 1/4处
Vector3 half = BezierCurve.CubicBezier(0.5f, p0, p1, p2, p3);     // 中点
Vector3 end = BezierCurve.CubicBezier(1.0f, p0, p1, p2, p3);      // 终点
```

## ⚠️ 常见错误

### 错误1: 参数t超出范围
```csharp
// ❌ 错误 - t不在0-1范围内
Vector3 point = BezierCurve.QuadraticBezier(1.5f, p0, p1, p2);

// ✅ 正确
Vector3 point = BezierCurve.QuadraticBezier(0.5f, p0, p1, p2);
```

### 错误2: 曲线点数不足
```csharp
// ❌ 错误 - segmentCount < 2
Vector3[] curve = BezierCurve.GenerateCubicBezierCurve(p0, p1, p2, p3, 1);

// ✅ 正确
Vector3[] curve = BezierCurve.GenerateCubicBezierCurve(p0, p1, p2, p3, 50);
```

### 错误3: 空曲线点数组
```csharp
// ❌ 错误
Vector3[] emptyPoints = new Vector3[0];
Mesh mesh = BezierCurve.CreateBezierExtrusionMesh(emptyPoints, 0.2f, 12);

// ✅ 正确
Vector3[] curvePoints = BezierCurve.GenerateCubicBezierCurve(p0, p1, p2, p3, 50);
Mesh mesh = BezierCurve.CreateBezierExtrusionMesh(curvePoints, 0.2f, 12);
```

## 🚀 性能提示

- **高质量** - 使用高 segmentCount（64+）获得光滑外观
- **低质量** - 使用低 segmentCount（8-16）以获得更好性能
- **平衡** - 对大多数情况，32x16 或 48x16 是很好的平衡点

## 📂 文件位置

```
GeometryTools/
├── Src/
│   ├── Mesh.cs              ← 网格数据结构
│   ├── MeshLibrary.cs       ← 几何体生成（含Torus）
│   ├── MeshTools.cs         ← 网格工具
│   └── BezierCurve.cs       ← 贝塞尔曲线 ⭐ 新增
└── GeometryTools.csproj
```

---
**快速构建命令：**
```bash
dotnet build UnityLikeTools.sln
```

**导出演示网格：**
```bash
dotnet run --project GeometryToolsDemo.cs
```
