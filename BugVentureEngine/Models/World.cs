using System.Collections.Generic;

namespace BugVentureEngine.Models
{
	public class World
	{
		private List<Location> _locations = new List<Location>();

		/// <summary>
		/// 在世界里面增加新的地点
		/// </summary>
		internal void AddLocation(Location location)
		{
			_locations.Add(location);
		}

		public Location LocationAt(int xCoordinates, int yCoordinates)
		{
			foreach (Location loc in _locations)
			{
				if (loc.XCoordinate == xCoordinates && loc.YCoordinate == yCoordinates)
				{
					return loc;
				}
			}

			return null;
		}
	}
}
