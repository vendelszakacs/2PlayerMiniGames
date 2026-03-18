using _2PlayerMiniGames;
using System.Windows;

namespace _2PlayerMiniGames;

public partial class MainWindow : Window
{
	public MainWindow()
	{
		InitializeComponent();
	}

	private void TicTacToe_Click(object sender, RoutedEventArgs e)
	{
		var window = new TicTacToeWindow();
		window.Owner = this;
		window.Show();
	}
}

