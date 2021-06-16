using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugVentureEngine.Models
{
	class ItemPercentage
	{
		public int ID { get; }
		public int Percentage { get; }

		public ItemPercentage(int id, int percentage)
		{
			ID = id;
			Percentage = percentage;
		}
	}
}
