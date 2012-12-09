using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CheckersModel;

namespace CheckersEngine
{
    public class Rules
    {
        /// <summary>
        /// in case this move isnt a capture- check if valid
        /// </summary>
        /// <param name="board"></param>
        /// <param name="srcCoord"></param>
        /// <param name="destCoord"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool IsValidMove(Board board, Coordinate srcCoord, Coordinate destCoord, Player player)
        {
            if (!(InBounds(board, srcCoord.X, srcCoord.Y)) || !(InBounds(board, destCoord.X, destCoord.Y)))
                return false;
            if ((Math.Abs(srcCoord.X - destCoord.X) > 1) || (Math.Abs(srcCoord.Y - destCoord.Y) > 1))
                return false;
            IList<Coordinate> coordsInDir = GetMovesInDirection(board, srcCoord, player);
           return coordsInDir.Contains(destCoord)&& !board.IsAloacted(destCoord);
        }


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
                    var temp1 = new Coordinate(board[coordinate.X + 1, coordinate.Y + 1]);
                    coordinateList.Add(temp1);
                }

                //Check in bounds
                if (InBounds(board, coordinate.X + 1, coordinate.Y - 1))
                {
                    var temp2 = new Coordinate(board[coordinate.X + 1, coordinate.Y - 1]);
                    coordinateList.Add(temp2);
                }

                //Check in bounds
                if (InBounds(board, coordinate.X - 1, coordinate.Y + 1))
                {
                    var temp3 = new Coordinate(board[coordinate.X - 1, coordinate.Y + 1]);
                    coordinateList.Add(temp3);
                }

