using System;

namespace CheckersModel
{
    public class Board
    {
        /// <summary>
        /// The number of rows on a checker board
        /// </summary>
        public static readonly int rows =8;
        /// <summary>
        /// The number of columns on a checker board
        /// </summary>
        public static readonly int columns =8;
        /// <summary>
        /// The number of legal checkers
        /// </summary>
        public static readonly int validCheckers=32;

        private readonly Coordinate[] board = new Coordinate[validCheckers];

        /// <summary>
        /// Get the number of rows on the board
        /// </summary>
        public int Rows { get { return rows; } }

        /// <summary>
        /// Get the number of columns on the board
        /// </summary>
        public int Columns { get { return columns; } }

        /// <summary>
        /// Get the size of the board.  This is the number of valid positions on the board.
        /// </summary>
        public int Size { get { return this.board.Length; } }

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
        /// return a value in position index
        /// </summary>
        /// <param name="i"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public Coordinate this[int position]
        {
            get
            {
                if (position <=0 || position > validCheckers)
                {
                    throw new ArgumentOutOfRangeException("position", "Position must be between 1 and 32");
                }
                position--;
                return board[position];
            }
            set
            {
                if (position <= 0 || position > validCheckers)
                {
                    throw new ArgumentOutOfRangeException("position", "Position must be between 0 and 31");
                }
                position--;
                board[position] = value;
            }
        }

        /// <summary>
        /// Huerisitic board grade
        /// </summary>
        public int Grade { get; set; }

        /// <summary>
        /// Clear the board
        /// </summary>
        public void Clear()
        {
            foreach (Coordinate item in board)
            {
                item.Status=Piece.None;
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
                        if (k >= 0 && k <= 11)
                        {
                            board[k].Status = Piece.WhitePiece;
                        }
                        else if (k >= 12 && k <= 19)
                        {
                            board[k].Status = Piece.None;
                        }
                        else
                        {
                            board[k].Status = Piece.BlackPiece;
                        }
                        k++;
                    }
                }
            }
        }
    }
}