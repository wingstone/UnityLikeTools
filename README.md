# UnityLikeTools - Unity风格工具集

一个为C#开发者提供的Unity风格工具集合，包含数学库、几何工具和模型IO处理。

## 概述

UnityLikeTools 是一套完整的C#库，为游戏开发、图形应用和3D建模提供基础工具。项目采用模块化设计，包含四个核心组件：

- **MathLibrary** - Unity风格的数学库
- **GeometryTools** - 几何体生成和网格处理工具
- **ModelIOTools** - 3D模型文件导入导出
- **Demo** - 示例演示项目

## 项目结构

```
UnityLikeTools/
├── UnityLikeTools.sln          # 解决方案文件
├── Directory.Build.props        # 全局配置
├── MathLibrary/                 # 数学库
│   ├── MathLibrary.csproj
│   └── Src/
│       ├── Vector2.cs           # 2D向量
│       ├── Vector3.cs           # 3D向量
│       ├── Vector4.cs           # 4D向量
│       ├── VectorInt2.cs        # 整数2D向量
│       ├── VectorInt3.cs        # 整数3D向量
│       ├── VectorInt4.cs        # 整数4D向量
│       ├── Color.cs             # 颜色
│       ├── Quaternion.cs        # 四元数
│       ├── Matrix4x4.cs         # 4x4矩阵
│       ├── Transform.cs         # 变换
│       ├── Random.cs            # 随机数生成器
│       └── Mathf.cs             # 数学函数
├── GeometryTools/               # 几何工具
│   ├── GeometryTools.csproj
│   └── Src/
│       ├── Mesh.cs              # 网格数据结构
│       ├── MeshLibrary.cs       # 几何体工厂
│       ├── MeshTools.cs         # 网格处理工具
│       └── BezierCurve.cs       # 贝塞尔曲线
├── ModelIOTools/                # 模型IO工具
│   ├── ModelIOTools.csproj
│   └── Src/
│       ├── ObjFileHandler.cs    # OBJ格式处理
│       └── GltfFileHandler.cs   # GLTF格式处理
└── Demo/                        # 演示项目
    ├── Demo.csproj
    └── Src/
        └── DemoMain.cs
```

## 核心功能

### 📐 MathLibrary - 数学库

提供完整的Unity风格数学类型和函数：

#### 向量类型
- **Vector2/3/4** - 浮点向量，支持点积、叉积、归一化、插值等
- **VectorInt2/3/4** - 整数向量，用于离散计算
- 完整的运算符重载（+、-、*、/、==、!=）

#### 变换与旋转
- **Quaternion** - 四元数旋转，支持欧拉角、轴角转换和SLERP插值
- **Matrix4x4** - 4x4变换矩阵，支持TRS变换和矩阵运算
- **Transform** - 组合位置、旋转、缩放的变换组件

#### 其他工具
- **Color** - RGBA颜色（浮点[0,1]），支持颜色插值和整数转换
- **Mathf** - 数学工具类：三角函数、插值、限制、取整等
- **Random** - 随机数生成器

### 🔷 GeometryTools - 几何工具

强大的网格生成和处理功能：

#### 基础几何体（MeshLibrary）
- 立方体（Cube）
- 球体（Sphere）
- 圆柱体（Cylinder）
- 圆锥体（Cone）
- 环面（Torus）
- 平面（Plane）
- 胶囊体（Capsule）
- 草叶（GrassBlade）

#### 贝塞尔曲线（BezierCurve）
- 二次和三次贝塞尔曲线计算
- 2D和3D曲线支持
- 曲线点集生成
- 曲线挤出成网格

#### 网格处理（MeshTools）
- 网格合并
- 网格变换
- 法线计算
- 网格优化

### 📦 ModelIOTools - 模型IO

支持主流3D模型格式的导入导出：

#### OBJ格式
- 读取OBJ文件（顶点、法线、UV、面）
- 导出OBJ文件
- 支持多材质分组

#### GLTF格式
- 读取GLTF/GLB文件
- 导出GLTF/GLB文件  
- 支持嵌入式纹理和二进制数据

## 快速开始

