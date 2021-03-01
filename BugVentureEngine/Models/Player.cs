﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BugVentureEngine.Models
{
	public class Player : LivingEntity
	{
		#region Properties

		private string _characterClass;
		private int _experiencePoints;

		public string CharacterClass
		{
			get { return _characterClass; }
			set
			{
				_characterClass = value;
				OnPropertyChanged(nameof(CharacterClass));
			}
		}

		public int ExperiencePoints
		{
			get { return _experiencePoints; }
			private set
			{
				_experiencePoints = value;
				OnPropertyChanged(nameof(ExperiencePoints));
				SetLevelAndMaximumHitPoints();
			}
		}

		public ObservableCollection<QuestStatus> Quests { get; }

		public ObservableCollection<Recipe> Recipes { get; }

		#endregion

		public event EventHandler OnLeveledUp;

		public Player(string name, string characterClass, int experiencePoints, int maximumHitPoints, int currentHitPoints, int gold)
			: base(name, maximumHitPoints, currentHitPoints, gold)
		{
			CharacterClass = characterClass;
			ExperiencePoints = experiencePoints;

			Quests = new ObservableCollection<QuestStatus>();
			Recipes = new ObservableCollection<Recipe>();
		}

		public bool HasAllTheseItems(List<ItemQuantity> items)
		{
			foreach (ItemQuantity item in items)
			{
				if (Inventory.Count(i => i.ItemTypeID == item.ItemID) < item.Quantity)
				{
					return false;
				}
			}
			return true;
		}

		public void AddExperience(int experiencePoints)
		{
			ExperiencePoints += experiencePoints;
		}

		public void LearnRecipe(Recipe recipe)
		{
			if (!Recipes.Any(r => r.ID == recipe.ID))
			{
				Recipes.Add(recipe);
			}
		}

		private void SetLevelAndMaximumHitPoints()
		{
			int originalLevel = Level;
			Level = (ExperiencePoints / 100) + 1;
			if (Level != originalLevel)
			{
				MaximumHitPoints = Level * 10;
				OnLeveledUp?.Invoke(this, System.EventArgs.Empty);
			}
		}
	}
}
