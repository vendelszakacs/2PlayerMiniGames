using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Controls;
namespace _2PlayerMiniGames;

public partial class WasdArrowsWindow : Window
{
    private const double Step = 10;
    private int _redScore;
    private int _blueScore;

    public WasdArrowsWindow()
    {
        InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        ResetPositions();
        UpdateScoreText();
        Focus();
    }

    private void ResetPositions()
    {
        var width = GameCanvas.ActualWidth;
        var height = GameCanvas.ActualHeight;

        if (width <= 0 || height <= 0)
        {
            width = GameCanvas.Width > 0 ? GameCanvas.Width : 400;
            height = GameCanvas.Height > 0 ? GameCanvas.Height : 250;
        }

        Canvas.SetLeft(RedPlayer, 10);
        Canvas.SetTop(RedPlayer, height / 2 - RedPlayer.Height / 2);

        Canvas.SetLeft(BluePlayer, width - BluePlayer.Width - 10);
        Canvas.SetTop(BluePlayer, height / 2 - BluePlayer.Height / 2);

        StatusText.Text = "Piros: WASD, Kék: nyilak. Cél: érj a másik oldalra!";
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        // Ne mozduljon folyamatosan, ha lenyomva tartjuk: csak egyszer lépjen
        if (e.IsRepeat)
            return;

        MovePlayers(e.Key);
    }

    private void MovePlayers(Key key)
    {
        MoveWithKeys(RedPlayer, key, Key.W, Key.S, Key.A, Key.D);
        MoveWithKeys(BluePlayer, key, Key.Up, Key.Down, Key.Left, Key.Right);

        CheckWinCondition();
    }

    private void MoveWithKeys(Rectangle player, Key pressed, Key up, Key down, Key left, Key right)
    {
        double x = Canvas.GetLeft(player);
        double y = Canvas.GetTop(player);

        if (pressed == up) y -= Step;
        if (pressed == down) y += Step;
        if (pressed == left) x -= Step;
        if (pressed == right) x += Step;

        x = Clamp(x, 0, GameCanvas.ActualWidth - player.Width);
        y = Clamp(y, 0, GameCanvas.ActualHeight - player.Height);

        Canvas.SetLeft(player, x);
        Canvas.SetTop(player, y);
    }

    private static double Clamp(double value, double min, double max)
    {
        if (value < min) return min;
        if (value > max) return max;
        return value;
    }

    private void CheckWinCondition()
    {
        var redX = Canvas.GetLeft(RedPlayer);
        var blueX = Canvas.GetLeft(BluePlayer);

        if (redX + RedPlayer.Width >= GameCanvas.ActualWidth - 1)
        {
            _redScore++;
            StatusText.Text = "Piros nyert! (WASD)";
            UpdateScoreText();
            ResetPositions();
        }
        else if (blueX <= 1)
        {
            _blueScore++;
            StatusText.Text = "Kék nyert! (nyilak)";
            UpdateScoreText();
            ResetPositions();
        }
    }

    private void UpdateScoreText()
    {
        ScoreText.Text = $"Eredmény - Piros: {_redScore} | Kék: {_blueScore}";
    }

    private void NewGame_Click(object sender, RoutedEventArgs e)
    {
        ResetPositions();
    }
}

