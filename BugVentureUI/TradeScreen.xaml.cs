using BugVentureEngine.Models;
using BugVentureEngine.ViewModels;
using System.Windows;

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
			GroupedInventoryItem groupInventoryItem = ((FrameworkElement)sender).DataContext as GroupedInventoryItem;

			if (groupInventoryItem != null)
			{
				session.CurrentPlayer.ReceiveGold(groupInventoryItem.Item.Price);
				session.CurrentTrader.AddItemToInventory(groupInventoryItem.Item);
				session.CurrentPlayer.RemoveItemFromInventory(groupInventoryItem.Item);
			}
		}

		public void OnClick_Buy(object sender, RoutedEventArgs e)
		{
			GroupedInventoryItem groupedInventoryItem = ((FrameworkElement)sender).DataContext as GroupedInventoryItem;

			if (groupedInventoryItem != null)
			{
				if (session.CurrentPlayer.Gold >= groupedInventoryItem.Item.Price)
				{
					session.CurrentPlayer.SpendGold(groupedInventoryItem.Item.Price);
					session.CurrentTrader.RemoveItemFromInventory(groupedInventoryItem.Item);
					session.CurrentPlayer.AddItemToInventory(groupedInventoryItem.Item);
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
