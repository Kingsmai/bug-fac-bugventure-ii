using BugVentureEngine.Models;
using BugVentureEngine.Shared;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace BugVentureEngine.Factories
{
	public static class TraderFactory
	{
		private const string GAME_DATA_FILENAME = ".\\GameData\\Traders.xml";

		private static readonly List<Trader> _traders = new List<Trader>();

		static TraderFactory()
		{
			if (File.Exists(GAME_DATA_FILENAME))
			{
				XmlDocument data = new XmlDocument();
				data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));

				loadTradersFromNodes(data.SelectNodes("/Traders/Trader"));
			}
			else
			{
				throw new FileNotFoundException($"Missing data file: {GAME_DATA_FILENAME}");
			}
		}

		private static void loadTradersFromNodes(XmlNodeList nodes)
		{
			foreach (XmlNode node in nodes)
			{
				Trader trader = new Trader(node.AttributeAsInt("ID"),
					node.SelectSingleNode("./Name")?.InnerText ?? "");

				foreach (XmlNode childNode in node.SelectNodes("./InventoryItems/Item"))
				{
					int quantity = childNode.AttributeAsInt("Quantity");

					// 为我们添加的每个项目创建一个新的 GameItem 对象。
					// 这是为了允许独特的物品，例如带有附魔的剑。 
					for (int i = 0; i < quantity; i++)
					{
						trader.AddItemToInventory(ItemFactory.CreateGameItem(childNode.AttributeAsInt("ID")));
					}
				}

				_traders.Add(trader);
			}
		}

		public static Trader GetTraderByID(int id)
		{
			return _traders.FirstOrDefault(t => t.ID == id);
		}
	}
}
