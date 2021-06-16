using BugVentureEngine.Models;
using BugVentureEngine.Shared;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace BugVentureEngine.Factories
{
	internal static class QuestFactory
	{
		private const string GAME_DATA_FILENAME = ".\\GameData\\Quests.xml";

		private static readonly List<Quest> _quests = new List<Quest>();

		static QuestFactory()
		{
			if (File.Exists(GAME_DATA_FILENAME))
			{
				XmlDocument data = new XmlDocument();
				data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));

				LoadQuestsFromNodes(data.SelectNodes("/Quests/Quest"));
			}
			else
			{
				throw new FileNotFoundException($"Missing data file: {GAME_DATA_FILENAME}");
			}
		}

		private static void LoadQuestsFromNodes(XmlNodeList nodes)
		{
			if (nodes == null)
			{
				return;
			}

			foreach (XmlNode node in nodes)
			{
				// 声明需要完成任务的关键道具，和任务奖励道具
				List<ItemQuantity> itemsToComplete = new List<ItemQuantity>();
				List<ItemQuantity> rewardItems = new List<ItemQuantity>();

				foreach (XmlNode childNode in node.SelectNodes("./ItemsToComplete/Item"))
				{
					itemsToComplete.Add(new ItemQuantity(childNode.AttributeAsInt("ID"), childNode.AttributeAsInt("Quantity")));
				}

				foreach (XmlNode childNode in node.SelectNodes("./RewardItems/Item"))
				{
					rewardItems.Add(new ItemQuantity(childNode.AttributeAsInt("ID"), childNode.AttributeAsInt("Quantity")));
				}

				// 创建新的任务
				_quests.Add(new Quest(node.AttributeAsInt("ID"),
									  node.SelectSingleNode("./Name")?.InnerText ?? "",
									  node.SelectSingleNode("./Description")?.InnerText ?? "",
									  itemsToComplete,
									  node.AttributeAsInt("RewardExperiencePoints"),
									  node.AttributeAsInt("RewardGold"),
									  rewardItems));
			}
		}

		internal static Quest GetQuestByID(int id)
		{
			return _quests.FirstOrDefault(quest => quest.ID == id);
		}
	}
}
