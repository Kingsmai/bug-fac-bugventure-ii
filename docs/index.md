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

Extend: [LivingEntity](#LivingEntity)

角色、玩家类

#### 属性

| 属性值            | 用途/详情                            | 数据类型                            |
| ----------------- | ------------------------------------ | ----------------------------------- |
| Class             | 职业（每个职业有不同加成）           | string                              |
| Experience Points | 经验值                               | int                                 |
| Weapons           | 武器列表：自动找到物品栏里的所有武器 | List\<GameItem\>                    |
| Quests            | 任务栏                               | ObservableCollection\<QuestStatus\> |

> ObservableCollection在属性变动时会自动通知UI

#### 事件

##### OnLeveledUp

玩家升级

#### 构造方法

```C#
public Player(string name, string characterClass, int experiencePoints, int maximumHitPoints, int currentHitPoints, int gold)
```

#### 方法

[AddItemToInventory(GameItem item)](#AddItemToInventory(GameItem item))

[RemoveItemFromInventory(GameItem Item)](#RemoveItemFromInventory(GameItem Item))

##### HasAllTheseItems(List\<ItemQuantity\> items)

检查是否拥有列表里的所有物品，并且数量是否足够

##### AddExperience(int experience)

添加经验值

##### SetLevelAndMaximumHitPoints()

计算经验值，如果升级的话：增加最大生命值、提交OnLeveledUp事件

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
| TraderHere          | 在这里的商人                                                 | Trader                   |

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

#### 枚举

1. Miscellaneous
2. Weapon
3. Consumable

#### 属性

| 属性值            | 用途/详情                                 | 数据类型                          |          |
| ----------------- | ----------------------------------------- | --------------------------------- | -------- |
| Category          | 枚举选择道具类型                          | ItemCategory                      |          |
| ItemTypeID        | 唯一ID值                                  | int                               |          |
| Name              | 名称                                      | string                            |          |
| Price             | 价格                                      | int                               |          |
| IsUnique          | 是否单独生成一个对象                      | bool                              |          |
| ~~MaximumDamage~~ | 27/2/2021更新 -> 移除<br />最大攻击力     | int                               | get only |
| ~~MinimumDamage~~ | 27/2/2021更新 -> 移除<br />最小攻击力     | int                               | get only |
| Action            | 27/2/2021更新<br />动作（CommandPattern） | ~~AttackWithWeapon~~<br />IAction |          |

#### 构造方法

```c#
public GameItem(ItemCategory category, int itemTypeID, string name, int price,
			bool isUnique = false, AttackWithWeapon action = null)
```

#### 方法

##### Clone()

克隆一个新的实例物品

> 如果我们的武器可以进行附魔，那么每把武器（或道具）就需要有独立的属性。这时候就需要new新的道具实例，才能让每个道具拥有独立属性

```c#
public GameItem Clone()
```

### [Deprecated] Weapon

Extend: [GameItem](#GameItem)

武器 - 27/2/2021 已废弃

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

Extend: [LivingEntity](#LivingEntity)

怪物

#### 属性

| 属性值                 | 用途/详情                      | 数据类型 |             |
| ---------------------- | ------------------------------ | -------- | ----------- |
| ImageName              | 图片名                         | string   |             |
| ~~MinimumDamage~~      | 最小攻击力<br />28/2/2021 移除 | int      |             |
| ~~MaximumDamage~~      | 最大攻击力<br />28/2/2021 移除 | int      |             |
| RewardExperiencePoints | 奖励经验值                     | int      | private set |

#### 构造方法

```c#
public Monster(string name, string imageName, int maximumHitPoints, int currentHitPoints,
               int rewardExperiencePoints, int gold)
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

### Trader

Extend: [LivingEntity](#LivingEntity)

#### 属性

| 属性值 | 用途/详情 | 数据类型 |
| ------ | --------- | -------- |
|        |           |          |

#### 构造方法

```c#
public Trader(string name)
```

#### 方法

[AddItemToInventory(GameItem item)](#AddItemToInventory(GameItem item))

[RemoveItemFromInventory(GameItem Item)](#RemoveItemFromInventory(GameItem Item))

### LivingEntity

Extend: [BaseNotificationClass](#BaseNotificationClass)

【抽象类】所有游戏中的生物（玩家、怪物、商人等）

#### 属性

| 属性值            | 用途/详情                          | 数据类型                                     |               |
| ----------------- | ---------------------------------- | -------------------------------------------- | ------------- |
| Name              | 名称                               | string                                       | private set   |
| CurrentHitPoints  | 当前生命值                         | int                                          | private set   |
| MaximumHitPoints  | 最大生命值                         | int                                          | protected set |
| Gold              | 金币                               | int                                          | private set   |
| Level             | 等级                               | int                                          | protected set |
| CurrentWeapon     | 27/2/2021 更新<br />当前所持武器   | GameItem                                     |               |
| CurrentConsumable | 当前所持可消耗道具                 | GameItem                                     |               |
| Inventory         | 物品栏                             | ObservableCollection\<GameItem\>             |               |
| GroupedInventory  | 带有数量的物品栏                   | ObservableCollection\<GroupedInventoryItem\> |               |
| Weapons           | 物品栏列表里的武器                 | List\<GameItem\>                             |               |
| Consumables       | 物品栏列表里的可消耗道具           | List\<GameItem\>                             |               |
| HasConsumable     | 【UI用】检测玩家是否持有可消耗道具 | bool                                         |               |
| IsDead            | 判断当前生物是否已经死亡           | bool                                         |               |

#### 事件

OnKilled - 当此生物死亡触发

OnActionPerformed - 当此生物执行某项动作

#### 构造方法

```c#
protected LivingEntity(string name, int maximumHitPoints, int currentHitPoints, int gold, int level = 1)
```

#### 方法

[AddItemToInventory(GameItem item)](#AddItemToInventory(GameItem item))

[RemoveItemFromInventory(GameItem Item)](#RemoveItemFromInventory(GameItem Item))

### GroupedInventoryItem

Extend: [BaseNotificationClass](#BaseNotificationClass)

是ItemQuantity类的衍生，保存物品的数量

#### 属性

| 属性值   | 用途/详情 | 数据类型 |
| -------- | --------- | -------- |
| Item     | 游戏物品  | GameItem |
| Quantity | 物品数量  | int      |

## BugVentureEngine.ViewModels

MVVM - 用于View和Model之间的媒介（项目逻辑）

### GameSession

Extend: [BaseNotificationClass](#BaseNotificationClass)

游戏内容和UI之间的交互

#### 属性

| 属性值             | 用途/详情                                                    | 数据类型                 |
| ------------------ | ------------------------------------------------------------ | ------------------------ |
| CurrentWorld       | 当前所处世界                                                 | World                    |
| CurrentPlayer      | 当前角色                                                     | Player                   |
| CurrentLocation    | 当前地点<br />修改它的值时，会自动给玩家当前地点所存在的任务<br />会自动刷新当前面对的怪物 | Location                 |
| CurrentMonster     | 当前面对的怪物                                               | Monster                  |
| CurrentTrader      | 当前地点所存在的商人                                         | Trader                   |
| ~~CurrentWeapon~~  | 27/2/2021 摒弃<br />当前装备武器                             | ~~Weapon~~<br />GameItem |
| HasLocationToNorth | 【只读】检查北边是否有路                                     | bool                     |
| HasLocationToEast  | 【只读】检查东边是否有路                                     | bool                     |
| HasLocationToSouth | 【只读】检查南边是否有路                                     | bool                     |
| HasLocationToWest  | 【只读】检查西边是否有路                                     | bool                     |
| HasMonster         | 当当前位置有怪物时，为true                                   | bool                     |
| HasTrader          | 当当前位置有商人时，为true                                   | bool                     |

#### 构造方法

创建新角色、通过WorldFactory创建新世界、将玩家放置在家中，并给予玩家一把初始武器

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

##### CompleteQuestsAtLocation()

检查玩家是否完成任务，并奖励道具

##### GivePlayerQuestAtLocation()

检查当前地点是否拥有任务，有的话交给玩家，并显示任务信息

```C#
private void GivePlayerQuestsAtLocation()
```

##### GetMonsterAtLocation()

获取当前地点的怪物

```C#
CurrentMonster = CurrentLocation.GetMonster();
```

##### AttackCurrentMonster()

攻击当前面对的怪物

##### UseCurrentConsumable()

使用当前的可消耗道具

##### OnCurrentPlayerPerformedAction

当前玩家执行动作

##### OnCurrentMonsterPerformedAction

当前怪物执行动作

##### OnCurrentPlayerKilled

当前玩家被杀死之后自动触发

##### OnCurrentMonsterKilled

当前怪物被杀死之后自动触发

##### RaiseMessage(string message)

提升信息到UI（如果UI绑定OnMessageRaised事件）

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

| 属性值             | 用途/详情                                                    | 数据类型         |                         |
| ------------------ | ------------------------------------------------------------ | ---------------- | ----------------------- |
| _standardGameItems | 保存世界里所有的道具，以便可以达到“找到这个道具，然后返回这个道具”的效果 | List\<GameItem\> | private static readonly |

#### 构造方法

将武器、道具加入到游戏中

```c#
static ItemFactory()
```

#### 方法

##### CreateGameItem()

会创建一个新的实例

`2021/2/23`会根据物品类型返回不同的实例（GameItem, Weapon等）

```C#
public static GameItem CreateGameItem(int itemTypeID)
```

### QuestFactory

任务工厂 - 生成游戏中的所有任务

#### 属性

| 属性值  | 用途/详情                                                    | 数据类型      |                         |
| ------- | ------------------------------------------------------------ | ------------- | ----------------------- |
| _quests | 保存世界里所有的任务，以便可以达到“找到这个任务，然后返回这个任务”的效果 | List\<Quest\> | private static readonly |

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

### TraderFactory

创建商人对象

#### 属性

| 属性值   | 用途/详情    | 数据类型       |                         |
| -------- | ------------ | -------------- | ----------------------- |
| _traders | 保存所有商人 | List\<Trader\> | private static readonly |

#### 方法

##### GetTraderByName(string name)

通过名字获取商人对象

##### AddTraderToList(Trader trader)

将商人添加到商人列表里

## BugVentureEngine.EventArgs

### GameMessageEventArgs

Extends: System.EventArgs

The ViewModel is going to communicate with the View by “raising events”. This will let the View know that something happened. But, the View also needs to know what text to display on the screen.

To send additional information with an event, you use an “event argument”. We’re going to create a custom event argument that will hold the text to display in the View.

#### 属性

| 属性值  | 用途/详情                        | 数据类型 |             |
| ------- | -------------------------------- | -------- | ----------- |
| Message | hold the message text to display | string   | private set |

#### 构造方法

```c#
public GameMessageEventArgs(string message)
```

## BugVentureEngine.Actions

### BaseAction

抽象类

### IAction

命令设计模式的接口

### AttackWithWeapon

Extends: [BaseAction](#BaseAction), [IAction](#IAction)

Execute actor 对 target 造成伤害

### Heal

Extends: [BaseAction](#BaseAction), [IAction](#IAction)

Execute

# 所有方法

## AddItemToInventory(GameItem item)

将物品加入到物品栏

## RemoveItemFromInventory(GameItem Item)

将物品从物品栏中移除

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

| 类型        | ID   | 名称         | 价格 | 最小攻击力 | 最大攻击力 | 回复效果 |
| ----------- | ---- | ------------ | ---- | ---------- | ---------- | -------- |
| Weapon      | 1001 | Pointy Stick | 1    | 1          | 2          |          |
| Weapon      | 1002 | Rusty Sword  | 5    | 1          | 3          |          |
|             |      |              |      |            |            |          |
| Weapon      | 1501 | Snake fangs  | 0    | 0          | 2          |          |
| Weapon      | 1502 | Rat claws    | 0    | 0          | 2          |          |
| Weapon      | 1503 | Spider fangs | 0    | 0          | 4          |          |
|             |      |              |      |            |            |          |
| HealingItem | 2001 | GranolaBar   | 5    |            |            | 2        |
|             |      |              |      |            |            |          |
| GameItem    | 9001 | Snake fang   | 1    |            |            |          |
| GameItem    | 9002 | Snakeskin    | 2    |            |            |          |
| GameItem    | 9003 | Rat tail     | 1    |            |            |          |
| GameItem    | 9004 | Rat fur      | 2    |            |            |          |
| GameItem    | 9005 | Spider fang  | 1    |            |            |          |
| GameItem    | 9006 | Spider silk  | 2    |            |            |          |

## 任务

| ID   | 名称                  | 描述                                        | 完成任务所需道具 | 奖励经验值 | 奖励金币 | 奖励道具        |
| ---- | --------------------- | ------------------------------------------- | ---------------- | ---------- | -------- | --------------- |
| 1    | Clear the herb garden | Defeat the snakes in the Herbalist's garden | Snake fang * 5   | 25         | 10       | Rusty Sword * 1 |

## 怪物

| ID   | 名称         | 生命值 | 武器ID | 奖励经验值 | 奖励金币 | 掉落物                               | 出现地点           |
| ---- | ------------ | ------ | ------ | ---------- | -------- | ------------------------------------ | ------------------ |
| 1    | Snake        | 4      | 1501   | 5          | 1        | Snake fang 25%<br />Snakeskin 75%    | Herbalist's garden |
| 2    | Rat          | 5      | 1502   | 5          | 1        | Rat tail 25%<br />Rat fur 75%        | Farmer's Field     |
| 3    | Giant Spider | 10     | 1503   | 10         | 3        | Spider fang 25%<br />Spider silk 75% | Spider Forest      |

## 商人

| 名字               | 商品 | 出现地点        |
| ------------------ | ---- | --------------- |
| Susan              |      | Trading Shop    |
| Farmer Ted         |      | Farmer's House  |
| Pete the Herbalist |      | Herbalist's Hut |

