using System.Collections.Generic;
using System.Linq;
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
            if (coordinateList != null)
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
            IList<Board> newBoards = new List<Board>();
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

                            IList<Coordinate> capturesList = coordsToCaptureAndDest(board, board[i], coordinate, player).Keys.ToList();

                            if (capturesList != null)
                            {
                                // captureList is all the coordinates presenting the board after the capture.
                                foreach (Coordinate coord in capturesList.Reverse())
                                {
                                    Board nBoard = board.Copy();
                                    nBoard.UpdateBoard(nBoard[nBoard.Search(board[i])], coord);
                                    IsBecameAKing(nBoard, coord);
                                    newBoards.Add(nBoard);
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
                            newBoards.Add(nBoard);
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
            else return false;
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
            else return false;
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
                board.NumberOfBlcakKings++;
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
                        return board.NumberOfBlcakKings + board.NumberOfBlackPieces;
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
            IDictionary<Board, IList<Coordinate>> optionalBoards = CalculateNewBoardsFromCoordinates(board, player);
            int count = optionalBoards.Count();
            return !(count > 0);
        }


        /// <summary>
        ///     Calculate jumps including multiple jumps and best capture route
        /// </summary>
        /// <param name="board"></param>
        /// <param name="oponentCoordinate"></param>
        /// <param name="player"></param>
        /// <param name="srcCoordinate"></param>
        public IList<Coordinate> CalculatesCoordsToJumpTo(Board board, Coordinate srcCoordinate,
                                                          Coordinate oponentCoordinate, Player player)
        {
            IList<Coordinate> resultCoords = new List<Coordinate>();
            int srcX = srcCoordinate.X;
            int srcY = srcCoordinate.Y;
            int oponentX = oponentCoordinate.X;
            int oponentY = oponentCoordinate.Y;
            Coordinate dest;
            int destX, destY;

            //find the direction of the optional capture and set destination accordingly
            if(srcX<oponentX) destX=oponentX+1;
            else destX=oponentX-1;

            if (srcY < oponentY) destY = oponentY + 1;
            else destY = oponentY - 1;
            if (InBounds(board, destX, destY))
            {
                dest = new Coordinate(board[destX, destY]);
            }
            else
            {
                return null;
            }
            if (dest == null)
            {
                return null;
            }
            if (dest.X == destX && dest.Y == destY)
            {
                // in case destination is not empty we cant capture
                if (dest.Status != Piece.None)
                    return null;
                resultCoords.Add(dest);
                dest.Status = srcCoordinate.Status;
                IsBecameAKing(board, dest);
                //find coordinates if we can continue capture from dest
                IList<Coordinate> moreOptionalDirCaptures = GetMovesInDirection(board, dest, player);
                IList<Coordinate> maxEats = new List<Coordinate>();
                int max = 0;
                foreach (var coord in moreOptionalDirCaptures)
                {
                    if (board.IsOpponentPiece(player, coord))
                    {
                        IList<Coordinate> temp = CalculatesCoordsToJumpTo(board, dest, coord, player);
                        if (temp != null)
                        {
                            if (max < temp.Count)
                            {
                                max = temp.Count;
                                maxEats = temp;
                            }
                            if (max == temp.Count)
                            {
                                maxEats = maxEats.Concat(temp).ToList();
                            }
                        }
                    }
                }
                if (maxEats.Count > 0)
                {
                    resultCoords = maxEats;
                }
            }

            return resultCoords;
        }
        
        public IDictionary<Coordinate,IList<Coordinate>> findCaptures (Board board, Player player)
        {
            IDictionary<Coordinate, IList<Coordinate>> res = new Dictionary<Coordinate, IList<Coordinate>>();
            for(int i =1; i<=32; i++)
            {
                if(board.IsOwner(player, board[i]))
                {
                    var coordsInDir=GetMovesInDirection(board,board[i],player);
                    if (coordsInDir == null) break;
                    foreach(Coordinate coordindir in coordsInDir)
                    {
                        if(board.IsOpponentPiece(board.GetOpponent(player),coordindir))
                        {
                            var coordsToJumpTo = coordsToCaptureAndDest(board, board[i], coordindir, player);
                            if (coordsToJumpTo.Count != 0)
                            {
                                res = res.Concat(coordsToJumpTo).ToDictionary(pair => pair.Key, pair => pair.Value);
                            }
                        }
                    }
                }
            }
            return res;
        }
        public IDictionary<Coordinate, IList<Coordinate>> coordsToCaptureAndDest( Board board, Coordinate srcCoordinate, Coordinate oponentCoordinate, Player player)
        {
            IDictionary<Coordinate, IList<Coordinate>> map = new Dictionary<Coordinate, IList<Coordinate>>();
            int srcX= srcCoordinate.X;
            int srcY=srcCoordinate.Y;
            int oponentX=oponentCoordinate.X;
            int oponentY=oponentCoordinate.Y;
            Coordinate dest;
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
                return null;
            }
            if (dest == null)
            {
                return null;
            }
            if (dest.Status != Piece.None)
                    return map;
            else 
            {
                dest.Status = srcCoordinate.Status;
                IsBecameAKing(board,dest);
                //find coordinates if we can continue capture from dest
                IList<Coordinate> moreOptionalDirCaptures = GetMovesInDirection(board, dest, player);
                if(moreOptionalDirCaptures==null) 
                {
                   map.Add(dest,null);
                    return map;
                }
                IDictionary<Coordinate, IList<Coordinate>> temp;
                foreach(var CID in moreOptionalDirCaptures)
                {
                    if(board.IsOpponentPiece(player, CID))
                    {
                        temp=coordsToCaptureAndDest(board,dest,CID,player);
                        if(temp.Values.Count()>map.Values.Count())
                        {
                            map=temp;
                            foreach(var pair in map)
                            {
                                map[pair.Key].Add(CID);
                            }
                        }
                        else if (temp.Values.Count()==map.Values.Count())
                        {                          
                            map= map.Concat(temp).ToDictionary(pair => pair.Key, pair => pair.Value);
                            foreach(var pair in temp)
                            {
                                map[pair.Key].Add(CID);
                            }
                        }                        
                    }                              
                }  
                return map;  
        }
    }
    }
}