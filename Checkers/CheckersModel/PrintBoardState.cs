using System;
using System.IO;
using System.Text;

namespace CheckersModel
{
    public class PrintBoardState
    {
        /// <summary>
        ///     Write the board state to the console
        /// </summary>
        /// <param name="board">The board</param>
        public void DrawBoard(Board board)
        {
            drawBoard(board, Console.Out);
        }

        /// <summary>
        /// Print Board in Console
        /// </summary>
        /// <param name="board"></param>
        /// <param name="writer"></param>
        private void drawBoard(Board board, TextWriter writer)
        {
            var buf1 = new StringBuilder();
            var buf2 = new StringBuilder();
            writer.WriteLine("+--------------------------------+       +--------------------------------+");
            for (int k = 1; k <= 32; k++)
            {
                for (int i = 8; i > 0; i--)
                {
                    int j = 1;
                    if (i%2 == 0)
                    {
                        j = 2;
                        buf1.AppendFormat("    ");
                    }
                    int shift;
                    for (shift = 3; (j <= 8 && shift >= 0); j += 2, shift--)
                    {
                        int cellNum = i*4 - shift;
                        var coord = board[i, j];
                        string soldierColor;
                        if (board.IsBlack(coord))
                        {
                            soldierColor = "b";
                            if (board.IsKing(coord)) soldierColor = "bk";
                        }
                        else if (board.IsWhite(coord))
                        {
                            soldierColor = "w";
                            if (board.IsKing(coord)) soldierColor = "wk";
                        }
                        else
                        {
                            soldierColor = ".";
                        }
                        k++;
                        buf1.AppendFormat(" | {0} |  ", soldierColor);
                        if (cellNum >= 1 && cellNum <= 9)
                        {
                            buf2.AppendFormat("  | {0} | ", cellNum);
                        }
                        else
                        {
                            buf2.AppendFormat("  | {0}| ", cellNum);
                        }
                    }
                    writer.WriteLine("{0}        {1}", buf1, buf2);
                    writer.WriteLine("+--------------------------------+       +--------------------------------+");
                    buf1.Length = 0;
                    buf2.Length = 0;
                }
            }
            buf1.Append("|");
            buf2.Append("|");
        }
    }
}