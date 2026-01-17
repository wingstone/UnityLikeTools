# GeometryTools 新增功能总结

## 📋 更新日期
2026年1月17日

## ✨ 新增功能

### 1. 环面（Torus）网格生成
**位置：** `GeometryTools/Src/MeshLibrary.cs`

**方法签名：**
```csharp
public static Mesh CreateTorus(
    float majorRadius = 1f, 
    float minorRadius = 0.3f, 
    int majorSegments = 32, 
    int minorSegments = 16)
```

**功能描述：**
- 创建甜甜圈形状的3D网格
- 支持自定义主半径和副半径
- 可调节环面的光滑度

**参数说明：**
- `majorRadius` - 从环心到管心的距离
- `minorRadius` - 管子的半径（厚度）
- `majorSegments` - 环周向分段数（默认32，越大越光滑）
- `minorSegments` - 管子周向分段数（默认16，越大越光滑）

**使用示例：**
```csharp
// 标准环面
var torus = MeshLibrary.CreateTorus(1f, 0.3f, 32, 16);

// 光滑环面
var smoothTorus = MeshLibrary.CreateTorus(1f, 0.3f, 64, 32);

// 粗糙环面
var roughTorus = MeshLibrary.CreateTorus(1f, 0.3f, 16, 8);
```

### 2. 贝塞尔曲线功能库
**位置：** `GeometryTools/Src/BezierCurve.cs` (新文件)

#### 2.1 3D贝塞尔曲线计算

**二次贝塞尔曲线：**
```csharp
public static Vector3 QuadraticBezier(
    float t, 
    Vector3 p0, 
    Vector3 p1, 
    Vector3 p2)
```
- 基于三个控制点的二阶贝塞尔曲线
- 参数 t 范围为 0 到 1

**三次贝塞尔曲线：**
```csharp
public static Vector3 CubicBezier(
    float t, 
    Vector3 p0, 
    Vector3 p1, 
    Vector3 p2, 
    Vector3 p3)
```
- 基于四个控制点的三阶贝塞尔曲线
- 提供更多的形状控制灵活性

#### 2.2 贝塞尔曲线点集生成

**二次曲线段生成：**
```csharp
public static Vector3[] GenerateQuadraticBezierCurve(
    Vector3 p0, 
    Vector3 p1, 
    Vector3 p2, 
    int segmentCount)
```

**三次曲线段生成：**
```csharp
public static Vector3[] GenerateCubicBezierCurve(
    Vector3 p0, 
    Vector3 p1, 
    Vector3 p2, 
    Vector3 p3, 
    int segmentCount)
```

#### 2.3 2D贝塞尔曲线

```csharp
public static Vector2 QuadraticBezier2D(float t, Vector2 p0, Vector2 p1, Vector2 p2)
public static Vector2 CubicBezier2D(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
```

#### 2.4 贝塞尔曲线挤出网格

**方法：**
```csharp
public static Mesh CreateBezierExtrusionMesh(
    Vector3[] curvePoints, 
    float profileRadius, 
    int profileSegments)
```

**功能描述：**
- 沿着贝塞尔曲线路径挤出圆形截面
- 用于创建弯管、绳索、DNA螺旋等复杂形状
- 自动计算每个点的局部坐标系

**参数说明：**
- `curvePoints` - 贝塞尔曲线上的点数组（通过 GenerateQuadraticBezierCurve 或 GenerateCubicBezierCurve 生成）
- `profileRadius` - 圆形截面的半径
- `profileSegments` - 截面圆周分段数（越大越光滑）

**使用示例：**
```csharp
// 创建S形曲线路径
Vector3[] curve = BezierCurve.GenerateCubicBezierCurve(
    new Vector3(0, 0, 0),
    new Vector3(1, 1, 0),
    new Vector3(2, 1, 0),
    new Vector3(3, 0, 0),
    segmentCount: 50
);

// 沿曲线挤出管状网格
Mesh tubeMesh = BezierCurve.CreateBezierExtrusionMesh(
    curvePoints: curve,
    profileRadius: 0.2f,
    profileSegments: 16
);

// 导出为OBJ文件
MeshTools.WriteMeshToObj(tubeMesh, "curved_tube.obj");
```

## 📊 文件变更

