using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Minesweeper
{
    public class Cell
    {
        private readonly int i;
        private readonly int j;
        private readonly float x;
        private readonly float y;
        private readonly float w;
        public bool Shown { get; set; }
        public bool Bomb { get; set; }
        public bool Flag { get; set; }
        public bool GameOver { get; set; } = false;
        public int BombsNear { get; private set; }
        public Cell(int i,int j,float w)
        {
            this.i = i;
            this.j = j;
            x = i * w;
            y = j * w;
            this.w = w;
            Shown = false;
            Bomb = false;
        }
        public void SetFlag() { if (!Shown) Flag = !Flag; else if(!GameOver) Flag = false; }
        public bool Show(Cell[,]grid)
        {
            if (!Flag)
            {
                if (BombsNear == 0)
                {
                    for (int i = -1; i <= 1; i++)
                    {
                        int xoff = i + this.i;
                        if (xoff < 0 || xoff == grid.GetLength(0)) continue;
                        for (int j = -1; j <= 1; j++)
                        {
                            int yoff = j + this.j;
                            if (yoff < 0 || yoff == grid.GetLength(1)) continue;
                            if (!grid[xoff, yoff].Shown)
                            {
                                Shown = true;
                                grid[xoff, yoff].Show(grid);
                            }
                        }
                    }
                }
                else if (BombsNear > 0)
                {
                    for (int i = -1; i <= 1; i++)
                    {
                        int xoff = i + this.i;
                        if (xoff < 0 || xoff == grid.GetLength(0)) continue;
                        for (int j = -1; j <= 1; j++)
                        {
                            int yoff = j + this.j;
                            if (yoff < 0 || yoff == grid.GetLength(1)) continue;
                            if (!grid[xoff, yoff].Shown)
                            {
                                Shown = true;

                            }
                        }
                    }
                }
                else //game over
                {
                    for (int i = 0; i < grid.GetLength(0); i++)
                    {
                        for (int j = 0; j < grid.GetLength(1); j++)
                        {
                            grid[i, j].Shown = true;
                            grid[i, j].GameOver = true;
                        }

                    }
                    return true;
                }
            }
            return false;
        }

        public void Count(Cell[,]grid)
        {
            if (Bomb) { BombsNear = -1;return; }

            for (int i = -1; i <= 1; i++)
            {
                int xoff = i + this.i;
                if (xoff < 0 || xoff == grid.GetLength(0)) continue;
                for (int j = -1; j <= 1; j++)
                {
                    int yoff = j + this.j;
                    if (yoff < 0 || yoff == grid.GetLength(1)) continue;
                    if (grid[xoff, yoff].Bomb)
                    {
                        BombsNear++;
                    }
                    
                }
            }
        }

        public void Draw(Graphics g)
        {
            float xCenter = x + w * 0.2f, yCenter = y + w * 0.2f;
            if (!Shown && !Flag)
            {
                g.FillRectangle(Brushes.Gray, x, y, w - 1, w - 1);
                
            }
            if(!Flag && Shown)
            {
               
                if (!Bomb)
                {
                    g.FillRectangle(Brushes.White, x, y, w - 1, w - 1);
                    if (BombsNear > 0)
                        g.DrawString(BombsNear.ToString(), new Font(FontFamily.GenericSerif, 14), Brushes.Black, xCenter - 1, yCenter - 5);
                }
                else if (Bomb)
                {
                    g.FillRectangle(Brushes.White, x, y, w - 1, w - 1);
                    g.FillEllipse(Brushes.Red, xCenter, yCenter, w * 0.5f, w * 0.5f);
                }
                
            }else if (Flag)
                {
                g.FillRectangle(Brushes.Gray, x, y, w - 1, w - 1);
                g.FillPolygon(Brushes.SkyBlue, new PointF[] { new PointF(x, y), new PointF(x, y+w*0.8f), new PointF(x+w*0.8f, y) });
                }
            if (GameOver)
            {
                if (Flag && Bomb)
                {
                    g.FillRectangle(Brushes.White, x, y, w - 1, w - 1);
                    g.FillEllipse(Brushes.Green, xCenter, yCenter, w * 0.5f, w * 0.5f);
                }
                if (Flag && !Bomb)
                {
                    g.FillRectangle(Brushes.White, x, y, w - 1, w - 1);
                    g.FillEllipse(Brushes.Black, xCenter, yCenter, w * 0.5f, w * 0.5f);
                }
            }
        }
    }
}
