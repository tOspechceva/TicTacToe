using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class Form1 : Form
    {
        private Game game = new Game(); // Игровая логика

        public Form1()
        {
            InitializeComponent();

            // Назначение координат каждой кнопке через Tag
            button1.Tag = (0, 0);
            button2.Tag = (0, 1);
            button3.Tag = (0, 2);
            button4.Tag = (1, 0);
            button5.Tag = (1, 1);
            button6.Tag = (1, 2);
            button7.Tag = (2, 0);
            button8.Tag = (2, 1);
            button9.Tag = (2, 2);

            // Подписка на событие победы
            game.OnWin += HandleWin;
        }
        
        // Кнопка: Установить игрока X
        private void button11_Click(object sender, EventArgs e)
        {
            game.CurrentPlayer = Game.Player.X;
            textBox1.Text = "Ходит X";
        }

        // Кнопка: Установить игрока O
        private void button12_Click(object sender, EventArgs e)
        {
            game.CurrentPlayer = Game.Player.O;
            textBox1.Text = "Ходит O";
        }

        // Кнопка: Очистить поле
        private void buttonClear_Click(object sender, EventArgs e)
        {
            // Очистить кнопки
            foreach (Control control in this.Controls)
            {
                if (control is Button btn && btn.Tag is ValueTuple<int, int>)
                {
                    btn.Text = "";
                    btn.ForeColor = Color.Black;
                    btn.Enabled = true;
                }
            }

            // Очистить игровое поле и текст
            game.Reset();
            textBox1.Text = "Поле очищено!";
        }

        // Обработка хода игрока
        private void Button_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            var (row, col) = ((int, int))btn.Tag;

            if (btn.Text != "" || game.CurrentPlayer == Game.Player.None)
                return;

            if (!game.MakeMove(row, col))
                return;

            btn.Text = game.CurrentPlayer.ToString();

            // Проверка победы
            var winLine = game.CheckWinnerAndRaise();
            if (winLine != null)
                return;

            // Проверка ничьей
            if (game.IsDraw())
            {
                textBox1.Text = "Ничья!";
                return;
            }

            // Смена игрока
            game.SwitchPlayer();
            textBox1.Text = $"Ходит {game.CurrentPlayer}";
        }

        // Обработчик события победы
        private void HandleWin(Game.Player winner, List<(int, int)> winLine)
        {
            textBox1.Text = $"Победа: {winner}";

            foreach (var (r, c) in winLine)
            {
                var btn = GetButtonAt(r, c);
                if (btn != null)
                    btn.ForeColor = Color.Red;
            }
        }

        // Получить кнопку по координатам
        private Button GetButtonAt(int row, int col)
        {
            foreach (Control control in this.Controls)
            {
                if (control is Button btn && btn.Tag is ValueTuple<int, int> tag)
                {
                    if (tag.Item1 == row && tag.Item2 == col)
                        return btn;
                }
            }
            return null;
        }

        
    }
}
