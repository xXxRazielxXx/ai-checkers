﻿using System;

namespace CheckersModel
{
    public class Board
    {
        /// <summary>
        ///     The number of rows on a checker board
        /// </summary>
        public static readonly int rows = 8;

        /// <summary>
        ///     The number of columns on a checker board
        /// </summary>
        public static readonly int columns = 8;

        /// <summary>
        ///     The number of legal checkers
        /// </summary>
        public static readonly int validCheckers = 32;

        private readonly Coordinate[] board = new Coordinate[validCheckers];

        /// <summary>
        ///     Defualt constructor
        /// </summary>
        public Board(int size)
        {
            for (int i = 1; i <= 32; i++)
            {
                this[i] = new Coordinate();
            }
            InitializeBoard(size);
        }

        /// <summary>
        ///     Defualt constructor
        /// </summary>
        public Board(){}

        public int NumberOfWhitePieces { set; get; }
        public int NumberOfBlackPieces { set; get; }
        public int NumberOfWhiteKings { set; get; }
        public int NumberOfBlcakKings { set; get; }

        /// <summary>
        ///     Get the number of rows on the board
        /// </summary>
        public int Rows
        {
            get { return rows; }
        }

        /// <summary>
        ///     Get the number of columns on the board
        /// </summary>
        public int Columns
        {
            get { return columns; }
        }

        /// <summary>
        ///     Get the size of the board.  This is the number of valid positions on the board.
        /// </summary>
        public int Size
        {
            get { return board.Length; }
        }

        /// <summary>
        ///     return a value in position index
        ///     starts at one instead of zero and end at 32 instead of 31
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Coordinate this[int position]
        {
            get
            {
                if (position <= 0 || position > validCheckers)
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
                    throw new ArgumentOutOfRangeException("position", "Position must be between 0 and 32");
                }
                position--;
                board[position] = value;
            }
        }

        /// <summary>
        ///     Huerisitic board grade
        /// </summary>
        public int Grade { get; set; }

        /// <summary>
        /// returns the index of cor in the board array
        /// </summary>
        /// <param name="cor"></param>
        /// <returns></returns>
        public int Search(Coordinate cor)
        {
            for (int coordinate = 1; coordinate <= Size; coordinate++)
            {
                if ((this[coordinate].X == cor.X) && (this[coordinate].Y == cor.Y))
                {
                    if (this[coordinate].Status == Piece.None)
                    {
                        return coordinate;
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// Update coordinate on board
        /// </summary>
        /// <param name="board"></param>
        /// <param name="orgCoord"></param>
        /// <param name="desCoord"></param>
        public void UpdateBoard(Coordinate orgCoord, Coordinate desCoord)
        {
            this[Search(desCoord)].Status = this[Search(orgCoord)].Status;
            this[Search(orgCoord)].Status = Piece.None;


        }

        /// <summary>
        ///     Checks if on coordinate there a soldier which is not a king with any color
        /// </summary>
        /// <param name="cor"></param>
        /// <returns></returns>
        public bool IsSoldier(Coordinate cor)
        {
            return ((cor.Status == Piece.BlackPiece) || (cor.Status == Piece.WhitePiece));
        }

        /// <summary>
        ///     Is on given coordinate a black piece is located
        /// </summary>
        /// <param name="cor"></param>
        /// <returns></returns>
        public bool IsBlack(Coordinate cor)
        {
            return ((cor.Status == Piece.BlackPiece) || (cor.Status == Piece.BlackKing));
        }

        /// <summary>
        ///     Is on given coordinate a white piece is located
        /// </summary>
        /// <param name="cor"></param>
        /// <returns></returns>
        public bool IsWhite(Coordinate cor)
        {
            return ((cor.Status == Piece.WhitePiece) || (cor.Status == Piece.WhiteKing));
        }

        /// <summary>
        ///     Is a King loacted on Coordinate
        /// </summary>
        /// <param name="cor"></param>
        /// <returns></returns>
        public bool IsKing(Coordinate cor)
        {
            return ((cor.Status == Piece.BlackKing) || (cor.Status == Piece.WhiteKing));
        }

        /// <summary>
        ///     Is a piece located on coordinate
        /// </summary>
        /// <param name="cor"></param>
        /// <returns></returns>
        public bool IsAloacted(Coordinate cor)
        {
            return (cor.Status != Piece.None);
        }

        /// <summary>
        ///     Is the given player the owner of the specified piece
        /// </summary>
        /// <param name="player"></param>
        /// <param name="cor"></param>
        /// <returns></returns>
        public bool IsOwner(Player player, Coordinate cor)
        {
            if (cor.Status == Piece.None)
            {
                return false;
            }
            Player pieceowner = GetPlayer(cor);
            return (player == pieceowner);
        }

        /// <summary>
        ///     Return piece  type
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        public Piece PieceColor(Coordinate coord)
        {
            return coord.Status;
        }


        /// <summary>
        ///     Get the player that owns the given piece
        /// </summary>
        /// <param name="cor"></param>
        /// <returns></returns>
        public Player GetPlayer(Coordinate cor)
        {
            if (IsBlack(cor))
            {
                return Player.Black;
            }
            if (IsWhite(cor))
            {
                return Player.White;
            }
            return Player.None;
        }

        /// <summary>
        ///     Is the given player the opponent of the specified piece
        /// </summary>
        /// <param name="player"></param>
        /// <param name="coordinate"></param>
        /// <returns></returns>
        public bool IsOpponentPiece(Player player, Coordinate coordinate)
        {
            if (coordinate.Status == Piece.None)
            {
                return false;
            }
            return !IsOwner(player, coordinate);
        }

        /// <summary>
        ///     Are the specified pieces opponents
        /// </summary>
        /// <param name="piece1"></param>
        /// <param name="piece2"></param>
        /// <returns></returns>
        public bool AreOpponents(Coordinate piece1, Coordinate piece2)
        {
            if ((piece1.Status == Piece.None) || (piece2.Status == Piece.None))
            {
                return false;
            }
            return (GetPlayer(piece1) != GetPlayer(piece2));
        }

        /// <summary>
        ///     Clear the board
        /// </summary>
        public void Clear()
        {
            foreach (Coordinate item in board)
            {
                item.Status = Piece.None;
            }
        }

        /// <summary>
        /// Get the opponent of the given player
        /// </summary>
        /// <param name="player">The player to get the opponent for</param>
        /// <returns>The opponent of the given player</returns>
        public Player GetOpponent(Player player)
        {
            if (player == Player.None)
            {
                return player;
            }

            return (player == Player.Black) ? Player.White : Player.Black;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public Board Copy(Board board)
        {
            var nboard = new Board();
            for (int i = 1; i <= 32; i++)
            {
                nboard[i].Status = board[i].Status;
                nboard[i].X = board[i].X;
                nboard[i].Y = board[i].Y;
            }
            return nboard;

        }

        /// <summary>
        ///     Initialize Board
        /// </summary>
        /// <param name="size"></param>
        public void InitializeBoard(int size)
        {
            int k = 1;
            for (; k <= 32; k++)
            {
                for (int i = 1; i <= size; i++)
                {
                    int j = i%2 == 0 ? 2 : 1;
                    for (; j <= size; j += 2)
                    {
                        this[k].X = i;
                        this[k].Y = j;
                        if (k >= 1 && k <= 12)
                        {
                            this[k].Status = Piece.WhitePiece;
                        }
                        else if (k >= 13 && k <= 20)
                        {
                            this[k].Status = Piece.None;
                        }
                        else
                        {
                            this[k].Status = Piece.BlackPiece;
                        }
                        k++;
                    }
                }
            }
        }
    }
}