using System;
using System.Globalization;
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

        public void drawBoard(Board board, TextWriter writer)
        {
            var buf1 = new StringBuilder();
            var buf2 = new StringBuilder();            
            writer.WriteLine("+--------------------------------+       +--------------------------------+");
            int cellNum = 0;
            int shift = 0;
            for (int k = 1; k <= 32; k++)
            {
                for (int i = 8; i >0; i--)
                {
                    var soldierColor = "";
                    int j = 1;
                    if (i % 2 == 0)
                    {
                        j = 2;
                        buf1.AppendFormat("    ");
                    }
                    for (shift=3 ; (j <= 8 && shift>=0); j += 2, shift--)
                    {
                        cellNum = i * 4 - shift;
                        var coord = board[i, j];
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
                        buf1.AppendFormat(" | {0} |  ",soldierColor);
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

        ///// <summary>
        /////     Write the board state to the console
        ///// </summary>
        ///// <param name="board">The board</param>
        ///// <param name="writer">The writer</param>
        //public void DrawBoard(Board board, TextWriter writer)
        //{
        //    var buf1 = new StringBuilder();
        //    var buf2 = new StringBuilder();
        //    bool darkSquare = false;
        //    writer.WriteLine("+-----------------------+   +-----------------------+");

        //    buf1.Append("|  ");
        //    buf2.Append("|  ");
        //    int col = 1;
        //    for (int pos = board.Size; pos >= 1; pos--)
        //    {
        //        String ch = string.Empty;

        //        if ((col != 0) && (col%8 == 0))
        //        {
        //            col = 0;
        //            pos++;
        //            darkSquare = !darkSquare;
        //            buf1.Append("|");
        //            buf2.Append("|");                    
        //            writer.WriteLine("{0}   {1}", buf1, buf2);
        //            buf1.Length = 0;
        //            buf2.Length = 0;
        //            writer.WriteLine("+-----------------------+   +-----------------------+");
        //        }
        //        else
        //        {
        //            col++;
        //            darkSquare = !darkSquare;

        //            if (!darkSquare)
        //            {
        //                ch = "..";
        //                pos++;
        //            }
        //            else
        //            {
        //                Piece piece = board[pos].Status;
        //                switch (piece)
        //                {
        //                    case Piece.None:
        //                        ch = " ";
        //                        break;
        //                    case Piece.BlackPiece:
        //                        ch = "b";
        //                        break;
        //                    case Piece.WhitePiece:
        //                        ch = "w";
        //                        break;
        //                    case Piece.BlackKing:
        //                        ch = "B";
        //                        break;
        //                    case Piece.WhiteKing:
        //                        ch = "W";
        //                        break;
        //                }
        //            }

        //            buf1.AppendFormat("|{0,2}", ch);
        //            buf2.AppendFormat("|{0,2}", (darkSquare) ? pos.ToString(CultureInfo.CurrentCulture) : " ");
        //        }
        //    }

        //    buf1.Append("|");
        //    buf2.Append("|");
        //    writer.WriteLine("{0}  |   {1}  |", buf1, buf2);
        //    writer.WriteLine("+-----------------------+   +-----------------------+");
        //}
    }
}