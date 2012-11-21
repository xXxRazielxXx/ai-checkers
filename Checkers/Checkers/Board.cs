using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Checkers
{
    public class Board
    {
        private Coordinate[] board = new Coordinate[32];

        /// <summary>
        /// Defualt constructor
        /// </summary>
        public Board()
        {
            for (int i = 0; i < 32; i++)
            {
                board[i] = new Coordinate();
            }
        }

        /// <summary>
        /// Huerisitic board grade
        /// </summary>
        public int Grade { get; set; } 

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
                        board[k].Checker.Queen = false;
                        if (k >= 0 && k <= 11)
                        {
                            board[k].Checker.Status = PlayerColor.White;
                        }
                        else if (k >= 12 && k <= 19)
                        {
                            board[k].Checker.Status = PlayerColor.Empty;
                        }
                        else
                        {
                            board[k].Checker.Status = PlayerColor.Black;
                        }
                        k++;
                    }
                }
            }
        }
    }
}