                //Check in bounds
                if (InBounds(board, coordinate.X - 1, coordinate.Y - 1))
                {
                    var temp4 = new Coordinate(board[coordinate.X - 1, coordinate.Y - 1]);
                    coordinateList.Add(temp4);
                }
            }
            return coordinateList;
        }

        /// <summary>
        ///     Check if in bound
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
        ///     Get Moves In Direction
        /// </summary>
        /// <param name="board"></param>
        /// <param name="coordinate"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public IList<Coordinate> GetMovesInDirection(Board board, Coordinate coordinate, Player player)
        {
            IList<Coordinate> coordinateList = OptionalMoves(board, coordinate, player);
            if (coordinateList.Count!=0)
            {
                if (board.IsOwner(player, coordinate))
                {
                    foreach (Coordinate item in coordinateList.Reverse())
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
        ///     Calucalte new game boards which are optional moves
        /// </summary>
        /// <param name="board"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public IDictionary<Board, IList<Coordinate>> CalculateNewBoardsFromCoordinates(Board board, Player player)
        {
            IDictionary<Board, IList<Coordinate>> newBoardsPositions = new Dictionary<Board, IList<Coordinate>>();
            for (int i = 1; i <= board.Size; i++)
            {
                if (board.IsOwner(player, board[i]))
                {
                    IList<Coordinate> coordinateList = GetMovesInDirection(board, board[i], player);
                    foreach (Coordinate coordinate in coordinateList.Reverse()) //reverser list correction
                    {
                        //if a soldier of mine exists in this coord then this coord is not optional;
                        if (board.IsOwner(player, coordinate))
                        {
                            coordinateList.Remove(coordinate);
                        }
                            //if an oppenent soldier exsist in this coord try capturing him!
                        else if (board.IsOpponentPiece(player, coordinate))
                        {

                            IList<Coordinate> capturesList = CoordsToCaptureAndDest(board, board[i], coordinate, player).Values.ToList();

                            if (capturesList.Count!=0)
                            {
                                // captureList is all the coordinates presenting the board after the capture.
                                foreach (Coordinate coord in capturesList.Reverse())
                                {
                                    Board nBoard = board.Copy();
                                    nBoard.UpdateBoard(nBoard[nBoard.Search(board[i])], coord);
                                    IsBecameAKing(nBoard, coord);
                                    IList<Coordinate> temp = new List<Coordinate>();

                                    temp.Add(nBoard[nBoard.Search(board[i])]);
                                    temp.Add(coord);
                                    newBoardsPositions.Add(nBoard, temp);
                                }
                            }
                        }
                        else if (!board.IsAloacted(coordinate))
                        {
                            //create new board that represnt board after the move
                            Board nBoard = board.Copy();
                            nBoard.UpdateBoard(nBoard[nBoard.Search(board[i])], coordinate);
                            IsBecameAKing(nBoard, coordinate);
                            IList<Coordinate> temp = new List<Coordinate>();
                            temp.Add(nBoard[nBoard.Search(board[i])]);
                            temp.Add(coordinate);
                            newBoardsPositions.Add(nBoard, temp);
                        }
                    }
                }
            }

            return newBoardsPositions;
        }

        /// <summary>
        ///     checks if player lost the game- is he blocked or have no soldiers;
        /// </summary>
        /// <param name="player"></param>
        /// <param name="srcBoard"></param>
        /// <returns></returns>
        public bool DidPlayerLost(Player player, Board srcBoard)
        {           
            var numberplayerPieces = NumberOfPlayerPieces(srcBoard, player);
            var isplayerBlocked = IsPlayerBlocked(srcBoard, player);
            if (numberplayerPieces == 0 || isplayerBlocked)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///     checks if the game is already determined (one of the oppenents)
        /// </summary>
        /// <param name="player"></param>
        /// <param name="srcBoard"></param>
        /// <returns></returns>
        public bool IsBoardLeaf(Player player, Board srcBoard)
        {
            if (DidPlayerLost(player, srcBoard) || (DidPlayerLost(srcBoard.GetOpponent(player), srcBoard)))
                return true;
            return false;
        }


        /// <summary>
        ///     Checks if Piece on coordinate became a King
        /// </summary>
        /// <param name="board"></param>
        /// <param name="coordinate"></param>
        public bool IsBecameAKing(Board board, Coordinate coordinate)
        {
            int index;
            if ((board[index = board.Search(coordinate)].Status == Piece.BlackPiece) && (coordinate.X == 1))
            {
                board[index].Status = Piece.BlackKing;
                board.NumberOfBlackKings++;
                board.NumberOfBlackPieces--;
            }
            else if ((board[index = board.Search(coordinate)].Status == Piece.WhitePiece) && (coordinate.X == board.Rows))
            {
                board[index].Status = Piece.WhiteKing;
                board.NumberOfWhiteKings++;
                board.NumberOfWhitePieces--;
            }
            else
            {
                return false;
            }
            return true;
        }

        /// <summary>
        ///     Count the number of player pieces on board
        /// </summary>
        /// <param name="board"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public int NumberOfPlayerPieces(Board board, Player player)
        {
            switch (player)
            {
                case Player.White:
                    {
                        return board.NumberOfWhiteKings + board.NumberOfWhitePieces;
                    }
                case Player.Black:
                    {
                        return board.NumberOfBlackKings + board.NumberOfBlackPieces;
                    }
            }

            return 0;
        }

        /// <summary>
        ///     Is opponet won the game by blocking player
        /// </summary>
        /// <param name="board"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool IsPlayerBlocked(Board board, Player player)
        {
            for (int i = 1; i <= 32; i++)
            {
                if (board.GetPlayer(board[i]) == player)
                {
                    if (!IsCoordinateBlocked(board,board[i], player))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Checks if a coordinate can't move (capture or just move)
        /// </summary>
        /// <param name="srcBoard"></param>
        /// <param name="coordinate"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool IsCoordinateBlocked(Board srcBoard, Coordinate coordinate, Player player)
        {
            int count = 0;
            IList<Coordinate> moves = GetMovesInDirection(srcBoard, coordinate, player);
            if (moves.Count == 0)
            {
                return true;
            }
            foreach (var move in moves)
            {
                if (move.Status == Piece.None)
                {
                    return false;
                }
                if (srcBoard.GetPlayer(move) == player)
                {
                    count++;
                    if (moves.Count == count)
                    {
                        return true;
                    }
                }
                else if (srcBoard.GetPlayer(move) != player)
                {
                    IDictionary<IList<Coordinate>,Coordinate> captures = CoordsToCaptureAndDest(srcBoard, coordinate,
                                                                                                 move, player);
                    if (captures.Count > 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }


        /// <summary>
        /// Find dictionary of all captures on board per player
        /// </summary>
        /// <param name="board"></param>
        /// <param name="player"></param>
        /// <returns>dictionary of coordinate (destination cords) and list of coordinates (captured coordinates)</returns>
        public IDictionary<IList<Coordinate>,IList<Coordinate>> FindCaptures (Board board, Player player)
        {
            IDictionary<IList<Coordinate>, IList<Coordinate>> res = new Dictionary<IList<Coordinate>, IList<Coordinate>>();
            for(int i =1; i<=32; i++)
            {
                if(board.IsOwner(player, board[i]))
                {
                    var coordsInDir=GetMovesInDirection(board,board[i],player);
                    if (coordsInDir.Count==0) break;
                    foreach(Coordinate coordindir in coordsInDir)
                    {
                        if(board.IsOpponentPiece(player,coordindir))
                        {
                            var coordsToJumpTo = CoordsToCaptureAndDest(board, board[i], coordindir, player);
                            if (coordsToJumpTo.Count != 0)
                            {
                                foreach (var item in coordsToJumpTo)
                                {
                                    res.Add(new List<Coordinate>{board[i],item.Value},item.Key); //first - source , second - dest in first list
                                }
                                //res = res.Concat(coordsToJumpTo).ToDictionary(pair => pair.Key, pair => pair.Value);
                            }
                        }
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// Find captures from a given source coord and opponent coord
        /// 
        /// </summary>
        /// <param name="board"></param>
        /// <param name="srcCoordinate"></param>
        /// <param name="oponentCoordinate"></param>
        /// <param name="player"></param>
        /// <returns> a dictionary of dest coords and lists of captures</returns>
        public IDictionary<IList<Coordinate>, Coordinate> CoordsToCaptureAndDest(Board board, Coordinate srcCoordinate, Coordinate oponentCoordinate, Player player)
        {
            IDictionary<IList<Coordinate>, Coordinate> map = new Dictionary<IList<Coordinate>,Coordinate>();
            int srcX= srcCoordinate.X;
            int srcY=srcCoordinate.Y;
            int oponentX=oponentCoordinate.X;
            int oponentY=oponentCoordinate.Y;
            Coordinate dest;
            Piece opponentPiece = new Piece();
            int destX, destY;

            //find the direction of the optional capture and set destination accordingly
            if(srcX<oponentX) destX=oponentX+1;
            else destX=oponentX-1;

            if(srcY<oponentY) destY=oponentY+1;
            else destY=oponentY-1;
            if (InBounds(board, destX, destY))
            {
                dest = new Coordinate(board[destX,destY]);
            }
            else
            {
                return map;
            }
            if (dest.Status != Piece.None)
                    return map;
            dest.Status = srcCoordinate.Status;
            
            IsBecameAKing(board,dest);
            if (board.IsKing(dest))
            {
                opponentPiece = board[oponentCoordinate.X, oponentCoordinate.Y].Status;
                board[oponentCoordinate.X, oponentCoordinate.Y].Status = Piece.None;
            }
            //find coordinates if we can continue capture from dest
            IList<Coordinate> moreOptionalDirCaptures = GetMovesInDirection(board, dest, player);
            map.Add(new List<Coordinate> {oponentCoordinate}, dest);
            if(moreOptionalDirCaptures.Count==0) 
            {              
                return map;
            }            
            foreach(var cid in moreOptionalDirCaptures)
            {
                if(board.IsOpponentPiece(player, cid))
                {
                    IDictionary<IList<Coordinate>, Coordinate> temp = CoordsToCaptureAndDest(board,dest,cid,player);
                    if(temp.Keys.Count()>map.Keys.Count())
                    {                       
                        map=temp;
                        if (map.Values.Contains(dest))
                        {
                            var item = map.FirstOrDefault( (x => x.Value.X == dest.X && x.Value.Y == dest.Y));
                            map.Remove(item.Key);
                        }
                            

                    }
                    else if (temp.Keys.Count()==map.Keys.Count())
                    {                          
                        map= map.Concat(temp).ToDictionary(pair => pair.Key, pair => pair.Value);
                        if (map.Values.Contains(dest))
                        {
                            var item = map.FirstOrDefault((x => x.Value.X == dest.X && x.Value.Y == dest.Y));
                            map.Remove(item.Key);
                        }
                    }
                }
            }
            if (!map.Values.Contains(dest))
            {
                foreach (var item in map)
                {
                    item.Key.Add(oponentCoordinate);
                }
            }
            if(board[oponentCoordinate.X, oponentCoordinate.Y].Status == Piece.None)
            {
                board[oponentCoordinate.X, oponentCoordinate.Y].Status = opponentPiece;
            }
            return map;
        }

        /// <summary>
        /// Create new boards
        /// </summary>
        /// <param name="srcBoard"></param>
        /// <param name="capturesAvailable"></param>
        /// <returns></returns>
        public IDictionary<Board, IList<Coordinate>> CreateNewBoards(Board srcBoard,IDictionary<IList<Coordinate>, IList<Coordinate>> capturesAvailable)
        {
            IDictionary<Board, IList<Coordinate>> res = new Dictionary<Board, IList<Coordinate>>();
            foreach (var item in capturesAvailable)
            {
                Board nBoard = srcBoard.Copy();
                nBoard.UpdateBoard(nBoard[nBoard.Search(item.Key.First())], item.Key.Last());
                IsBecameAKing(nBoard, item.Key.Last());
                Player player = nBoard.GetPlayer(item.Key.First());
                Player oPlayer = nBoard.GetOpponent(player);
                foreach (var capCoord in item.Value)
                {
                        nBoard[nBoard.Search(capCoord)].Status=Piece.None;
                        nBoard.UpdateCapturedSoldiers(capCoord,oPlayer);
                }
                IList<Coordinate> temp = new List<Coordinate>();
                temp.Add(nBoard[nBoard.Search(item.Key.First())]);
                temp.Add(item.Key.Last());
                res.Add(nBoard,temp);
            }
            return res;
        }

        public int DefineDepth(Board board)
        {
            int num = board.NumOfSolOnBoard();
            if (num > 17)
                return 5;
            else if (num < 17 && num > 11)
                return 7;
            else return 10;
        }
    }
}