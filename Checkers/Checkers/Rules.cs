﻿using System.Collections.Generic;
using CheckersModel;

namespace CheckersEngine
{
    public class Rules
    {
        /// <summary>
        ///     Get all valid moves for a coordinate
        /// </summary>
        /// <param name="board"></param>
        /// <param name="coordinate"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public IList<Coordinate> OptionalMoves(Board board, Coordinate coordinate, Player player)
        {
            IList<Coordinate> coordinateList = new List<Coordinate>();
            //Check that player own the piece on that coordinate
            if (board.IsOwner(player, coordinate))
            {
                //Check in bounds
                if (InBounds(board, coordinate.X + 1, coordinate.Y + 1))
                {
                    var temp1 = new Coordinate {X = coordinate.X + 1, Y = coordinate.Y + 1};
                    coordinateList.Add(temp1);
                }

                //Check in bounds
                if (InBounds(board, coordinate.X + 1, coordinate.Y - 1))
                {
                    var temp2 = new Coordinate {X = coordinate.X + 1, Y = coordinate.Y - 1};
                    coordinateList.Add(temp2);
                }

                //Check in bounds
                if (InBounds(board, coordinate.X - 1, coordinate.Y + 1))
                {
                    var temp3 = new Coordinate {X = coordinate.X - 1, Y = coordinate.Y + 1};
                    coordinateList.Add(temp3);
                }

                //Check in bounds
                if (InBounds(board, coordinate.X - 1, coordinate.Y - 1))
                {
                    var temp4 = new Coordinate {X = coordinate.X - 1, Y = coordinate.Y - 1};
                    coordinateList.Add(temp4);
                }
            }
            return coordinateList;
        }

        /// <summary>
        ///  Check if in bound
        /// </summary>
        /// <param name="board"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public bool InBounds(Board board, int row, int column)
        {
            return ((row > 0) && (row <= board.Rows) && (column > 0) && (column <= board.Columns));
        }

        /// <summary>
        ///  Get Moves In Direction
        /// </summary>
        /// <param name="board"></param>
        /// <param name="coordinate"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public IList<Coordinate> GetMovesInDirection(Board board, Coordinate coordinate, Player player)
        {
            IList<Coordinate> coordinateList = OptionalMoves(board, coordinate, player);
            if (coordinateList != null)
            {
                if (board.IsOwner(player, coordinate))
                {
                    foreach (Coordinate item in coordinateList)
                    {
                        if (board.IsBlack(coordinate) && (board.IsSoldier(coordinate)))
                        {
                            if (item.X > coordinate.X)
                            {
                                coordinateList.Remove(item);
                            }
                        }
                        if (board.IsWhite(coordinate) && (board.IsSoldier(coordinate)))
                        {
                            if (item.X < coordinate.X)
                            {
                                coordinateList.Remove(item);
                            }
                        }
                    }
                }
            }
            return coordinateList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="board"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public IList<Board> CalculateNewBoardsFromCoordinates(Board board, Player player)
        {
            IList<Board> newBoards = new List<Board>();
            for(var i=1;i<=board.Size;i++)
            {
                if (board.IsOwner(player, board[i]))
                {
                    IList<Coordinate> coordinateList = GetMovesInDirection(board, board[i], player);
                    foreach (var coordinate in coordinateList)
                    {
                        if (board.IsOwner(player, coordinate))
                        {
                            coordinateList.Remove(coordinate);
                        }
                        if (board.IsOpponentPiece(player, coordinate))
                        {
                            IList<Coordinate> capturesList=CanJump(board, board[i], coordinate, player);
                            foreach (Coordinate coord in capturesList)
                            {
                                Board nBoard = board.Copy(board);
                                nBoard.UpdateBoard(board[i], coord); 
                                IsBecameAKing(nBoard, coord);
                                newBoards.Add(nBoard);                               
                            }

                        }
                        if (!board.IsAloacted(coordinate))
                        {
                            //create new board that represnt board after the move
                            Board nBoard = board.Copy(board);
                            nBoard.UpdateBoard(board[i], coordinate);  
                            IsBecameAKing(nBoard,coordinate);
                            newBoards.Add(nBoard);
                        }
                    }                   
                }
            }

            return newBoards;
        }

        
        /// <summary>
        /// Checks if Piece on coordinate became a King
        /// </summary>
        /// <param name="board"></param>
        /// <param name="coordinate"></param>
        public void IsBecameAKing(Board board, Coordinate coordinate)
        {
            int index;
            if ((board[index=board.Search(coordinate)].Status == Piece.BlackPiece) && (coordinate.X == 1))
            {
                board[index].Status = Piece.BlackKing;
            }
            else if ((board[index = board.Search(coordinate)].Status == Piece.WhitePiece) && (coordinate.X == board.Rows))
            {
                board[index].Status = Piece.WhiteKing;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="board"></param>
        /// <param name="oponentCoordinate"></param>
        /// <param name="player"></param>
        /// <param name="srcCoordinate"></param>
        public IList<Coordinate> CanJump(Board board, Coordinate srcCoordinate, Coordinate oponentCoordinate, Player player)
        {
            Player myColor = player;
            IList<Coordinate> optionalCoordinates =GetMovesInDirection(board, oponentCoordinate, player);
            foreach (Coordinate coord in optionalCoordinates)
            {
                if (player == Player.Black)
                {
                    if (coord.X - srcCoordinate.X != -1)
                    {
                        optionalCoordinates.Remove(coord);
                    }
                }
                else if (player == Player.White)
                {
                    
                }
            }
        }
    }
}