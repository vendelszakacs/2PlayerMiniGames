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
using System.Windows.Threading;

namespace _2PlayerMiniGames
{

    public partial class PongWindow : Window
    {
        private readonly HashSet<Key> pressedKeys = new HashSet<Key>();

        private DispatcherTimer gameTimer;
        private double ballX, ballY;
        private double ballDX, ballDY;
        private const double BaseSpeed = 6;
        private const double PaddleStep = 15;

        private int redScore = 0;
        private int blueScore = 0;
        private bool isGameRunning = false;

        public PongWindow()
        {
            InitializeComponent();

            gameTimer = new DispatcherTimer();
            gameTimer.Interval = TimeSpan.FromMilliseconds(16);
            gameTimer.Tick += GameLoop;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            StartNewGame();
        }

        private void GameLoop(object sender, EventArgs e)
        {
            if (!isGameRunning) return;

            ProcessInput();
            MoveBall();
            CheckCollisions();
        }

        private void ProcessInput()
        {
            double redTop = Canvas.GetTop(RedPaddle);
            double blueTop = Canvas.GetTop(BluePaddle);

            if (pressedKeys.Contains(Key.W) && redTop > 0)
            {
                Canvas.SetTop(RedPaddle, redTop - PaddleStep);
            }
            if (pressedKeys.Contains(Key.S) && redTop < GameCanvas.ActualHeight - RedPaddle.Height)
            {
                Canvas.SetTop(RedPaddle, redTop + PaddleStep);
            }

            if (pressedKeys.Contains(Key.Up) && blueTop > 0)
            {
                Canvas.SetTop(BluePaddle, blueTop - PaddleStep);
            }
            if (pressedKeys.Contains(Key.Down) && blueTop < GameCanvas.ActualHeight - BluePaddle.Height)
            {
                Canvas.SetTop(BluePaddle, blueTop + PaddleStep);
            }
        }

        private void MoveBall()
        {
            ballX += ballDX;
            ballY += ballDY;

            Canvas.SetLeft(Ball, ballX);
            Canvas.SetTop(Ball, ballY);

            if (ballY <= 0 || ballY >= GameCanvas.ActualHeight - Ball.Height)
            {
                ballDY *= -1;
            }

            if (ballX <= 0)
            {
                blueScore++;
                CheckWinCondition("Kék");
            }
            else if (ballX >= GameCanvas.ActualWidth - Ball.Width)
            {
                redScore++;
                CheckWinCondition("Piros");
            }
        }

        private void CheckWinCondition(string lastScorer)
        {
            UpdateScoreDisplay();

            if (redScore >= 10 || blueScore >= 10)
            {
                EndGame($"{lastScorer} nyert!");
            }
            else
            {
                ResetBall();
            }
        }

        private void EndGame(string message)
        {
            isGameRunning = false;
            gameTimer.Stop();
            StatusText.Text = message;
            StatusText.Foreground = Brushes.Yellow;
            GameCanvas.Background = new SolidColorBrush(Color.FromRgb(30, 30, 30));
        }

        private void ResetBall()
        {
            ballX = (GameCanvas.ActualWidth - Ball.Width) / 2;
            ballY = (GameCanvas.ActualHeight - Ball.Height) / 2;

            Random rnd = new Random();
            ballDX = rnd.Next(0, 2) == 0 ? BaseSpeed : -BaseSpeed;
            ballDY = rnd.Next(0, 2) == 0 ? BaseSpeed : -BaseSpeed;
        }

        private void CheckCollisions()
        {
            Rect ballRect = new Rect(Canvas.GetLeft(Ball), Canvas.GetTop(Ball), Ball.Width, Ball.Height);
            Rect redRect = new Rect(Canvas.GetLeft(RedPaddle), Canvas.GetTop(RedPaddle), RedPaddle.Width, RedPaddle.Height);
            Rect blueRect = new Rect(Canvas.GetLeft(BluePaddle), Canvas.GetTop(BluePaddle), BluePaddle.Width, BluePaddle.Height);

            if (ballRect.IntersectsWith(redRect) || ballRect.IntersectsWith(blueRect))
            {
                ballDX *= -1.1;
                if (Math.Abs(ballDX) > 20) ballDX = ballDX > 0 ? 20 : -20;
                ballX += ballDX;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (!isGameRunning) return;

            if (!pressedKeys.Contains(e.Key))
            {
                pressedKeys.Add(e.Key);
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (pressedKeys.Contains(e.Key))
            {
                pressedKeys.Remove(e.Key);
            }
        }

        private void StartNewGame()
        {
            redScore = 0;
            blueScore = 0;
            isGameRunning = true;

            StatusText.Text = "Pong: Első 10 pontig";
            StatusText.Foreground = Brushes.White;
            GameCanvas.Background = new SolidColorBrush(Color.FromRgb(40, 44, 52));

            double height = GameCanvas.ActualHeight > 0 ? GameCanvas.ActualHeight : 450;
            Canvas.SetLeft(RedPaddle, 30);
            Canvas.SetTop(RedPaddle, height / 2 - RedPaddle.Height / 2);
            Canvas.SetLeft(BluePaddle, 740);
            Canvas.SetTop(BluePaddle, height / 2 - BluePaddle.Height / 2);

            UpdateScoreDisplay();
            ResetBall();
            gameTimer.Start();
            this.Focus();
        }

        private void UpdateScoreDisplay()
        {
            ScoreText.Text = $"{redScore} : {blueScore}";
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            StartNewGame();
        }
    }
}
