using BugVentureEngine.EventArgs;
using BugVentureEngine.Factories;
using BugVentureEngine.Models;
using System;
using System.Linq;

/// <summary>
/// ViewModel旨在管理视图和模型之间如何通信。 
///		- 我们可以使用此类来管理不同的模型。 例如，当玩家在战斗中时，我们将需要管理一个Player对象和一个Monster对象。 
///		- 通过增加class，我们可以获得创建自动化测试的能力。 因此，我们无需手动（缓慢地）测试我们的程序。
///		- 这也使我们能够轻松创建不同的UI –将程序转换为Web游戏或文本游戏。
/// </summary>
namespace BugVentureEngine.ViewModels
{
	public class GameSession : BaseNotificationClass
	{
		public event EventHandler<GameMessageEventArgs> OnMessageRaised; // View 订阅这个事件

		#region Properties

		private Location _currentLocation;
		private Monster _currentMonster;
		private Trader _currentTrader;

		public World CurrentWorld { get; set; }
		public Player CurrentPlayer { get; set; }

		public Location CurrentLocation
		{
			get { return _currentLocation; }
			set
			{
				_currentLocation = value;

				OnPropertyChanged(nameof(CurrentLocation));
				OnPropertyChanged(nameof(HasLocationToNorth));
				OnPropertyChanged(nameof(HasLocationToSouth));
				OnPropertyChanged(nameof(HasLocationToEast));
				OnPropertyChanged(nameof(HasLocationToWest));

				CompleteQuestsAtLocation();
				GivePlayerQuestsAtLocation();
				GetMonsterAtLocation();

				CurrentTrader = CurrentLocation.TraderHere;
			}
		}

		public Monster CurrentMonster
		{
			get { return _currentMonster; }
			set
			{
				_currentMonster = value;

				OnPropertyChanged(nameof(CurrentMonster));
				OnPropertyChanged(nameof(HasMonster));

				if (CurrentMonster != null)
				{
					RaiseMessage("");
					RaiseMessage($"You see a {CurrentMonster.Name} here");
				}
			}
		}

		public Trader CurrentTrader
		{
			get { return _currentTrader; }
			set
			{
				_currentTrader = value;

				OnPropertyChanged(nameof(CurrentTrader));
				OnPropertyChanged(nameof(HasTrader));
			}
		}

		public Weapon CurrentWeapon { get; set; }

