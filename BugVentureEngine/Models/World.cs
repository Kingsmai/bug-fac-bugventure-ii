using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugVentureEngine.Models
{
	public class World
	{
		private List<Location> _locations = new List<Location>();

		/// <summary>
		/// 在世界里面增加新的地点
		/// </summary>
		/// <param name="xCoordinates">纵坐标</param>
		/// <param name="yCoordinates">横坐标</param>
		/// <param name="name">地点名称</param>
		/// <param name="description">地点介绍</param>
		/// <param name="imageName">地点图标路径</param>
		internal void AddLocation(int xCoordinates, int yCoordinates, string name, string description, string imageName)
		{
			Location loc = new Location();
			loc.XCoordinate = xCoordinates;
			loc.YCoordinate = yCoordinates;
			loc.Name = name;
			loc.Description = description;
			loc.ImageName = imageName;

			_locations.Add(loc);
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
