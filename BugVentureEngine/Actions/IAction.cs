using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugVentureEngine.Models;

namespace BugVentureEngine.Actions
{
	public interface IAction
	{
		event EventHandler<string> OnActionPerformed;
		void Execute(LivingEntity actor, LivingEntity target);
	}
}
