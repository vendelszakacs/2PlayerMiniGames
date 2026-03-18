using System.Windows;
using System.Windows.Controls;

namespace _2PlayerMiniGames;

public partial class TicTacToeWindow : Window
{
    private readonly Button[,] _cells;
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

    private void Cell_Click(object sender, RoutedEventArgs e)
    {
        if (_gameOver) return;

        if (sender is not Button button) return;
        if (!string.IsNullOrEmpty(button.Content?.ToString())) return;

        button.Content = _currentPlayer;

        if (CheckWin(_currentPlayer))
        {
            StatusText.Text = $"Nyert: {_currentPlayer}";
            _gameOver = true;
            return;
        }

        if (IsBoardFull())
        {
            StatusText.Text = "Döntetlen!";
            _gameOver = true;
            return;
        }

        _currentPlayer = _currentPlayer == 'X' ? 'O' : 'X';
        StatusText.Text = $"Következő: {_currentPlayer}";
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

    private bool IsBoardFull()
    {
        foreach (var cell in _cells)
        {
            if (string.IsNullOrEmpty(cell.Content?.ToString()))
                return false;
        }

        return true;
    }

    private void NewGame_Click(object sender, RoutedEventArgs e)
    {
        foreach (var cell in _cells)
        {
            cell.Content = string.Empty;
        }

        _currentPlayer = 'X';
        _gameOver = false;
        StatusText.Text = "Következő: X";
    }
}

