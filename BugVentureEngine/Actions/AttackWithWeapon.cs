using System;
using BugVentureEngine.Models;

namespace BugVentureEngine.Actions
{
	public class AttackWithWeapon : IAction
	{
		private readonly GameItem _weapon;
		private readonly int _minimumDamage;
		private readonly int _maximumDamage;

		public event EventHandler<string> OnActionPerformed;

		public AttackWithWeapon(GameItem weapon, int minimumDamage, int maximumDamage)
		{
			if (weapon.Category != GameItem.ItemCategory.Weapon)
			{
				throw new ArgumentException($"{weapon.Name} is not a weapon.");
			}

			if (minimumDamage < 0)
			{
				throw new ArgumentException("minimumDamage must be 0 or larger.");
			}

			if (maximumDamage < minimumDamage)
			{
				throw new ArgumentException("maximumDamage must be >= minimumDamage.");
			}

			_weapon = weapon;
			_minimumDamage = minimumDamage;
			_maximumDamage = maximumDamage;
		}

		public void Execute(LivingEntity actor, LivingEntity target)
		{
			int damage = RandomNumberGenerator.NumberBetween(_minimumDamage, _maximumDamage);

			string actorName = (actor is Player) ? "You" : $"The {actor.Name.ToLower()}";
			string targetName = (target is Player) ? "you" : $"the {target.Name.ToLower()}";

			if (damage == 0)
			{
				ReportResult($"{actorName} missed {targetName}.");
			}
			else
			{
				ReportResult($"{actorName} hit {targetName} for {damage} point{(damage > 1 ? "s" : "")}.");
				target.TakeDamage(damage); // 先显示信息再承伤，因为死亡会触发事件
			}
		}

		private void ReportResult(string result)
		{
			OnActionPerformed?.Invoke(this, result);
		}
	}
}
