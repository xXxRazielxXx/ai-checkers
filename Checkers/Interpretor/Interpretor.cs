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
            int depth = rule.DefineDepth(board.Board);
            IList<Coordinate> tempCaptures = new List<Coordinate>();
            alphaBeta.AlphaBeta(ourBoard, depth, Int32.MinValue, Int32.MaxValue, player, true, ref srcCoord, ref destCoord, ref temp, ref tempCaptures);
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
        /// <param name="depth"></param>
        /// <returns></returns>
        public IMove GetBoardMove(IBoardState boardState,int depth)
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
        /// <param name="needToContinueEating"></param>
        /// <returns></returns>
        public IBoardState GetBoardState(Player player, MoveType moveType, Point position, out bool needToContinueEating)
        {
            Rules rule= new Rules();
            needToContinueEating = false; //not correct
            this.Board = ConvertBoardStateToBoard(this);
            var destPoint = ConvertMoveTypeToCoordinate(position, moveType); //returns type point
            var srcPoint = ConvertPointToCoordinate(position.X, position.Y);          // returns type point
            var srcCoord = new Coordinate { X = srcPoint.X, Y = srcPoint.Y, Status = board.PieceColor(board[srcPoint.X, srcPoint.Y]) };
            var oppCoord = new Coordinate { X = destPoint.X, Y = destPoint.Y, Status = board.PieceColor(board[destPoint.X, destPoint.Y]) };


            if (!CheckValidPieceColor(this.board, srcPoint.X, srcPoint.Y, player))
            {
                needToContinueEating = false;
                return null;
            }   
            if (!IsEmptyCoord(board, destPoint.X, destPoint.Y) && CheckValidPieceColor(board,destPoint.X,destPoint.Y,player))
            {
                needToContinueEating = false;
                return null;
            }
            if (!IsEmptyCoord(board, destPoint.X, destPoint.Y) &&
                !CheckValidPieceColor(board, destPoint.X, destPoint.Y, player))
            {
                //calculate capture.....problem....
                //calculate new dest;
                Coordinate newDestCoord;
                bool done = false;                
                var captures = rule.CoordsToCaptureAndDest(board, srcCoord, oppCoord, player);
                if (captures.Count > 0)
                {
                    foreach (var listOfCap in captures.Keys)
                    {
                        if (listOfCap.First()==oppCoord)
                        {
                            int length = listOfCap.Count;                           
                            newDestCoord = rule.FindDestByCap(board, srcCoord, oppCoord);
                            this.board.UpdateBoard(srcCoord, newDestCoord);                                                     
                            this.board.UpdateCapturedSoldiers(oppCoord, board.GetOpponent(player));
                            board[oppCoord.X, oppCoord.Y].Status = Piece.None;
                            this.BoardCells = ConvertBoardToBoardState(board);  
                            if (length > 1)
                                needToContinueEating =true;                           
                            return this;
                        }
                    }                    
                }                
                    return null;
            }     
            //check if player doesnt have any availble captures- if he does then this move isn't valid
            var capturesAvaileble = rule.FindCaptures(board, player);
            if (capturesAvaileble.Count == 0)
            {
                board.UpdateBoard(board[srcPoint.X, srcPoint.Y], board[destPoint.X, destPoint.Y]);
                this.BoardCells = ConvertBoardToBoardState(board);
                return this;
            }
            return null;
        }

        public bool IsEmptyCoord(Board board, int x, int y)
        {
            var destCoord = board[x, y];
            if (destCoord != null && destCoord.Status == Piece.None)
                return true;
            return false;
        }

        public bool CheckValidPieceColor(Board board, int x, int y, Player player)
        {
            var srcCoord = board[x, y];
            if (srcCoord != null && board.GetPlayer(srcCoord) == player)
                return true;
            return false;
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