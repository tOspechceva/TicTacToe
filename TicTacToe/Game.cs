using System;
using System.Collections.Generic;

namespace TicTacToe
{
    public class Game
    {
        // Игроки
        public enum Player { None, X, O }

        // Текущий игрок
        public Player CurrentPlayer { get; set; } = Player.None;

        // Игровое поле 3x3 (значения: "", "X", "O")
        public List<List<string>> Board { get; private set; } = new List<List<string>>
        {
            new List<string> { "", "", "" },
            new List<string> { "", "", "" },
            new List<string> { "", "", "" }
        };

        // Событие, вызываемое при победе игрока
        public event Action<Player, List<(int, int)>> OnWin;

        // Выполнить ход
        public bool MakeMove(int row, int col)
        {
            if (Board[row][col] != "")
                return false;

            Board[row][col] = CurrentPlayer.ToString();
            return true;
        }

        // Сменить игрока
        public void SwitchPlayer()
        {
            if (CurrentPlayer == Player.X)
                CurrentPlayer = Player.O;
            else if (CurrentPlayer == Player.O)
                CurrentPlayer = Player.X;
        }

        // Проверка победителя и возврат выигрышной линии (или null)
        public List<(int, int)> CheckWinner()
        {
            // Проверка строк
            for (int i = 0; i < 3; i++)
            {
                if (Board[i][0] != "" &&
                    Board[i][0] == Board[i][1] &&
                    Board[i][1] == Board[i][2])
                {
                    return new List<(int, int)> { (i, 0), (i, 1), (i, 2) };
                }
            }

            // Проверка столбцов
            for (int j = 0; j < 3; j++)
            {
                if (Board[0][j] != "" &&
                    Board[0][j] == Board[1][j] &&
                    Board[1][j] == Board[2][j])
                {
                    return new List<(int, int)> { (0, j), (1, j), (2, j) };
                }
            }

            // Диагонали
            if (Board[0][0] != "" &&
                Board[0][0] == Board[1][1] &&
                Board[1][1] == Board[2][2])
            {
                return new List<(int, int)> { (0, 0), (1, 1), (2, 2) };
            }

            if (Board[0][2] != "" &&
                Board[0][2] == Board[1][1] &&
                Board[1][1] == Board[2][0])
            {
                return new List<(int, int)> { (0, 2), (1, 1), (2, 0) };
            }

            return null;
        }

        // Проверка победы + вызов события
        public List<(int, int)> CheckWinnerAndRaise()
        {
            var winLine = CheckWinner();
            if (winLine != null)
                OnWin?.Invoke(CurrentPlayer, winLine);
            return winLine;
        }

        // Проверка ничьей
        public bool IsDraw()
        {
            foreach (var row in Board)
                foreach (var cell in row)
                    if (cell == "")
                        return false;

            return CheckWinner() == null;
        }

        // Сброс игры
        public void Reset()
        {
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    Board[i][j] = "";

            CurrentPlayer = Player.None;
        }
    }
}
