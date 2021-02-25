namespace BugVentureEngine.Models
{
	public class GameItem
	{
		public int ItemTypeID { get; set; }
		public string Name { get; set; }
		public int Price { get; set; }

		public GameItem(int itemTypeID, string name, int price)
		{
			ItemTypeID = itemTypeID;
			Name = name;
			Price = price;
		}

		/// <summary>
		/// 克隆一个新的实例物品
		/// </summary>
		/// <returns></returns>
		public GameItem Clone()
		{
			// 当前物品的属性会传给克隆的构造函数
			return new GameItem(ItemTypeID, Name, Price);
		}
	}
}