		public bool HasLocationToNorth => CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1) != null;
		public bool HasLocationToEast => CurrentWorld.LocationAt(CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate) != null;
		public bool HasLocationToSouth => CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1) != null;
		public bool HasLocationToWest => CurrentWorld.LocationAt(CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate) != null;
		public bool HasMonster => CurrentMonster != null; // expression body
		public bool HasTrader => CurrentTrader != null;

		#endregion

		public GameSession()
		{
			CurrentPlayer = new Player
			{
				Name = "Xiaomai",
				CharacterClass = "Fighter",
				CurrentHitPoints = 10,
				MaximumHitPoints = 10,
				Gold = 1000000,
				ExperiencePoints = 0,
				Level = 1
			};

			if (!CurrentPlayer.Weapons.Any())
			{
				// 如果角色没有任何装备，则给他一个木棒
				CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(1001));
			}

			CurrentWorld = WorldFactory.CreateWorld();

			CurrentLocation = CurrentWorld.LocationAt(0, -1);
		}

		public void MoveNorth()
		{
			if (HasLocationToNorth)
			{
				CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1);
			}
		}

		public void MoveEast()
		{
			if (HasLocationToEast)
			{
				CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate);
			}
		}

		public void MoveSouth()
		{
			if (HasLocationToSouth)
			{
				CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1);
			}
		}

		public void MoveWest()
		{
			if (HasLocationToWest)
			{
				CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate);
			}
		}

		private void CompleteQuestsAtLocation()
		{
			foreach (Quest quest in CurrentLocation.QuestsAvailableHere)
			{
				QuestStatus questToComplete = CurrentPlayer.Quests.FirstOrDefault(q => q.PlayerQuest.ID == quest.ID && !q.IsCompleted);

				if (questToComplete != null)
				{
					if (CurrentPlayer.HasAllTheseItems(quest.ItemsToComplete))
					{
						// Remove the quest completion items from the player's inventory
						foreach (ItemQuantity itemQuantity in quest.ItemsToComplete)
						{
							for (int i = 0; i < itemQuantity.Quantity; i++)
							{
								CurrentPlayer.RemoveItemFromInventory(CurrentPlayer.Inventory.First(item => item.ItemTypeID == itemQuantity.ItemID));
							}
						}

						RaiseMessage("");
						RaiseMessage($"You completed the '{quest.Name}' quest");

						// Give the player the quest rewards
						CurrentPlayer.ExperiencePoints += quest.RewardExperiencePoints;
						RaiseMessage($"You receive {quest.RewardExperiencePoints} experience points");

						CurrentPlayer.Gold += quest.RewardGold;
						RaiseMessage($"You receive {quest.RewardGold} gold");

						foreach (ItemQuantity itemQuantity in quest.RewardItems)
						{
							GameItem rewardItem = ItemFactory.CreateGameItem(itemQuantity.ItemID);

							CurrentPlayer.AddItemToInventory(rewardItem);
							RaiseMessage($"You receive a {rewardItem.Name}");
						}

						// Mark the Quest as completed
						questToComplete.IsCompleted = true;
					}
				}
			}
		}

		private void GivePlayerQuestsAtLocation()
		{
			foreach (Quest quest in CurrentLocation.QuestsAvailableHere)
			{
				if (!CurrentPlayer.Quests.Any(q => q.PlayerQuest.ID == quest.ID))
				{
					CurrentPlayer.Quests.Add(new QuestStatus(quest));

					RaiseMessage("");
					RaiseMessage($"You receive the '{quest.Name}' quest");
					RaiseMessage(quest.Description);

					RaiseMessage("Return with:");
					foreach (ItemQuantity itemQuantity in quest.ItemsToComplete)
					{
						RaiseMessage($"   {itemQuantity.Quantity} {ItemFactory.CreateGameItem(itemQuantity.ItemID).Name}");
					}

					RaiseMessage("And you will receive:");
					RaiseMessage($"   {quest.RewardExperiencePoints} experience points");
					RaiseMessage($"   {quest.RewardGold} gold");
					foreach (ItemQuantity itemQuantity in quest.RewardItems)
					{
						RaiseMessage($"   {itemQuantity.Quantity} {ItemFactory.CreateGameItem(itemQuantity.ItemID).Name}");
					}
				}
			}
		}

		private void GetMonsterAtLocation()
		{
			CurrentMonster = CurrentLocation.GetMonster();
		}

		public void AttackCurrentMonster()
		{
			if (CurrentWeapon == null)
			{
				RaiseMessage("You must select a weapon, to attack");
				return;
			}

			// 计算对怪物照成的伤害
			int damageToMonster = RandomNumberGenerator.NumberBetween(CurrentWeapon.MinimumDamage, CurrentWeapon.MaximumDamage);

			if (damageToMonster == 0)
			{
				RaiseMessage($"You missed the {CurrentMonster.Name}.");
			}
			else
			{
				CurrentMonster.CurrentHitPoints -= damageToMonster;
				RaiseMessage($"You hit the {CurrentMonster.Name} for {damageToMonster} points");
			}

			// 如果怪物被杀死
			if (CurrentMonster.CurrentHitPoints <= 0)
			{
				RaiseMessage("");
				RaiseMessage($"You defeated the {CurrentMonster.Name}!");

				CurrentPlayer.ExperiencePoints += CurrentMonster.RewardExperiencePoints;
				RaiseMessage($"You receive {CurrentMonster.RewardExperiencePoints} experience points.");

				CurrentPlayer.Gold += CurrentMonster.Gold;
				RaiseMessage($"You receive {CurrentMonster.Gold} gold.");

				foreach (GameItem gameItem in CurrentMonster.Inventory)
				{
					CurrentPlayer.AddItemToInventory(gameItem);
					RaiseMessage($"You receive one {gameItem.Name}.");
				}

				// 继续刷新新的敌人
				GetMonsterAtLocation();
			}
			else
			{
				// 如果怪物还活着，轮到怪物的回合
				int damageToPlayer = RandomNumberGenerator.NumberBetween(CurrentMonster.MinimumDamage, CurrentMonster.MaximumDamage);

				if (damageToPlayer == 0)
				{
					RaiseMessage("The monster attacks, but miss you.");
				}
				else
				{
					CurrentPlayer.CurrentHitPoints -= damageToPlayer;
					RaiseMessage($"The {CurrentMonster.Name} hit you for {damageToPlayer} points.");
				}

				if (CurrentPlayer.CurrentHitPoints <= 0)
				{
					RaiseMessage("");
					RaiseMessage($"The {CurrentMonster.Name} killed you.");

					CurrentLocation = CurrentWorld.LocationAt(0, -1); // 回家
					CurrentPlayer.CurrentHitPoints = CurrentPlayer.Level * 10; // 完全恢复生命值
				}
			}
		}

		private void RaiseMessage(string message)
		{
			// 如果有任何东西订阅OnMessageRaised，调用
			OnMessageRaised?.Invoke(this, new GameMessageEventArgs(message));
		}
	}
}
