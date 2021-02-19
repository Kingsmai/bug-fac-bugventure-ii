using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using BugVentureEngine.Models;
using BugVentureEngine.Factories;

/// <summary>
/// ViewModel旨在管理视图和模型之间如何通信。 
///		- 我们可以使用此类来管理不同的模型。 例如，当玩家在战斗中时，我们将需要管理一个Player对象和一个Monster对象。 
///		- 通过增加class，我们可以获得创建自动化测试的能力。 因此，我们无需手动（缓慢地）测试我们的程序。
///		- 这也使我们能够轻松创建不同的UI –将程序转换为Web游戏或文本游戏。
/// </summary>
namespace BugVentureEngine.ViewModels
{
	public class GameSession : INotifyPropertyChanged
	{
		private Location _currentLocation;

		public World CurrentWorld { get; set; }
		public Player CurrentPlayer { get; set; }
		public Location CurrentLocation
		{
			get { return _currentLocation; }
			set
			{
				_currentLocation = value;
				OnPropertyChanged("CurrentLocation");
				OnPropertyChanged("HasLocationToNorth");
				OnPropertyChanged("HasLocationToEast");
				OnPropertyChanged("HasLocationToWest");
				OnPropertyChanged("HasLocationToSouth");
			}
		}
		public bool HasLocationToNorth { get { return CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1) != null; } }
		public bool HasLocationToEast { get { return CurrentWorld.LocationAt(CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate) != null; } }
		public bool HasLocationToSouth { get { return CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1) != null; } }
		public bool HasLocationToWest { get { return CurrentWorld.LocationAt(CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate) != null; } }

		public GameSession()
		{
			CurrentPlayer = new Player();
			CurrentPlayer.Name = "Xiaomai";
			CurrentPlayer.CharacterClass = "Fighter";
			CurrentPlayer.HitPoints = 10;
			CurrentPlayer.Gold = 1000000;
			CurrentPlayer.ExperiencePoints = 0;
			CurrentPlayer.Level = 1;

			WorldFactory factory = new WorldFactory();
			CurrentWorld = factory.CreateWorld();

			CurrentLocation = CurrentWorld.LocationAt(0, -1);
		}

		public void MoveNorth()
		{
			CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1);
		}

		public void MoveEast()
		{
			CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate);
		}

		public void MoveSouth()
		{
			CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1);
		}

		public void MoveWest()
		{
			CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate);
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
