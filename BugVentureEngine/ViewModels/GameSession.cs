using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BugVentureEngine.Models;

namespace BugVentureEngine.ViewModels
{
	class GameSession
	{
		Player CurrentPlayer { get; set; }

		public GameSession()
		{
			CurrentPlayer = new Player();
			CurrentPlayer.Name = "Xiaomai";
			CurrentPlayer.Gold = 1000000;
		}
	}
}
