using BugVentureEngine.Models;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using BugVentureEngine.Shared;
using System.Linq;
using System;

namespace BugVentureEngine.Factories
{
	public static class MonsterFactory
	{
		private const string GAME_DATA_FILENAME = ".\\GameData\\Monsters.xml";

		private static readonly List<Monster> _baseMonster = new List<Monster>();

		static MonsterFactory()
		{
			if (File.Exists(GAME_DATA_FILENAME))
			{
				XmlDocument data = new XmlDocument();
				data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));

				string rootImagePath = data.SelectSingleNode("/Monsters").AttributeAsString("RootImagePath");

				LoadMonstersFromNode(data.SelectNodes("/Monsters/Monster"), rootImagePath);
			}
			else
			{
				throw new FileNotFoundException($"Missing data file: {GAME_DATA_FILENAME}");
			}
		}

		private static void LoadMonstersFromNode(XmlNodeList nodes, string rootImagePath)
		{
			if (nodes == null)
			{
				return;
			}

			foreach (XmlNode node in nodes)
			{
				Monster monster = new Monster(node.AttributeAsInt("ID"),
					node.AttributeAsString("Name"),
					$"{rootImagePath}{node.AttributeAsString("ImageName")}",
					node.AttributeAsInt("MaximumHitPoints"),
					ItemFactory.CreateGameItem(node.AttributeAsInt("WeaponID")),
					node.AttributeAsInt("RewardXP"),
					node.AttributeAsInt("Gold"));

				XmlNodeList lootItemNodes = node.SelectNodes("./LootItems/LootItem");
				if (lootItemNodes != null)
				{
					foreach (XmlNode lootItemNode in lootItemNodes)
					{
						monster.AddItemToLootTable(lootItemNode.AttributeAsInt("ID"),
							lootItemNode.AttributeAsInt("Percentage"));
					}
				}

				_baseMonster.Add(monster);
			}
		}

		public static Monster GetMonster(int id)
		{
			return _baseMonster.FirstOrDefault(m => m.ID == id)?.GetNewInstance();
		}
	}
}
