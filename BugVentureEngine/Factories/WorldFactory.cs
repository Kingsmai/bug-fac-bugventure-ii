using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugVentureEngine.Models;

namespace BugVentureEngine.Factories
{
	/// <summary>
	/// 用来“生产”整个世界
	/// internal关键字是用来给同一个项目之间使用，这个类只有GameSession类会使用到
	/// </summary>
	internal static class WorldFactory
	{
		/// <summary>
		/// 用来创建整个世界
		/// </summary>
		/// <returns>返回拥有全部游戏地点的世界</returns>
		internal static World CreateWorld()
		{
			World newWorld = new World();
            newWorld.AddLocation(-2, -1, "Farmer's Field",
                "There are rows of corn growing here, with giant rats hiding between them.",
                "FarmFields.png");

            newWorld.LocationAt(-2, -1).AddMonster(2, 100);

            newWorld.AddLocation(-1, -1, "Farmer's House",
                "This is the house of your neighbor, Farmer Ted.",
                "Farmhouse.png");

            newWorld.AddLocation(0, -1, "Home",
                "This is your home",
                "Home.png");

            newWorld.AddLocation(-1, 0, "Trading Shop",
                "The shop of Susan, the trader.",
                "Trader.png");

            newWorld.AddLocation(0, 0, "Town square",
                "You see a fountain here.",
                "TownSquare.png");

            newWorld.AddLocation(1, 0, "Town Gate",
                "There is a gate here, protecting the town from giant spiders.",
                "TownGate.png");

            newWorld.AddLocation(2, 0, "Spider Forest",
                "The trees in this forest are covered with spider webs.",
                "SpiderForest.png");

            newWorld.LocationAt(2, 0).AddMonster(3, 100);

            newWorld.AddLocation(0, 1, "Herbalist's hut",
                "You see a small hut, with plants drying from the roof.",
                "HerbalistsHut.png");

            newWorld.LocationAt(0, 1).QuestsAvailableHere.Add(QuestFactory.GetQuestByID(1));

            newWorld.AddLocation(0, 2, "Herbalist's garden",
                "There are many plants here, with snakes hiding behind them.",
                "HerbalistsGarden.png");

            newWorld.LocationAt(0, 2).AddMonster(1, 100);

            return newWorld;
		}
	}
}
