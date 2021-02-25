namespace BugVentureEngine.Models
{
	public class Monster : LivingEntity
	{
		public string ImageName { get; set; }

		public int MinimumDamage { get; set; }
		public int MaximumDamage { get; set; }

		public int RewardExperiencePoints { get; private set; }

		public Monster(string name, string imageName, int maximumHitPoints, int currentHitPoints,
			int minimumDamage, int maximumdamage,
			int rewardExperiencePoints, int gold)
			: base(name, maximumHitPoints, currentHitPoints, gold)
		{
			ImageName = $"/BugVentureEngine;component/Images/Monsters/{imageName}";
			MinimumDamage = minimumDamage;
			MaximumDamage = maximumdamage;
			RewardExperiencePoints = rewardExperiencePoints;
		}
	}
}
