using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        int cols = 50;
        int rows = 25;
        int score = 0;
        int dx = 0;
        int dy = 0;
        int front = 0;
        int back = 0;
        const int MAX_SNAKE_LENGTH = 1250;

        Piece[] snake = new Piece[MAX_SNAKE_LENGTH];
        List<int> available = new List<int>();
        bool[,] visit;

        Random rand = new Random();

        Timer timer = new Timer();

        public Form1()
        {
            InitializeComponent();
            init();
            launchTimer();
        }

        private void init()
        {
            visit = new bool[rows, cols];
            Piece head = new Piece((rand.Next() % cols) * 20, (rand.Next() % rows) * 20);
            lblFood.Location = new Point((rand.Next() % cols) * 20, (rand.Next() % rows) * 20);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    visit[i, j] = false;
                    available.Add(i * cols + j);
                }
            }
            visit[head.Location.Y / 20, head.Location.X / 20] = true;
            available.Remove(head.Location.Y / 20 * cols + head.Location.X / 20);
            Controls.Add(head);
            snake[front] = head;
        }

        private void launchTimer()
        {
            timer.Interval = 100;
            timer.Tick += move;
            timer.Start();
        }

        private void move(object sender, EventArgs e)
        {
            int x = snake[front].Location.X;
            int y = snake[front].Location.Y;
            
            if (dx == 0 && dy == 0)
            {
                return;
            }

            if (isInWall(x + dx, y + dy))
            {
                timer.Stop();
                MessageBox.Show("Game Over!");
                return;
            }

            if (getFood(x + dx, y + dy))
            {
                score += 1;
                lblScore.Text = "Score: " + score.ToString();

                if (hitsBody((y + dy) / 20, (x + dx) / 20))
                {
                    return;
                }

                Piece head = new Piece(x + dx, y + dy);
                front = (front - 1 + MAX_SNAKE_LENGTH) % MAX_SNAKE_LENGTH;
                snake[front] = head;
                visit[head.Location.Y / 20, head.Location.X / 20] = true;
                Controls.Add(head);
                putFoodAtRandom();
            } 
            else
            {
                if (hitsBody((y + dy) / 20, (x + dx) / 20))
                {
                    Console.WriteLine("hogehoge");
                    return;
                }
                visit[snake[back].Location.Y / 20, snake[back].Location.X / 20] = false;
                front = (front - 1 + MAX_SNAKE_LENGTH) % MAX_SNAKE_LENGTH;
                snake[front] = snake[back];
                snake[front].Location = new Point(x + dx, y + dy);
                back = (back - 1 + MAX_SNAKE_LENGTH) % MAX_SNAKE_LENGTH;
                visit[(y + dy) / 20, (x + dx) / 20] = true;
            }
        }

        private bool isInWall(int x, int y)
        {
            return x < 0 || y < 0 || 980 < x || 480 < y;
        }

        private bool getFood(int x, int y)
        {
            return x == lblFood.Location.X && y == lblFood.Location.Y;
        }

        private bool hitsBody(int x, int y)
        {
            if (visit[x, y])
            {
                timer.Stop();
                MessageBox.Show("Snake hits his body!");
                return true;
            }
            return false;
        }

        private void putFoodAtRandom()
        {
            available.Clear();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (!visit[i, j])
                    {
                        available.Add(i * cols + j);
                    }
                }
            }
            int idx = rand.Next(available.Count) % available.Count;
            lblFood.Left = (available[idx] * 20) % Width;
            lblFood.Top = (available[idx] * 20) / Width * 20;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            dx = 0;
            dy = 0;
            switch(e.KeyCode)
            {
                case Keys.Right:
                    dx = 20;
                    break;
                case Keys.Left:
                    dx = -20;
                    break;
                case Keys.Up:
                    dy = -20;
                    break;
                case Keys.Down:
                    dy = 20;
                    break;
            }
        }
    }
}
