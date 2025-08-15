# 角色系统单元测试套件

## 概述

本测试套件为修仙Demo的角色系统提供了全面的单元测试覆盖，使用GdUnit4测试框架。

## 测试文件结构

```
scripts/tests/characters/
├── CharacterLevelGdUnitTest.cs      # CharacterLevel类单元测试
├── ExperienceSystemGdUnitTest.cs    # ExperienceSystem类单元测试
├── PlayerGdUnitTest.cs              # Player类单元测试
├── TestRunner.cs                    # 集成测试运行器
├── TESTING_SUMMARY.md               # 测试完成总结
└── README.md                       # 本文档
```

## 重要注解说明

### [RequireGodotRuntime]注解

由于Player类继承自Godot的CharacterBody2D，需要使用Godot运行时环境。因此，所有涉及Player实例的测试方法都添加了`[RequireGodotRuntime]`注解：

```csharp
[TestCase]
[RequireGodotRuntime]
public void TestInitialCharacterName()
{
    // 测试代码
}
```

### 使用场景

- **需要注解的情况**:
  - 测试类继承自Godot.Node或相关类型
  - 使用Godot的SceneTree功能
  - 访问Godot的资源系统

- **不需要注解的情况**:
  - 纯逻辑类测试（如CharacterLevel、ExperienceSystem）
  - 不依赖Godot运行时环境的测试

## 测试运行方式

### 1. Godot编辑器

1. 打开Godot编辑器
2. 确保项目已编译成功
3. 打开"测试"面板（如果未显示，请启用GdUnit4插件）
4. 选择要运行的测试类或方法
5. 点击运行按钮

### 2. 命令行

```bash
# 编译项目
dotnet build

# 运行所有测试
dotnet test

# 运行特定测试类
dotnet test --filter "FullyQualifiedName~PlayerGdUnitTest"
```

### 3. 集成测试

使用TestRunner.cs中的集成测试：

```csharp
// 在Godot脚本中运行集成测试
var testRunner = new CharacterSystemTestRunner();
testRunner.TestCharacterSystemIntegration();
```

## 测试配置

### 项目依赖

在`xiuxian_demo.csproj`中已配置：

```xml
<PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.8.0" />
<PackageReference Include="gdUnit4.api" Version="5.0.0" />
<PackageReference Include="gdUnit4.test.adapter" Version="3.0.0" />
<PackageReference Include="gdUnit4.analyzers" Version="1.0.0" />
```

### 编译状态

- ✅ **编译成功**: 项目已成功编译
- ⚠️ **警告**: 5个非关键警告（分析器版本不匹配等）
- ❌ **错误**: 0个编译错误

## 测试最佳实践

### 1. 命名规范
- 测试方法名清晰表达测试意图
- 使用`Test`前缀+功能描述+场景

### 2. 注解使用
- 根据测试类型正确使用`[RequireGodotRuntime]`
- 使用`[BeforeTest]`和`[AfterTest]`进行测试准备和清理

### 3. 断言风格
- 使用GdUnit4提供的断言方法
- 保持断言简单明了

### 4. Mock策略
- 对于Godot依赖，使用反射或直接设置
- 对于纯逻辑类，直接实例化测试

## 常见问题

### Q: 测试运行时出现"需要Godot运行时"错误
A: 确保为涉及Godot功能的测试方法添加了`[RequireGodotRuntime]`注解

### Q: 测试在命令行运行失败但在编辑器成功
A: 确保使用Godot的Mono版本运行测试，或直接在Godot编辑器中运行

### Q: 如何处理Godot节点测试
A: 使用`[RequireGodotRuntime]`注解，并考虑使用GdUnit4的节点测试功能

## 扩展建议

1. **添加更多边界测试**
2. **集成性能测试**
3. **添加UI交互测试**
4. **实现测试数据驱动**
5. **添加并发测试**

---

**更新日期**: 2024年
**测试框架**: GdUnit4 v5.0.0
**Godot版本**: 4.4.1.mono