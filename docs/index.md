# BugVentureEngine Documentation

## BugVentureEngine

### BaseNotificationClass

Extend: INotifyPropertyChanged

当属性被更改时通知客户端

#### 方法

```C#
protected virtual void OnPropertyChanged(string propertyName)
{
	PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
```

### RandomNumberGenerator

#### 方法

##### NumberBetween()

获取两个数之间的随机值

```c#
public static int NumberBetween(int minimumValue, int maximumValue)
```

##### SimpleNumberBetween()

简易版获取两个数之间的随机值（但是没那么随机）

```c#
public static int SimpleNumberBetween(int minimumValue, int maximumValue)
```

## BugVentureEngine.Models

数据模型（角色、怪物、地点……）

### Player

Extend: [BaseNotificationClass](#BaseNotificationClass)

角色、玩家类

#### 属性

| 属性值            | 用途/详情                  | 数据类型                            |
| ----------------- | -------------------------- | ----------------------------------- |
| Name              | 名称                       | string                              |
| Class             | 职业（每个职业有不同加成） | string                              |
| Hit Points        | 生命值                     | int                                 |
| Experience Points | 经验值                     | int                                 |
| Level             | 等级                       | int                                 |
| Gold              | 金币                       | int                                 |
| Inventory         | 物品栏                     | ObservableCollection\<GameItem\>    |
| Quests            | 任务栏                     | ObservableCollection\<QuestStatus\> |

> ObservableCollection在属性变动时会自动通知UI

#### 构造方法

```C#
public Player()
```

### Location

游戏位置（小地图）

#### 属性

| 属性值              | 用途/详情                                                    | 数据类型                 |
| ------------------- | ------------------------------------------------------------ | ------------------------ |
| XCoordinate         | x坐标                                                        | int                      |
| YCoordinate         | y坐标                                                        | int                      |
| Name                | 名称                                                         | string                   |
| Description         | 叙述                                                         | string                   |
| ImageName           | 图片路径<br />`/AssemblyName;component/path/to/image.extension` | string                   |
| QuestsAvailableHere | 在这里会触发的任务                                           | List\<Quest\>            |
| MonstersHere        | 在这里的怪物                                                 | List\<MonsterEncounter\> |

#### 方法

##### AddMonster()

往当前地点添加怪物

```C#
public void AddMonster(int monsterID, int chanceOfEncountering)
```

##### GetMonster()

如果当前地点有怪物，则会返回一只怪物

```C#
public Monster GetMonster()
```

### World

游戏世界（大地图）

#### 属性

| 属性值     | 用途/详情                | 数据类型         |
| ---------- | ------------------------ | ---------------- |
| _locations | 【私有】大地图中所有地点 | List\<Location\> |

#### 方法

##### AddLocation()

往私有变量`_locations`中添加新的地点

```C#
internal void AddLocation(int xCoordinates, int yCoordinates, string name, string description, string imageName)
```

##### LocationAt()

获取某个坐标所存在于`_locations`的地点（如果该位置没有地点，则返回null）

```c#
public Location LocationAt(int xCoordinates, int yCoordinates)
```

### GameItem

游戏道具

#### 属性

| 属性值     | 用途/详情 | 数据类型 |
| ---------- | --------- | -------- |
| ItemTypeID | 唯一ID值  | int      |
| Name       | 名称      | string   |
| Price      | 价格      | int      |

#### 构造方法

```c#
public GameItem(int itemTypeID, string name, int price)
```

#### 方法

##### Clone()

克隆一个新的实例物品

> 如果我们的武器可以进行附魔，那么每把武器（或道具）就需要有独立的属性。这时候就需要new新的道具实例，才能让每个道具拥有独立属性

```c#
public GameItem Clone()
```

### Weapon

Extend: [GameItem](#GameItem)

武器

#### 属性

| 属性值        | 用途/详情  | 数据类型 |
| ------------- | ---------- | -------- |
| MinimumDamage | 最低攻击力 | int      |
| MaximumDamage | 最大攻击力 | int      |

#### 构造方法

```c#
public Weapon(int itemTypeID, string name, int price, int minDamage, int maxDamage) : base(itemTypeID, name, price)
```

#### 方法

##### Clone()

克隆一个新的实例武器

```C#
public new Weapon Clone()
```

### ItemQuantity

记录物品数量（比如提交任务所需的物品）

#### 属性

| 属性值   | 用途/详情                        | 数据类型 |
| -------- | -------------------------------- | -------- |
| ItemID   | 必须和GameItem关联，物品的唯一ID | int      |
| Quantity | 物品数量                         | int      |

#### 构造方法

```c#
public ItemQuantity(int itemID, int quantity)
```

### Quest

任务

#### 属性

| 属性值                 | 用途/详情        | 数据类型             |
| ---------------------- | ---------------- | -------------------- |
| ID                     | 任务唯一ID       | int                  |
| Name                   | 名称             | string               |
| Description            | 描述             | string               |
| ItemsToComplete        | 任务完成关键道具 | List\<ItemQuantity\> |
| RewardExperiencePoints | 奖励经验值       | int                  |
| RewardGold             | 奖励金币         | int                  |
| RewardItems            | 奖励道具         | List\<ItemQuantity\> |

#### 构造方法

```C#
public Quest(int id, string name, string description, List<ItemQuantity> itemsToComplete,
             int rewardExperiencePoints, int rewardGold, List<ItemQuantity> rewardItems)
```

### QuestStatus

任务状态（是否完成）

#### 属性

| 属性值      | 用途/详情      | 数据类型 |
| ----------- | -------------- | -------- |
| PlayerQuest | 玩家拥有的任务 | Quest    |
| IsCompleted | 任务是否完成   | bool     |

#### 构造方法

```c#
public QuestStatus(Quest quest)
```

### Monster

Extend: [BaseNotificationClass](#BaseNotificationClass)

怪物

#### 属性

| 属性值                 | 用途/详情        | 数据类型                             |             |
| ---------------------- | ---------------- | ------------------------------------ | ----------- |
| Name                   | 名称             | string                               | private set |
| ImageName              | 图片名           | string                               |             |
| MaximumHitPoints       | 最大生命值       | int                                  | private set |
| HitPoints              | 当前生命值       | int                                  | private set |
| RewardExperiencePoints | 奖励经验值       | int                                  | private set |
| RewardGold             | 奖励金币         | int                                  | private set |
| Inventory              | 物品栏（掉落物） | ObservableCollection\<ItemQuantity\> |             |

#### 构造方法

```c#
public Monster(string name, string imageName, int maximumHitPoints, int hitPoints, int rewardExperiencePoints, int rewardGold)
```

### MonsterEncounter

怪物触发器（遇怪）

#### 属性

| 属性值               | 用途/详情 | 数据类型 |
| -------------------- | --------- | -------- |
| MonsterID            | 怪物ID    | int      |
| ChanceOfEncountering | 遇敌概率  | int      |

#### 构造方法

```c#
public MonsterEncounter(int monsterID, int chanceOfEncountering)
```

## BugVentureEngine.ViewModels

MVVM - 用于View和Model之间的媒介（项目逻辑）

### GameSession

Extend: [BaseNotificationClass](#BaseNotificationClass)

游戏内容和UI之间的交互

#### 属性

| 属性值             | 用途/详情                                                    | 数据类型 |
| ------------------ | ------------------------------------------------------------ | -------- |
| CurrentWorld       | 当前所处世界                                                 | World    |
| CurrentPlayer      | 当前角色                                                     | Player   |
| CurrentLocation    | 当前地点<br />修改它的值时，会自动给玩家当前地点所存在的任务<br />会自动刷新当前面对的怪物 | Location |
| CurrentMonster     | 当前面对的怪物                                               | Monster  |
| HasLocationToNorth | 【只读】检查北边是否有路                                     | bool     |
| HasLocationToEast  | 【只读】检查东边是否有路                                     | bool     |
| HasLocationToSouth | 【只读】检查南边是否有路                                     | bool     |
| HasLocationToWest  | 【只读】检查西边是否有路                                     | bool     |
| HasMonster         | 当当前位置有怪物时，为true                                   | bool     |

#### 构造方法

创建新角色、通过WorldFactory创建新世界、将玩家放置在家中

```C#
public GameSession()
```

#### 方法

##### MoveNorth()

向北移动

##### MoveSouth()

向南移动

##### MoveEast()

向东移动

##### MoveWest()

向西移动

##### GivePlayerQuestAtLocation()

检查当前地点是否拥有任务，有的话交给玩家

```C#
private void GivePlayerQuestsAtLocation()
```

##### GetMonsterAtLocation()

获取当前地点的怪物

```C#
CurrentMonster = CurrentLocation.GetMonster();
```

## BugVentureEngine.Factories

### WorldFactory

世界工厂 - 生成由小地图合并起来的大地图

#### 方法

##### CreateWorld()

用来创建整个世界

```c#
internal static World CreateWorld()
```

### ItemFactory

物品工厂 - 生成游戏中的所有物品

#### 属性

| 属性值             | 用途/详情                                                    | 数据类型         |
| ------------------ | ------------------------------------------------------------ | ---------------- |
| _standardGameItems | 【私有、static】保存世界里所有的道具，以便可以达到“找到这个道具，然后返回这个道具”的效果 | List\<GameItem\> |

#### 构造方法

将武器、道具加入到游戏中

```c#
static ItemFactory()
```

#### 方法

##### CreateGameItem()

会创建一个新的实例

```C#
public static GameItem CreateGameItem(int itemTypeID)
```

### QuestFactory

任务工厂 - 生成游戏中的所有任务

#### 属性

| 属性值  | 用途/详情                                                    | 数据类型      |
| ------- | ------------------------------------------------------------ | ------------- |
| _quests | 【私有、static】保存世界里所有的任务，以便可以达到“找到这个任务，然后返回这个任务”的效果 | List\<Quest\> |

#### 构造方法

将任务加入到游戏中

```C#
static QuestFactory()
```

#### 方法

##### GetQuestByID()

通过ID获取任务

```c#
internal static Quest GetQuestByID(int id)
```

### MonsterFactory

#### 方法

##### GetMonster()

通过怪物ID获取怪物

```c#
public static Monster GetMonster(int monsterID)
```

##### AddLootItem()

随机增加怪物掉落物

```c#
private static void AddLootItem(Monster monster, int itemID, int percentage)
```

# 游戏中的元素列表

## 地点

| 名称               | 描述                                                         | X坐标 | Y坐标 |
| ------------------ | ------------------------------------------------------------ | ----- | ----- |
| Farmer's Field     | There are rows of corn growing here, with giant rats hiding between them. | -2    | -1    |
| Farmer's House     | This is the house of your neighbor, Farmer Ted.              | -1    | -1    |
| Home               | This is your home                                            | 0     | -1    |
| Trading Shop       | The shop of Susan, the trader.                               | -1    | 0     |
| Town square        | You see a fountain here.                                     | 0     | 0     |
| Town Gate          | There is a gate here, protecting the town from giant spiders. | 1     | 0     |
| Spider Forest      | The trees in this forest are covered with spider webs.       | 2     | 0     |
| Herbalist's hut    | You see a small hut, with plants drying from the roof.       | 0     | 1     |
| Herbalist's garden | There are many plants here, with snakes hiding behind them.  | 0     | 2     |

## 道具

| 类型     | ID   | 名称         | 价格 | 最小攻击力 | 最大攻击力 |
| -------- | ---- | ------------ | ---- | ---------- | ---------- |
| Weapon   | 1001 | Pointy Stick | 1    | 1          | 2          |
| Weapon   | 1002 | Rusty Sword  | 5    | 1          | 3          |
| GameItem | 9001 | Snake fang   | 1    |            |            |
| GameItem | 9002 | Snakeskin    | 2    |            |            |
| GameItem | 9003 | Rat tail     | 1    |            |            |
| GameItem | 9004 | Rat fur      | 2    |            |            |
| GameItem | 9005 | Spider fang  | 1    |            |            |
| GameItem | 9006 | Spider silk  | 2    |            |            |

## 任务

| ID   | 名称                  | 描述                                        | 完成任务所需道具 | 奖励经验值 | 奖励金币 | 奖励道具        |
| ---- | --------------------- | ------------------------------------------- | ---------------- | ---------- | -------- | --------------- |
| 1    | Clear the herb garden | Defeat the snakes in the Herbalist's garden | Snake fang * 5   | 25         | 10       | Rusty Sword * 1 |

## 怪物

| ID   | 名称         | 生命值 | 奖励经验值 | 奖励金币 | 掉落物                               | 出现地点           |
| ---- | ------------ | ------ | ---------- | -------- | ------------------------------------ | ------------------ |
| 1    | Snake        | 4      | 5          | 1        | Snake fang 25%<br />Snakeskin 75%    | Herbalist's garden |
| 2    | Rat          | 5      | 5          | 1        | Rat tail 25%<br />Rat fur 75%        | Farmer's Field     |
| 3    | Giant Spider | 10     | 10         | 3        | Spider fang 25%<br />Spider silk 75% | Spider Forest      |