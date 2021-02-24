using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BugVentureEngine.Models;
using BugVentureEngine.ViewModels;

namespace BugVentureUI
{
	/// <summary>
	/// Interaction logic for TradeScreen.xaml
	/// </summary>
	public partial class TradeScreen : Window
	{
		// It is the DataContext object, cast as a GameSession object. This is how we’ll reference the GameSession object
		public GameSession session => DataContext as GameSession;

		public TradeScreen()
		{
			InitializeComponent();
		}

		public void OnClick_Sell(object sender, RoutedEventArgs e)
		{
			// 获取当前需要出售的物品
			// get the item that sent the click event 
			// (the row in the datagrid where the user clicked the buy or sell button)
			// and cast it as a GameItem object.
			GameItem item = ((FrameworkElement)sender).DataContext as GameItem;

			if (item != null)
			{
				session.CurrentPlayer.Gold += item.Price;
				session.CurrentTrader.AddItemToInventory(item);
				session.CurrentPlayer.RemoveItemFromInventory(item);
			}
		}

		public void OnClick_Buy(object sender, RoutedEventArgs e)
		{
			GameItem item = ((FrameworkElement)sender).DataContext as GameItem;

			if (item != null)
			{
				if (session.CurrentPlayer.Gold >= item.Price)
				{
					session.CurrentPlayer.Gold -= item.Price;
					session.CurrentTrader.RemoveItemFromInventory(item);
					session.CurrentPlayer.AddItemToInventory(item);
				}
				else
				{
					MessageBox.Show("You do not have enough gold.");
				}
			}
		}

		private void OnClick_Close(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
