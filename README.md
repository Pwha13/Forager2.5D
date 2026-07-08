# Forager 2.5D

> 一款 2.5D 视角的生存建造类 Unity 游戏
> 采集、建造、制作、战斗、扩展岛屿！

![演示动图](./assets/gif1.gif)
![演示动图](./assets/gif2.gif)

演示动图位于assets文件夹，若演示动图未加载，请自行打开

## 技术栈

- **引擎**: Unity 2022.3.62f2c1
- **语言**: C#
- **渲染管线**: URP
- **插件**: DOTween (动画), Odin Inspector
- **输入系统**: Unity New Input System

## 核心系统

| 系统 | 说明 |
|------|------|
| 背包系统 | ScriptableObject 数据驱动，支持物品堆叠、快捷栏、装备栏 |
| 建造系统 | 网格对齐放置，物理检测防止重叠，支持陆地/水面两种模式 |
| 制造系统 | 工作台（锻造台/熔炉/缝纫台），配方解锁+材料检查 |
| AI 系统 | 行为树驱动的怪物 AI（史莱姆/猪），包含巡逻、追击、受伤反馈 |
| 存档系统 | JSON 本地存档，保存玩家状态、背包、岛屿、生成物 |
| 昼夜循环 | 实时光照角度变化，影响怪物生成类型 |
| 对象池 | 基于 ID 的通用对象池，支持自动回收 |
| 地图生成 | BFS 算法生成岛屿，最外圈随机删减实现不规则边缘 |

## 项目结构

```
Assets/
├── C#Scripts/
│   ├── Block/          # 方块、工作台、岛屿
│   ├── Player/         # 玩家移动、攻击、状态
│   ├── Manage/         # 数据管理、鼠标、相机、对象池
│   ├── UI/             # UI 系统
│   ├── Mob/            # 怪物 AI
│   ├── Backpack/       # 背包逻辑
│   ├── ScriptsObject/  # ScriptableObject 数据定义
│   └── Interact/       # 交互接口
├── Resources/Prefeb/   # 预制体
├── BehaviorTree/       # 行为树框架
├── Setting/            # Input Action 配置
└── Scenes/             # 场景文件
```

## 运行方式

1. 使用 Unity Hub 打开项目，选择 Unity 2022.3.62f2c1
2. 打开 `Assets/Scenes/SampleScene.unity`
3. 点击 Play 运行

## 操作说明

| 按键 | 功能 |
|------|------|
| WASD | 移动 |
| 鼠标左键 | 攻击/采集/交互 |
| 鼠标右键 | 打开工作台/使用物品 |
| E | 背包与设置 |
| 1-5 | 切换快捷栏工具 |
