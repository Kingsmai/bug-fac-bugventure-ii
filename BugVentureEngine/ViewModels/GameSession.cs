using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugVentureEngine.Models;

/// <summary>
/// ViewModel旨在管理视图和模型之间如何通信。 
///		- 我们可以使用此类来管理不同的模型。 例如，当玩家在战斗中时，我们将需要管理一个Player对象和一个Monster对象。 
///		- 通过增加class，我们可以获得创建自动化测试的能力。 因此，我们无需手动（缓慢地）测试我们的程序。
///		- 这也使我们能够轻松创建不同的UI –将程序转换为Web游戏或文本游戏。
/// </summary>
namespace BugVentureEngine.ViewModels
{
	public class GameSession
	{
		public Player CurrentPlayer { get; set; }
		public Location CurrentLocation { get; set; }

		public GameSession()
		{
			CurrentPlayer = new Player();
			CurrentPlayer.Name = "Xiaomai";
			CurrentPlayer.CharacterClass = "Fighter";
			CurrentPlayer.HitPoints = 10;
			CurrentPlayer.Gold = 1000000;
			CurrentPlayer.ExperiencePoints = 0;
			CurrentPlayer.Level = 1;

			CurrentLocation = new Location();
			CurrentLocation.Name = "Home";
			CurrentLocation.XCoordinate = 0;
			CurrentLocation.YCoordinate = -1;
			CurrentLocation.Description = "This is your house";
			// /assemblyName;component/path/to/image.png; 
			CurrentLocation.ImageName = "/BugVentureEngine;component/Images/Locations/Home.png";
		}
	}
}