### 修改文件
- `GeometryTools/Src/MeshLibrary.cs` - 添加 CreateTorus 方法

### 新增文件
- `GeometryTools/Src/BezierCurve.cs` - 贝塞尔曲线工具类
- `GEOMETRY_TOOLS_GUIDE.md` - 详细使用指南
- `GeometryToolsDemo.cs` - 功能演示程序

## 🧪 测试和验证

所有项目编译成功（✓ 通过）：
- MathLibrary
- MathLibraryTest
- GeometryTools
- ObjGenerator

### 生成的演示文件
运行 `GeometryToolsDemo.cs` 会生成以下OBJ文件：
1. `torus_smooth.obj` - 光滑环面 (64×32分段)
2. `torus_thick.obj` - 粗厚环面 (48×24分段)
3. `torus_thin.obj` - 细管环面 (64×16分段)
4. `bezier_tube.obj` - S形贝塞尔曲线管

## 🎯 应用场景

### Torus 网格
- 甜甜圈形装饰
- 轮胎模型
- 环形结构体
- 磁环
- 3D几何教学

### Bezier 曲线
- 曲线路径编辑
- 弯曲管道生成
- DNA螺旋可视化
- 动画路径
- 复杂曲线表面
- 参数化曲线设计

## 📝 API完整参考

### MeshLibrary 新方法

```csharp
// 创建环面
public static Mesh CreateTorus(
    float majorRadius = 1f,        // 环主半径
    float minorRadius = 0.3f,      // 管子半径
    int majorSegments = 32,        // 环分段数
    int minorSegments = 16)        // 管分段数
```

### BezierCurve 全部方法

| 方法 | 参数 | 返回值 | 描述 |
|------|------|--------|------|
| `QuadraticBezier` | t, p0, p1, p2 | Vector3 | 3D二次贝塞尔 |
| `CubicBezier` | t, p0, p1, p2, p3 | Vector3 | 3D三次贝塞尔 |
| `QuadraticBezier2D` | t, p0, p1, p2 | Vector2 | 2D二次贝塞尔 |
| `CubicBezier2D` | t, p0, p1, p2, p3 | Vector2 | 2D三次贝塞尔 |
| `GenerateQuadraticBezierCurve` | p0, p1, p2, count | Vector3[] | 生成二次曲线 |
| `GenerateCubicBezierCurve` | p0, p1, p2, p3, count | Vector3[] | 生成三次曲线 |
| `CreateBezierExtrusionMesh` | points, radius, segments | Mesh | 创建挤出网格 |

## 💡 代码示例

### 完整示例：创建和导出复杂形状

```csharp
using GeometryTools;
using MathLibrary;

// 创建平滑的环面
var torus = MeshLibrary.CreateTorus(
    majorRadius: 2.0f,
    minorRadius: 0.4f,
    majorSegments: 64,
    minorSegments: 32
);
MeshTools.WriteMeshToObj(torus, "my_torus.obj");

// 创建贝塞尔曲线管
Vector3[] bezierPath = BezierCurve.GenerateCubicBezierCurve(
    new Vector3(-3, 0, 0),
    new Vector3(-1, 2, 1),
    new Vector3(1, 2, -1),
    new Vector3(3, 0, 0),
    segmentCount: 100
);

var bezierTube = BezierCurve.CreateBezierExtrusionMesh(
    bezierPath, 
    profileRadius: 0.25f, 
    profileSegments: 20
);
MeshTools.WriteMeshToObj(bezierTube, "bezier_tube.obj");
```

## 🚀 后续可能扩展

- 添加B样条曲线支持
- 添加Catmull-Rom曲线
- 添加曲面生成（Bezier补丁）
- 添加网格平滑处理
- 添加法线生成功能
- 支持更多导出格式（FBX, GLTF等）

## ✅ 验证清单

- ✓ 环面网格生成正常
- ✓ 贝塞尔曲线计算准确
- ✓ 贝塞尔挤出网格生成正确
- ✓ 代码编译无错误
- ✓ 代码编译无警告
- ✓ OBJ导出功能正常
- ✓ 与现有功能兼容

---

**版本信息：**
- GeometryTools v1.1
- 基于 MathLibrary v1.0
- 目标框架：.NET 8.0
