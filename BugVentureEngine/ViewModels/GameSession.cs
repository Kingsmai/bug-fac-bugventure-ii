﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using BugVentureEngine.Models;
using BugVentureEngine.Factories;
using BugVentureEngine.EventArgs;

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

		private Location _currentLocation;
		private Monster _currentMonster;

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

				GivePlayerQuestsAtLocation();
				GetMonsterAtLocation();
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

		public Weapon CurrentWeapon { get; set; }

		public bool HasLocationToNorth { get { return CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1) != null; } }
		public bool HasLocationToEast { get { return CurrentWorld.LocationAt(CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate) != null; } }
		public bool HasLocationToSouth { get { return CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1) != null; } }
		public bool HasLocationToWest { get { return CurrentWorld.LocationAt(CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate) != null; } }
		public bool HasMonster => CurrentMonster != null; // expression body

		public GameSession()
		{
			CurrentPlayer = new Player
			{
				Name = "Xiaomai",
				CharacterClass = "Fighter",
				HitPoints = 10,
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

		private void GivePlayerQuestsAtLocation()
		{
			foreach (Quest quest in CurrentLocation.QuestsAvailableHere)
			{
				if (!CurrentPlayer.Quests.Any(q => q.PlayerQuest.ID == quest.ID))
				{
					CurrentPlayer.Quests.Add(new QuestStatus(quest));
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
				CurrentMonster.HitPoints -= damageToMonster;
				RaiseMessage($"You hit the {CurrentMonster.Name} for {damageToMonster} points");
			}

			// 如果怪物被杀死
			if (CurrentMonster.HitPoints <= 0)
			{
				RaiseMessage("");
				RaiseMessage($"You defeated the {CurrentMonster.Name}!");

				CurrentPlayer.ExperiencePoints += CurrentMonster.RewardExperiencePoints;
				RaiseMessage($"You receive {CurrentMonster.RewardExperiencePoints} experience points.");

				CurrentPlayer.Gold += CurrentMonster.RewardGold;
				RaiseMessage($"You receive {CurrentMonster.RewardGold} gold.");

				foreach (ItemQuantity itemQuantity in CurrentMonster.Inventory)
				{
					GameItem item = ItemFactory.CreateGameItem(itemQuantity.ItemID);
					CurrentPlayer.AddItemToInventory(item);
					RaiseMessage($"You receive {itemQuantity.Quantity} {item.Name}.");
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
					CurrentPlayer.HitPoints -= damageToPlayer;
					RaiseMessage($"The {CurrentMonster.Name} hit you for {damageToPlayer} points.");
				}

				if (CurrentPlayer.HitPoints <= 0)
				{
					RaiseMessage("");
					RaiseMessage($"The {CurrentMonster.Name} killed you.");

					CurrentLocation = CurrentWorld.LocationAt(0, 1); // 回家
					CurrentPlayer.HitPoints = CurrentPlayer.Level * 10; // 完全恢复生命值
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
