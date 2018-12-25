using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
    public partial class Form1 : Form
    {
        Cell[,] grid;
        Graphics g;
        Bitmap img;
        int cols, rows;
        int width, height;
        int bombsCount = 20;
        float scale;
        bool gameOver = false;

        private void PictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (!IsGamePassed() && !gameOver)
            {
                int x = (int)Math.Floor(e.X / scale);
                int y = (int)Math.Floor(e.Y / scale);
                if (e.Button == MouseButtons.Left)
                    gameOver = grid[x, y].Show(grid);
                else if (e.Button == MouseButtons.Right)
                {
                    grid[x, y].SetFlag();
                }

                UpdateField();
                if (gameOver)
                {
                    MessageBox.Show("Вы проиграли");
                }
                if (IsGamePassed())
                {
                    ShowAll();
                    MessageBox.Show("Победа!");
                }
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Random r = new Random();
            bombsCount = (int)numericUpDown1.Value;

            grid = new Cell[cols, rows];
            List<int[]> choice = new List<int[]>();
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    grid[i, j] = new Cell(i, j, scale);

                    choice.Add(new int[] { i, j });
                }
            }
            for (int i = 0; i < bombsCount; i++)
            {
                int index = r.Next(0, choice.Count);
                int x = choice[index][0];
                int y = choice[index][1];
                choice.RemoveAt(index);
                grid[x, y].Bomb = true;
            }
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    grid[i, j].Count(grid);


                }
            }
            UpdateField();
        }

        public Form1()
        {
            InitializeComponent();

            height = pictureBox1.Height;
            width = pictureBox1.Width;
            img = new Bitmap(width, height);
            g = Graphics.FromImage(img);
            scale = 24;
            cols = (int)Math.Floor(width / scale);
            rows = (int)Math.Floor(height / scale);
            pictureBox1.Width = (int)(cols * scale);
            pictureBox1.Height = (int)(rows * scale);
            numericUpDown1.Value = bombsCount;
            Button1_Click(null, null);


        }
        void UpdateField()
        {
            g.Clear(Color.Black);

            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    grid[i, j].Draw(g);
                }
            }

            pictureBox1.Image = img;
        }
        bool IsGamePassed()
        {
            
            int markedBombs = 0;
            int flags = 0;
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    if (grid[i, j].Flag)
                        flags++;
                    if (grid[i, j].Bomb && grid[i, j].Flag)
                        markedBombs++;

                }
            }
            return markedBombs == flags && bombsCount == markedBombs;
        }
        void ShowAll()
        {
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    grid[i, j].GameOver = true;
                    grid[i, j].Shown = true;
                }
            }
            UpdateField();
        }
    }
}
