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
            DrawBoard(board, Console.Out);
        }

        /// <summary>
        ///     Write the board state to the console
        /// </summary>
        /// <param name="board">The board</param>
        /// <param name="writer">The writer</param>
        public void DrawBoard(Board board, TextWriter writer)
        {
            var buf1 = new StringBuilder();
            var buf2 = new StringBuilder();
            bool darkSquare = false;
            writer.WriteLine("+-----------------------+   +-----------------------+");

            buf1.Append("|  ");
            buf2.Append("|  ");
            int col = 1;
            for (int pos = board.Size; pos >= 1; pos--)
            {
                String ch = string.Empty;

                if ((col != 0) && (col%8 == 0))
                {
                    col = 0;
                    pos++;
                    darkSquare = !darkSquare;
                    buf1.Append("|");
                    buf2.Append("|");

                    writer.WriteLine("{0}   {1}", buf1, buf2);
                    buf1.Length = 0;
                    buf2.Length = 0;
                    writer.WriteLine("+-----------------------+   +-----------------------+");
                }
                else
                {
                    col++;
                    darkSquare = !darkSquare;

                    if (!darkSquare)
                    {
                        ch = "..";
                        pos++;
                    }
                    else
                    {
                        Piece piece = board[pos].Status;
                        switch (piece)
                        {
                            case Piece.None:
                                ch = " ";
                                break;
                            case Piece.BlackPiece:
                                ch = "b";
                                break;
                            case Piece.WhitePiece:
                                ch = "w";
                                break;
                            case Piece.BlackKing:
                                ch = "B";
                                break;
                            case Piece.WhiteKing:
                                ch = "W";
                                break;
                        }
                    }

                    buf1.AppendFormat("|{0,2}", ch);
                    buf2.AppendFormat("|{0,2}", (darkSquare) ? pos.ToString(CultureInfo.CurrentCulture) : " ");
                }
            }

            buf1.Append("|");
            buf2.Append("|");
            writer.WriteLine("{0}  |   {1}  |", buf1, buf2);
            writer.WriteLine("+-----------------------+   +-----------------------+");
        }
    }
}