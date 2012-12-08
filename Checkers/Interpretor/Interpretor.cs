using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using CheckersEngine;
using CheckersModel;
using Interfaces;
using checkersengine;


namespace Interpretor
{
    /// <summary>
    /// Get player color initialize a new IPlayer and returns it
    /// </summary>
    public class PlayerCreator : IPlayerCreator
    {
        public IPlayer CreatePcPlayer(Player playerColor)
        {
            IPlayer play = new Play(playerColor);
            return play;
        }
    }


    /// <summary>
    /// Get a IBoardState in constructor and set the board field with it
    /// </summary>
    public class Move : IMove
    {
        public IBoardState Board { get;  set; }

        public Move(IBoardState board,Player player)
        {
            Rules rule = new Rules();
            var ourBoard = board.ConvertBoardStateToBoard(board);
            var alphaBeta = new Alphabeta();
            Board temp = new Board();
            var srcCoord = new Coordinate();
            var destCoord = new Coordinate();
            int depth = 5;
            alphaBeta.AlphaBeta(ourBoard, depth, Int32.MinValue, Int32.MaxValue, player, true, ref srcCoord, ref destCoord, ref temp);
            if ((rule.InBounds(ourBoard, srcCoord.X, srcCoord.Y)) && (rule.InBounds(ourBoard, destCoord.X, destCoord.Y)))
            {
                ourBoard = temp.Copy();
                board.BoardCells= board.ConvertBoardToBoardState(ourBoard);
                Board = board;
            }
        }
    }


    
    public class Play : IPlayer
    {
        private Player playerColor;

        /// <summary>
        /// Get player color and set the player field with it
        /// </summary>
        public Play(Player color)
        {
            this.playerColor = color;
        }

        /// <summary>
        /// Return IMove with current boardstate
        /// </summary>
        /// <param name="boardState"></param>
        /// <returns></returns>
        public IMove GetBoardMove(IBoardState boardState)
        {
            IMove move = new Move(boardState,playerColor);
            return move;
        }
    }

    public class BoardState : IBoardState
    {
        private Board board;


        /// <summary>
        /// Constructor which get size and initialize "our" board and build a new BoardCell and convert "our" board to BoardCell
        /// </summary>
        /// <param name="size"></param>
        public BoardState(int size)
        {
            this.board = new Board(size);
            this.BoardCells = new Piece[8,8];
            BoardCells = ConvertBoardToBoardState(board);
        }

        public Board Board { get; set; }

        /// <summary>
        /// Convert Board to BoardState (piece[,])
        /// </summary>
        /// <param name="ourBoard"></param>
        /// <returns></returns>
        public Piece[,] ConvertBoardToBoardState(Board ourBoard)
        {
            int k = 32;
            for (int i = 0; i < ourBoard.Rows; i++)
            {
                int j = i%2 == 0 ? 7 : 6;

                for (; j >=0; k--, j -= 2)
                {
                    BoardCells[i, j] = ourBoard[k].Status;
                }
            }
            return BoardCells;
        }

        /// <summary>
        /// Convert BoardState to Board (Coordinate[])
        /// </summary>
        /// <param name="boardState"></param>
        /// <returns></returns>
        public Board ConvertBoardStateToBoard(IBoardState boardState)
        {
            
            int k = 32;
            for (int i = 0; i < board.Rows; i++)
            {
                int j = i%2 == 0 ? 7 : 6;

                for (int shift=0; j >=0; k--, j -= 2, shift++)
                {
                    k = (8-i)*4 - shift;
                    board[k].Status = boardState.BoardCells[i, j];
                    board[k].X = 8-i;
                    board[k].Y = j+1;
                    if (boardState.BoardCells[i, j] == Piece.BlackPiece)
                    {
                        board.NumberOfBlackPieces++;
                    }
                    if (boardState.BoardCells[i, j] == Piece.BlackKing)
                    {
                        board.NumberOfBlackKings++;
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
            //temp.Columns = board.Rows = 8;
            //temp.Size = 32;

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
            var p = new Point { X = board.rows - y, Y = x+1 };
            return p;
        }

        /// <summary>
        /// Convert MoveType to Coordinate
        /// </summary>
        /// <param name="position"></param>
        /// <param name="move"></param>
        /// <returns></returns>
        public Point ConvertMoveTypeToCoordinate(Point position, MoveType move)
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

        /// <summary>
        /// Returns current board state after moveType from position
        /// </summary>
        /// <param name="player"></param>
        /// <param name="moveType"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public IBoardState GetBoardState(Player player, MoveType moveType, Point position)
        {
            this.Board = ConvertBoardStateToBoard(this);
            var point = ConvertMoveTypeToCoordinate(position, moveType);
            position = ConvertPointToCoordinate(position.X, position.Y);
            board.UpdateBoard(board[position.X, position.Y], board[point.X, point.Y]);
            this.BoardCells = ConvertBoardToBoardState(board);
            return this;
        }

        public GameState GetGameState(Player player)
        {
            Rules rule = new Rules();
            if (rule.DidPlayerLost(player, board))
                return GameState.Lost;
            else if (rule.DidPlayerLost(board.GetOpponent(player), board))
                return GameState.Won;
            else
                return GameState.Undetermined;        
        }

    }
}