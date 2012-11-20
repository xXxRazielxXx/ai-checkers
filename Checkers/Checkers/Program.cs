using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Checkers
{
    internal class Program
    {
        private static void Main()
        {
            var board = new Coord[32];
            for (int i = 0; i < 32; i++)
            {
                board[i] = new Coord();
            }
            InitializeBoard(board, 8);
        }

        private static void InitializeBoard(Coord[] board, int size)
        {
            int k = 0;
            for (; k <= 31; k++)
            {
                for (int i = 1; i <= size; i++)
                {
                    int j = i%2 == 0 ? 2 : 1;
                    for (; j <= size; j += 2)
                    {
                        board[k].X = i;
                        board[k].Y = j;
                        board[k].King = false;
                        if (k >= 0 && k <= 11)
                        {
                            board[k].status = CoordStatus.White;
                        }
                        else if (k >= 12 && k <= 19)
                        {
                            board[k].status = CoordStatus.Empty;
                        }
                        else
                        {
                            board[k].status = CoordStatus.Black;
                        }
                        k++;
                    }
                }
            }
        }
    }
}


