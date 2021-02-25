﻿using BugVentureEngine.Models;
using System.Collections.Generic;
using System.Linq;

namespace BugVentureEngine.Factories
{
	public static class ItemFactory
	{
		// 保存世界里所有的道具，以便可以达到“找到这个道具，然后返回这个道具”的效果
		private static readonly List<GameItem> _standardGameItems = new List<GameItem>();

		// 任何人使用任何这个类的东西，以下函数将会被执行
		static ItemFactory()
		{
			_standardGameItems.Add(new Weapon(1001, "Pointy Stick", 1, 1, 2));
			_standardGameItems.Add(new Weapon(1002, "Rusty Sword", 5, 1, 3));
			_standardGameItems.Add(new GameItem(9001, "Snake fang", 1));
			_standardGameItems.Add(new GameItem(9002, "Snakeskin", 2));
			_standardGameItems.Add(new GameItem(9003, "Rat tail", 1));
			_standardGameItems.Add(new GameItem(9004, "Rat fur", 2));
			_standardGameItems.Add(new GameItem(9005, "Spider fang", 1));
			_standardGameItems.Add(new GameItem(9006, "Spider silk", 2));
		}

		public static GameItem CreateGameItem(int itemTypeID)
		{
			GameItem standardItem = _standardGameItems.FirstOrDefault(item => item.ItemTypeID == itemTypeID);

			if (standardItem != null)
			{
				// 如果我们需要对武器进行附魔，那就必须要创建新的实例（让角色拥有多把武器等），每个实例有独立的属性
				if (standardItem is Weapon)
				{
					return (standardItem as Weapon).Clone(); // 因为Weapon有自己的属性
				}
				return standardItem.Clone();
			}

			return null;
		}
	}
}
