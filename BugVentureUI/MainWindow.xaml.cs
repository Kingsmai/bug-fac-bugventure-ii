using BugVentureEngine.EventArgs;
using BugVentureEngine.Models;
using BugVentureEngine.ViewModels;
using System.Windows;
using System.Windows.Documents;

namespace BugVentureUI
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly GameSession _gameSession = new GameSession();
		public MainWindow()
		{
			InitializeComponent();

			_gameSession.OnMessageRaised += OnGameMessageRaised; // 添加事件

			DataContext = _gameSession; // 让xaml知道它在跟谁工作
		}

		private void OnClick_MoveNorth(object sender, RoutedEventArgs e)
		{
			_gameSession.MoveNorth();
		}

		private void OnClick_MoveWest(object sender, RoutedEventArgs e)
		{
			_gameSession.MoveWest();
		}

		private void OnClick_MoveEast(object sender, RoutedEventArgs e)
		{
			_gameSession.MoveEast();
		}

		private void OnClick_MoveSouth(object sender, RoutedEventArgs e)
		{
			_gameSession.MoveSouth();
		}

		private void OnClick_AttackMonster(object sender, RoutedEventArgs e)
		{
			_gameSession.AttackCurrentMonster();
		}

		private void OnClick_UseCurrentConsumable(object sender, RoutedEventArgs e)
		{
			_gameSession.UseCurrentConsumable();
		}

		private void OnClick_DisplayTradeScreen(object sender, RoutedEventArgs e)
		{
			TradeScreen tradeScreen = new TradeScreen();
			tradeScreen.Owner = this;
			tradeScreen.DataContext = _gameSession;
			tradeScreen.ShowDialog();
		}

		private void OnClick_Craft(object sender, RoutedEventArgs e)
		{
			Recipe recipe = ((FrameworkElement)sender).DataContext as Recipe;
			_gameSession.CraftItemUsing(recipe);
		}

		//将信息显示在信息栏里
		private void OnGameMessageRaised(object sender, GameMessageEventArgs e)
		{
			GameMessages.Document.Blocks.Add(new Paragraph(new Run(e.Message)));
			GameMessages.ScrollToEnd();
		}
	}
}
