using BugVentureEngine.Factories;
using System.Collections.Generic;

namespace BugVentureEngine.Models
{
	public class Monster : LivingEntity
	{
		private readonly List<ItemPercentage> _lootTable = new List<ItemPercentage>();

		public int ID { get; }
		public string ImageName { get; }
		public int RewardExperiencePoints { get; }

		public Monster(int id, string name, string imageName,
			int maximumHitPoints, GameItem currentWeapon,
			int rewardExperiencePoints, int gold)
			: base(name, maximumHitPoints, maximumHitPoints, gold)
		{
			ID = id;
			ImageName = imageName;
			CurrentWeapon = currentWeapon;
			RewardExperiencePoints = rewardExperiencePoints;
		}

		public void AddItemToLootTable(int id, int percentage)
		{
			// 如果 loottable 已经存在此物件，则移除它。然后重新加入
			_lootTable.RemoveAll(ip => ip.ID == id);

			_lootTable.Add(new ItemPercentage(id, percentage));
		}

		public Monster GetNewInstance()
		{
			// 克隆此 monster 对象成为一个新的 Monster 对象
			Monster newMonster = new Monster(ID, Name, ImageName, MaximumHitPoints, CurrentWeapon, RewardExperiencePoints, Gold);

			foreach (ItemPercentage itemPercentage in _lootTable)
			{
				// 克隆掉落表，即使我们不会用到它
				newMonster.AddItemToLootTable(itemPercentage.ID, itemPercentage.Percentage);

				// 使用战利品表填充新怪物的物品栏
				if (RandomNumberGenerator.NumberBetween(1, 100) <= itemPercentage.Percentage)
				{
					newMonster.AddItemToInventory(ItemFactory.CreateGameItem(itemPercentage.ID));
				}
			}
			return newMonster;
		}
	}
}
