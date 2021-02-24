using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugVentureEngine.Factories;

namespace BugVentureEngine.Models
{
	public class Location
	{
		public int XCoordinate { get; set; }
		public int YCoordinate { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string ImageName { get; set; }
		public List<Quest> QuestsAvailableHere { get; set; } = new List<Quest>();
		public List<MonsterEncounter> MonstersHere { get; set; } = new List<MonsterEncounter>();
		public Trader TraderHere { get; set; }

		public void AddMonster(int monsterID, int chanceOfEncountering)
		{
			if (MonstersHere.Exists(m => m.MonsterID == monsterID))
			{
				// 这个怪物已经被加入这个地点，所以覆盖ChanceOfEncountering的值
				MonstersHere.First(m => m.MonsterID == monsterID).ChanceOfEncountering = chanceOfEncountering;
			}
			else
			{
				// 这个怪物不在这个地点里，所以把他添加进去
				MonstersHere.Add(new MonsterEncounter(monsterID, chanceOfEncountering));
			}
		}

		public Monster GetMonster()
		{
			if (!MonstersHere.Any())
			{
				return null;
			}

			// 所有在这个地点的怪物的总出现概率
			int totalChances = MonstersHere.Sum(m => m.ChanceOfEncountering);

			// 从 1 - 总概率（总概率不一定是100） 中随机一个号码
			int randomNumber = RandomNumberGenerator.NumberBetween(1, totalChances);

			// 遍历MonstersHere列表，将列表里怪物的出现率加到runningTotal变量里，
			// 当随机数小于runningTotal，那么就返回该怪物
			int runningTotal = 0;

			foreach (MonsterEncounter monsterEncounter in MonstersHere)
			{
				runningTotal += monsterEncounter.ChanceOfEncountering;

				if (randomNumber <= runningTotal)
				{
					return MonsterFactory.GetMonster(monsterEncounter.MonsterID);
				}
			}

			// 如果有问题，返回列表里最后一个怪物
			return MonsterFactory.GetMonster(MonstersHere.Last().MonsterID);
		}
	}
}