### 安装要求

- .NET 8.0 或更高版本
- Visual Studio 2022 或 Visual Studio Code
- C# 扩展（VS Code）

### 编译项目

```bash
# 编译整个解决方案
dotnet build UnityLikeTools.sln

# 编译单个项目
dotnet build MathLibrary/MathLibrary.csproj
dotnet build GeometryTools/GeometryTools.csproj
dotnet build ModelIOTools/ModelIOTools.csproj
```

### 运行演示

```bash
dotnet run --project Demo/Demo.csproj
```

## 使用示例

### 数学运算

```csharp
using MathLibrary;

// 向量运算
Vector3 position = new Vector3(1f, 2f, 3f);
Vector3 direction = Vector3.forward;
float distance = Vector3.Distance(position, direction);

// 四元数旋转
Quaternion rotation = Quaternion.Euler(45f, 0f, 0f);
Vector3 rotated = rotation.RotateVector(Vector3.up);

// 矩阵变换
Matrix4x4 transform = Matrix4x4.TRS(position, rotation, Vector3.one);
```

### 生成几何体

```csharp
using GeometryTools;
using MathLibrary;

// 创建球体
Mesh sphere = MeshLibrary.CreateSphere(1.0f, 32, 16);

// 创建环面
Mesh torus = MeshLibrary.CreateTorus(2.0f, 0.5f, 32, 16);

// 创建贝塞尔曲线挤出网格
Vector3 p0 = Vector3.zero;
Vector3 p1 = new Vector3(0, 5, 0);
Vector3 p2 = new Vector3(5, 5, 0);
Vector3 p3 = new Vector3(5, 0, 0);
Mesh bezierMesh = BezierCurve.CreateBezierExtrusionMesh(
    p0, p1, p2, p3, segments: 32, sides: 8, radius: 0.1f
);
```

### 模型导入导出

```csharp
using ModelIOTools;
using GeometryTools;

// 导出为OBJ
Mesh mesh = MeshLibrary.CreateCube(1.0f);
ObjFileHandler.WriteMesh(mesh, "cube.obj");

// 导出为GLTF
GltfFileHandler.WriteMesh(mesh, "cube.gltf");

// 读取OBJ文件
Mesh loadedMesh = ObjFileHandler.ReadMesh("model.obj");

// 读取GLTF文件
Mesh gltfMesh = GltfFileHandler.ReadMesh("model.gltf");
```

## 坐标系说明

- **左手坐标系**
- **Z轴向上**
- **Y轴向前**

这与Unity的默认坐标系不同，使用时请注意坐标转换。

## 扩展功能

### 自定义几何体

继承或扩展 `MeshLibrary` 类来创建自定义几何体：

```csharp
public static Mesh CreateCustomShape(float size)
{
    var mesh = new Mesh();
    // 设置顶点、法线、UV、三角形索引
    return mesh;
}
```

### 自定义文件格式

实现自定义的文件读写器，参考 `ObjFileHandler` 的实现模式。

## 性能优化

- 使用 `struct` 类型（Vector、Color等）减少GC压力
- 大量网格操作时考虑使用对象池
- 批量操作使用 `MeshTools.MergeMeshes` 减少绘制调用

## 常见问题

**Q: 如何在我的项目中引用这些库？**  
A: 使用项目引用：`dotnet add reference path/to/MathLibrary.csproj`

**Q: 是否与Unity引擎兼容？**  
A: API风格相似但不完全兼容。由于坐标系差异，需要进行转换。

**Q: 支持跨平台吗？**  
A: 是的，支持所有.NET 8.0支持的平台（Windows、Linux、macOS）。

**Q: 如何贡献代码？**  
A: 欢迎提交 Pull Request 或报告 Issue。

## 许可证

MIT License

## 更新日志

### 2026-02-07
- 重构项目文档结构
- 统一命名为 UnityLikeTools
- 更新 README 内容

### 2026-01-17
- 添加 Torus（环面）几何体
- 完善 Bezier 曲线功能
- 增加曲线挤出网格功能

## 联系方式

如有问题或建议，请通过 GitHub Issues 联系。
