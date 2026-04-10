using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace _2PlayerMiniGames;

public partial class WasdArrowsWindow : Window
{
    private const double Step = 40;
    private const int DefaultTime = 30;

    private int _redScore;
    private int _blueScore;

    private int _timeLeft = DefaultTime;
    private bool _isGameRunning = true;
    private DispatcherTimer _gameTimer;

    public WasdArrowsWindow()
    {
        InitializeComponent();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        _gameTimer = new DispatcherTimer();
        _gameTimer.Interval = TimeSpan.FromSeconds(1);
        _gameTimer.Tick += Timer_Tick;
        _gameTimer.Start();

        StartNewSession();
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
        _timeLeft--;
        TimerText.Text = $"Hátralévő idő: {_timeLeft} másodperc";

        if (_timeLeft <= 5)
        {
            TimerText.Foreground = Brushes.Red;
            TimerText.FontSize = 40;
        }

        if (_timeLeft <= 0)
        {
            DetermineWinner();
        }
    }

    private void DetermineWinner()
    {
        string winnerMessage = "Idő lejárt! ";
        if (_redScore > _blueScore) winnerMessage += "Piros a végső győztes!";
        else if (_blueScore > _redScore) winnerMessage += "Kék a végső győztes!";
        else winnerMessage += "Döntetlen!";

        EndGame(winnerMessage);
    }

    private void EndGame(string message)
    {
        _isGameRunning = false;
        _gameTimer.Stop();

        StatusText.Text = "JÁTÉK VÉGE!";
        StatusText.Foreground = Brushes.Yellow;
        TimerText.Text = message;
        TimerText.FontSize = 24;

        GameCanvas.Background = new SolidColorBrush(Color.FromRgb(30, 30, 30));
    }

    private void ResetPositions()
    {
        var width = GameCanvas.ActualWidth;
        var height = GameCanvas.ActualHeight;

        if (width <= 0 || height <= 0)
        {
            width = GameCanvas.Width > 0 ? GameCanvas.Width : 800;
            height = GameCanvas.Height > 0 ? GameCanvas.Height : 600;
        }

        Canvas.SetLeft(RedPlayer, 20);
        Canvas.SetTop(RedPlayer, height / 2 - RedPlayer.Height / 2);

        Canvas.SetLeft(BluePlayer, width - BluePlayer.Width - 20);
        Canvas.SetTop(BluePlayer, height / 2 - BluePlayer.Height / 2);

    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        // Ne mozduljon folyamatosan, ha lenyomva tartjuk: csak egyszer lépjen
        if (e.IsRepeat || !_isGameRunning)
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

        if (redX + RedPlayer.Width >= GameCanvas.ActualWidth - 5)
        {
            _redScore++;
            UpdateScoreText();
            ResetPositions();
        }
        else if (blueX <= 5)
        {
            _blueScore++;
            UpdateScoreText();
            ResetPositions();
        }
    }

    private void StartNewSession()
    {
        _isGameRunning = true;
        _timeLeft = DefaultTime;

        TimerText.Text = $"Hátralévő idő: {_timeLeft} másodperc";
        TimerText.Foreground = Brushes.White;
        TimerText.FontSize = 26;
        StatusText.Text = "Piros: WASD, Kék: nyilak. Cél: érj a másik oldalra!";
        StatusText.Foreground = Brushes.White;
        GameCanvas.Background = new SolidColorBrush(Color.FromRgb(40, 44, 52));

        ResetPositions();
        UpdateScoreText();
        Focus();

        _gameTimer.Start();
    }

    private void UpdateScoreText()
    {
        ScoreText.Text = $"Eredmény - Piros: {_redScore} | Kék: {_blueScore}";
    }

    private void NewGame_Click(object sender, RoutedEventArgs e)
    {
        _redScore = 0;
        _blueScore = 0;
        StartNewSession();
    }
}

