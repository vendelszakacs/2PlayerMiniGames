using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace _2PlayerMiniGames;

public partial class TicTacToeWindow : Window
{
    private readonly Button[,] _cells;
    private readonly Queue<Button> _xMoves = new();
    private readonly Queue<Button> _oMoves = new();
    private char _currentPlayer = 'X';
    private bool _gameOver;

    public TicTacToeWindow()
    {
        InitializeComponent();

        _cells = new[,]
        {
            { Cell00, Cell01, Cell02 },
            { Cell10, Cell11, Cell12 },
            { Cell20, Cell21, Cell22 }
        };
    }

    private void Update_Highlight()
    {
        foreach (var cell in _cells)
        {
            var content = cell.Content?.ToString();

            if (content == "X")
            {
                cell.Foreground = Brushes.Blue; 
            }
            else if (content == "O")
            {
                cell.Foreground = Brushes.Red;
            }
            else
            {
                
                cell.Foreground = Brushes.Black;
            }

            cell.Opacity = 1.0;
        }

        if (!_gameOver)
        {
            if (_xMoves.Count == 3)
            {
                var oldestX = _xMoves.Peek();
                var fadedBlue = new SolidColorBrush(Colors.Blue) { Opacity = 0.4 };
                fadedBlue.Freeze();
                oldestX.Foreground = fadedBlue;
            }

            if (_oMoves.Count == 3)
            {
                var oldestO = _oMoves.Peek();
                var fadedRed = new SolidColorBrush(Colors.Red) { Opacity = 0.4 };
                fadedRed.Freeze(); 
                oldestO.Foreground = fadedRed;
            }
        }
    }

    private void Cell_Click(object sender, RoutedEventArgs e)
    {
        if (_gameOver) return;
        if (sender is not Button button) return;
        if (!string.IsNullOrEmpty(button.Content?.ToString())) return;

        var currentQueue = _currentPlayer == 'X' ? _xMoves : _oMoves;

        Button? removed = null;

        if (currentQueue.Count >= 3)
        {
            removed = currentQueue.Dequeue();
            removed.Content = string.Empty;
        }

        button.Content = _currentPlayer;
        currentQueue.Enqueue(button);

        Update_Highlight();

        if (CheckWin(_currentPlayer))
        {
            StatusText.Text = $"Nyert: {_currentPlayer}";  
            _gameOver = true;
            Update_Highlight(); 
            return;
        }

        _currentPlayer = _currentPlayer == 'X' ? 'O' : 'X';
        StatusText.Text = $"Következő: {_currentPlayer}";

        Update_Highlight();
    }

    private bool CheckWin(char player)
    {
        for (int i = 0; i < 3; i++)
        {
            if (CellMark(i, 0) == player && CellMark(i, 1) == player && CellMark(i, 2) == player)
                return true;
            if (CellMark(0, i) == player && CellMark(1, i) == player && CellMark(2, i) == player)
                return true;
        }

        if (CellMark(0, 0) == player && CellMark(1, 1) == player && CellMark(2, 2) == player)
            return true;
        if (CellMark(0, 2) == player && CellMark(1, 1) == player && CellMark(2, 0) == player)
            return true;

        return false;
    }

    private char CellMark(int row, int col)
    {
        var content = _cells[row, col].Content?.ToString();
        return string.IsNullOrEmpty(content) ? '-' : content[0];
    }



    private void NewGame_Click(object sender, RoutedEventArgs e)
    {
        foreach (var cell in _cells)
        {
            cell.Content = string.Empty;
        }

        _xMoves.Clear();
        _oMoves.Clear();

        _currentPlayer = 'X';
        _gameOver = false;
        StatusText.Text = "Következő: X";
    }
}

