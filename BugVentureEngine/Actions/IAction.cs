using BugVentureEngine.Models;
using System;

namespace BugVentureEngine.Actions
{
	public interface IAction
	{
		event EventHandler<string> OnActionPerformed;
		void Execute(LivingEntity actor, LivingEntity target);
	}
}
