using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Checkers
{
    public class Board
    {
        private Coord[] board = new Coord[32];

        /// <summary>
        /// Defualt constructor
        /// </summary>
        public Board()
        {
            for (int i = 0; i < 32; i++)
            {
                board[i] = new Coord();
            }
        }

        /// <summary>
        /// Initialize Board
        /// </summary>
        /// <param name="size"></param>
        public void InitializeBoard(int size)
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
                            board[k].Status = CoordStatus.White;
                        }
                        else if (k >= 12 && k <= 19)
                        {
                            board[k].Status = CoordStatus.Empty;
                        }
                        else
                        {
                            board[k].Status = CoordStatus.Black;
                        }
                        k++;
                    }
                }
            }
        }
    }
}