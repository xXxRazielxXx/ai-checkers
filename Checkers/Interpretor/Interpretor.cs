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
        public IBoardState Board { get; set; }

        public Move(IBoardState board, Player player, int depth)
        {
            Rules rule = new Rules();
            board.Board = board.ConvertBoardStateToBoard(board);
            var alphaBeta = new Alphabeta();
            Board temp = new Board();
            var srcCoord = new Coordinate();
            var destCoord = new Coordinate();
            IList<Coordinate> tempCaptures = new List<Coordinate>();
            alphaBeta.AlphaBeta(board.Board, depth, Int32.MinValue, Int32.MaxValue, player, true, ref srcCoord, ref destCoord, ref temp, ref tempCaptures);
            if ((rule.InBounds(board.Board, srcCoord.X, srcCoord.Y)) && (rule.InBounds(board.Board, destCoord.X, destCoord.Y)))
            {
                board.Board = temp.Copy();
                board.BoardCells = board.ConvertBoardToBoardState(board.Board);
                Board = board;
            }
            bool pcCaptured = tempCaptures.Count > 0;
            board.DrawGame = board.CheckDraw(board.Board, board.Board[destCoord.X, destCoord.Y], pcCaptured);
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
            IMove move = new Move(boardState, playerColor, depth);
            return move;
        }
    }

    public class BoardState : IBoardState
    {
        static int countMove = 0;
        static GameState drawGame = GameState.Undetermined;

        public GameState DrawGame { get { return drawGame; } set { drawGame = value; } }

        /// <summary>
        /// Constructor which get size and initialize "our" board and build a new BoardCell and convert "our" board to BoardCell
        /// </summary>
        /// <param name="size"></param>
        public BoardState(int size)
        {
            this.Board = new Board(size);
            this.BoardCells = new Piece[8,8];
            BoardCells = ConvertBoardToBoardState(Board);
        }

        public Board Board { get;  set;}

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
            for (int i = 0; i < Board.Rows; i++)
            {
                int j = i%2 == 0 ? 7 : 6;

                for (int shift=0; j >=0; k--, j -= 2, shift++)
                {
                    k = (8-i)*4 - shift;
                    Board[k].Status = boardState.BoardCells[i, j];
                    Board[k].X = 8 - i;
                    Board[k].Y = j + 1;

                }
            }


            return Board;
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
            var p = new Point { X = Board.rows - y, Y = x + 1 };
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
        public IBoardState GetBoardState(Player player, MoveType moveType, Point position, out bool needToContinueEating,out bool mustCapture)
        {
            Rules rule= new Rules();
            bool lastmovewasACaptured = false;
            needToContinueEating = false; //not correct
            this.Board = ConvertBoardStateToBoard(this);
            var destPoint = ConvertMoveTypeToCoordinate(position, moveType); //returns type point
            var srcPoint = ConvertPointToCoordinate(position.X, position.Y);          // returns type point
            var srcCoord = new Coordinate { X = srcPoint.X, Y = srcPoint.Y, Status = Board.PieceColor(Board[srcPoint.X, srcPoint.Y]) };
            var oppCoord = new Coordinate { X = destPoint.X, Y = destPoint.Y, Status = Board.PieceColor(Board[destPoint.X, destPoint.Y]) };


            if (!CheckValidPieceColor(this.Board, srcPoint.X, srcPoint.Y, player))
            {
                needToContinueEating = false;
                mustCapture = false;
                return null;
            }
            if (!IsEmptyCoord(Board, destPoint.X, destPoint.Y) && CheckValidPieceColor(Board, destPoint.X, destPoint.Y, player))
            {
                needToContinueEating = false;
                mustCapture = false;
                return null;
            }
            if (!IsEmptyCoord(Board, destPoint.X, destPoint.Y) &&
                !CheckValidPieceColor(Board, destPoint.X, destPoint.Y, player))
            {
                //calculate capture.....problem....
                //calculate new dest;
                bool done = false;
                var captures = rule.CoordsToCaptureAndDest(Board, srcCoord, oppCoord, player);
                if (captures.Count > 0)
                {
                    foreach (var listOfCap in captures.Keys)
                    {
                        if (listOfCap.Last() == oppCoord)
                        {
                            int length = listOfCap.Count;
                            Coordinate newDestCoord = rule.FindDestByCap(Board, srcCoord, oppCoord);
                            this.Board.UpdateBoard(srcCoord, newDestCoord);
                            this.Board.UpdateCapturedSoldiers(oppCoord, Board.GetOpponent(player));
                            rule.IsBecameAKing(Board, newDestCoord);
                            Board[oppCoord.X, oppCoord.Y].Status = Piece.None;
                            this.BoardCells = ConvertBoardToBoardState(Board);
                            lastmovewasACaptured = true;
                            drawGame = CheckDraw(Board, Board[newDestCoord.X,newDestCoord.Y], lastmovewasACaptured);
                            if (length > 1)
                                needToContinueEating =true;
                            mustCapture = false;
                            return this;
                        }
                    }
                    mustCapture = true;
                    return null;
                }
                    
                   
            }     
            //check if player doesnt have any availble captures- if he does then this move isn't valid
            var capturesAvaileble = rule.FindCaptures(Board, player);
            if (capturesAvaileble.Count == 0 )
            {
                if(!rule.IsValidMove(Board, srcCoord, Board[destPoint.X,destPoint.Y],player))
                {
                     mustCapture = false;
                     return null;
                }            
                Board.UpdateBoard(Board[srcPoint.X, srcPoint.Y], Board[destPoint.X, destPoint.Y]);
                rule.IsBecameAKing(Board, Board[destPoint.X, destPoint.Y]);
                this.BoardCells = ConvertBoardToBoardState(Board);
                mustCapture = false;
                drawGame = CheckDraw(Board, Board[destPoint.X, destPoint.Y], lastmovewasACaptured);
                return this;
            }
            mustCapture = true;
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
            var rule = new Rules();

            var numberplayerPieces = rule.NumberOfPlayerPieces(Board, player);
            var isplayerBlocked = rule.IsPlayerBlocked(Board, player);
            if (numberplayerPieces == 0 || isplayerBlocked)
            {
                return GameState.Lost;
            }
            var numberopponentPieces = rule.NumberOfPlayerPieces(Board, Board.GetOpponent(player));
            var isopponentBlocked = rule.IsPlayerBlocked(Board, Board.GetOpponent(player));
            if (numberopponentPieces == 0 || isopponentBlocked)
            {
                return GameState.Won;
            }
            if (drawGame == GameState.Draw)
            {
                return GameState.Draw;
            }
            else
            {
                return GameState.Undetermined;
            }       
        }

        /// <summary>
        /// Checks if draw and return number of moves
        /// </summary>
        /// <param name="board"></param>
        /// <param name="coordinate"></param>
        /// <param name="captured"></param>
        public GameState CheckDraw(Board board, Coordinate coordinate, bool captured)
        {

            if (board.IsKing(coordinate) && !captured)
            {
                countMove++;
                if (countMove == 15)
                {
                    return GameState.Draw;
                }
            }
            else
            {
                countMove = 0;
            }
                return GameState.Undetermined;        
        }

    }
}