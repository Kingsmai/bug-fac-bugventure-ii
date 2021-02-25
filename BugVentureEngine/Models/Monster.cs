namespace BugVentureEngine.Models
{
	public class Monster : LivingEntity
	{
		public string ImageName { get; set; }

		public int MinimumDamage { get; set; }
		public int MaximumDamage { get; set; }

		public int RewardExperiencePoints { get; private set; }

		public Monster(string name, string imageName, int maximumHitPoints, int hitPoints,
			int minimumDamage, int maximumdamage,
			int rewardExperiencePoints, int rewardGold)
		{
			Name = name;
			ImageName = $"/BugVentureEngine;component/Images/Monsters/{imageName}";
			MaximumHitPoints = maximumHitPoints;
			CurrentHitPoints = hitPoints;
			MinimumDamage = minimumDamage;
			MaximumDamage = maximumdamage;
			RewardExperiencePoints = rewardExperiencePoints;
			Gold = rewardGold;
		}
	}
}
