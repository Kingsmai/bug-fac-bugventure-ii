# BugVenture 2

> 核心架构非原创：学习对象：https://scottlilly.com/build-a-cwpf-rpg/
>
> Scott Lilly（原创）教学源码：[ScottLilly/SOSCSRPG: Scott's Open Source C# RPG (github.com)](https://github.com/ScottLilly/SOSCSRPG)

【游戏开发】纯UI游戏

【环境、技术】

- 集成环境：Visual Studio Community 2019
- C#（逻辑）、WPF（页面）

## 游戏设计

- 用户可以创建**角色**
  - 不同职业（每个职业有不同的加成）
- **角色**可以在**地点**之间移动
- 有些**地点**可能拥有可以战斗的**怪物**
- 如果**角色**杀死**怪物**，可以获得
  - 经验值、金币、随机**战利品**
- 如果**角色**没有成功杀死**怪物**，
  - 回家
  - 完全恢复
- 有些**地点**可能拥有**任务**
  - 完成任务需要提交**道具**
    - **道具**获得方法从怪物**战利品**
  - 如果玩家拥有完成任务所需要的**道具**，且将**道具**送回接**任务**的**地点**，会获得
    - 经验值、金币、奖励**道具**
- 有些**地点**可能拥有**商人**
  - **角色**可以买、卖**道具**
- 用户可以保存、加载游戏

![游戏地图规划](.\README.pngs\WPFGameWorld.png)

## 面向对象类

### 角色Player.cs

| 属性值                   | 数据类型 |
| ------------------------ | -------- |
| Name 名称                | string   |
| Class 职业               | Class    |
| Hit Points 生命值        | int      |
| Experience Points 经验值 | int      |
| Level 等级               | int      |
| Gold 金币                | int      |

### 物品栏设计

| 物品详情 | 数量 |
| -------- | ---- |
|          |      |

### 任务栏设计

| 任务详情 | 是否完成 |
| -------- | -------- |
|          |          |

## 项目结构简介

### BugVentureEngine项目

1. Models文件夹 - 存放数据模型（角色、怪物、地点……）
   1. Player.cs 角色、玩家
2. ViewModels - 用于View和Model之间的媒介（项目逻辑）
   1. GameSession.cs

