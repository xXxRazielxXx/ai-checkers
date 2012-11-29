using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using CheckersEngine;
using CheckersModel;
using Interfaces;


namespace Interpretor
{
    public class PlayerCreator:IPlayerCreator
    {

        public IPlayer CreatePcPlayer(Player playerColor)
        {
            IPlayer play = new Play(playerColor);
            return play;
        }
    }

    public class Move:IMove
    {
        public IBoardState Board { get; private set; }
        public Move(IBoardState board)
        {
            this.Board = board;
        }
    }

    public class Play:IPlayer
    {
        private Player playerColor;
        public Play(Player color)
        {
            this.playerColor = color;
        }
        public IMove GetBoardMove(IBoardState boardState)
        {
            IMove move = new Move(boardState);
            return move;
        }
    }

    public class BoardState:IBoardState
    {
        private Board board;

        public BoardState(int size)
        {
            this.board = new Board(size);
            this.BoardCells = new Piece[8, 8];
            BoardCells = ConvertBoardToBoardState(board);
        }

        public Board Board { get; set; }
        /// <summary>
        /// Convert Board to BoardState (piece[,])
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public Piece[,] ConvertBoardToBoardState(Board board)
        {
            int k = 32;
            for (int i = 0; i < board.Rows; i++)
            {
                int j = i % 2 == 0 ? 1 : 0;

                for (; j < board.Columns; k--, j += 2)
                {
                    BoardCells[i, j] = board[k].Status;
                }
            }
            return BoardCells;
        }

        /// <summary>
        /// Convert BoardState to Board (piece[,])
        /// </summary>
        /// <param name="boardState"></param>
        /// <returns></returns>
        public Board ConvertBoardStateToBoard(IBoardState boardState)
        {
            var board = new Board();
            int k = 1;
            for (int i = 0; i < board.Rows; i++)
            {
                int j = i % 2 == 0 ? 1 : 0;

                for (; j < board.Columns; k++, j += 2)
                {
                    board[k].Status = boardState.BoardCells[i, j];
                    board[k].X = i+1;
                    board[k].Y = j;
                    if (boardState.BoardCells[i,j] == Piece.BlackPiece)
                    {
                        board.NumberOfBlackPieces++;
                    }
                    if (boardState.BoardCells[i, j] == Piece.BlackKing)
                    {
                        board.NumberOfBlcakKings++;
                    }
                    if (boardState.BoardCells[i, j] == Piece.WhitePiece)
                    {
                        board.NumberOfWhitePieces++;
                    }
                    if (boardState.BoardCells[i, j] == Piece.WhiteKing)
                    {
                        board.NumberOfWhiteKings++;
                    }
                }
            }
            board.Columns = board.Rows = 8;
            board.Size = 32;

            return board;
        }

        public Piece[,] BoardCells { get; set; }

        /// <summary>
        /// Convert point to our coordinates in board
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Point ConvertPointToCoordinate(int x, int y)
        {
            var p = new Point { X = x, Y = y };
            if (x % 2 == 0 && y % 2 != 0)
            {
                p.X++;
            }
            if (x % 2 != 0 && y % 2 == 0)
            {
                p.X++;
                p.Y += 2;
            }
            return p;
        }

        /// <summary>
        /// Convert MoveType to Coordinate
        /// </summary>
        /// <param name="position"></param>
        /// <param name="move"></param>
        /// <returns></returns>
        public Point ConvertMoveToCoordinate(Point position, MoveType move)
        {
            var p = ConvertPointToCoordinate(position.X, position.Y);
            switch (move)
            {
                case MoveType.BottomLeft:
                    {
                        p.X--;
                        p.Y--;
                        break;
                    }
                case MoveType.BottomRight:
                    {
                        p.X--;
                        p.Y++;
                        break;
                    }
                case MoveType.UpperLeft:
                    {
                        p.X++;
                        p.Y--;
                        break;
                    }
                case MoveType.UpperRight:
                    {
                        p.X++;
                        p.Y++;
                        break;
                    }
            }
            return p;
        }


        public IBoardState GetBoardState(Player player, MoveType moveType, Point position)
        {
            IBoardState boardState = new BoardState(8);
            boardState.Board = ConvertBoardStateToBoard(this);
            position = ConvertPointToCoordinate(position.X, position.Y);
            var point = ConvertMoveToCoordinate(position, moveType);
            board.UpdateBoard(board[position.X, position.Y], board[point.X, point.Y]);
            boardState.BoardCells = ConvertBoardToBoardState(board);
            return boardState;
        }

        public GameState GetGameState(Player player)
        {
            var rule = new Rules();

            var numberplayerPieces = rule.NumberOfPlayerPieces(board, player);
            var isplayerBlocked = rule.IsPlayerBlocked(board, player);
            if (numberplayerPieces == 0 || isplayerBlocked)
            {
                return GameState.Lost;
            }
            var numberopponentPieces = rule.NumberOfPlayerPieces(this.Board,this.Board.GetOpponent(player));
            var isopponentBlocked = rule.IsPlayerBlocked(this.Board,this.Board.GetOpponent(player));
            if (numberopponentPieces == 0 || isopponentBlocked)
            {
                return GameState.Won;
            }
            else
            {
                return GameState.Undetermined;
            }
        }
    }
}
