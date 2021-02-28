using BugVentureEngine.Actions;

namespace BugVentureEngine.Models
{
	public class GameItem
	{
		public enum ItemCategory
		{
			Miscellaneous,
			Weapon,
			Consumable
		}

		public ItemCategory Category { get; }
		public int ItemTypeID { get; set; }
		public string Name { get; set; }
		public int Price { get; set; }
		public bool IsUnique { get; set; }
		public IAction Action { get; set; }

		public GameItem(ItemCategory category, int itemTypeID, string name, int price,
			bool isUnique = false, IAction action = null)
		{
			Category = category;
			ItemTypeID = itemTypeID;
			Name = name;
			Price = price;
			IsUnique = isUnique;
			Action = action;
		}

		public void PerformAction(LivingEntity actor, LivingEntity target)
		{
			Action?.Execute(actor, target);
		}

		/// <summary>
		/// 克隆一个新的实例物品
		/// </summary>
		/// <returns></returns>
		public GameItem Clone()
		{
			// 当前物品的属性会传给克隆的构造函数
			return new GameItem(Category, ItemTypeID, Name, Price, IsUnique, Action);
		}
	}
}
