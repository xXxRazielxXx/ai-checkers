using System.Collections.Generic;
using CheckersModel;
using System.Linq;


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
        public IDictionary<Board,IList<Coordinate>> CalculateNewBoardsFromCoordinates(Board board, Player player)
        {
            IList<Board> newBoards = new List<Board>();
            IDictionary<Board, IList<Coordinate>> newBoardsPositions = new Dictionary<Board, IList<Coordinate>>();
            for(var i=1;i<=board.Size;i++)
            {
                if (board.IsOwner(player, board[i]))
                {
                    IList<Coordinate> coordinateList = GetMovesInDirection(board, board[i], player);
                    foreach (var coordinate in coordinateList)
                    {
                        //if a soldier of mine exists in this coord then this coord is not optional;
                        if (board.IsOwner(player, coordinate))
                        {
                            coordinateList.Remove(coordinate);
                        }
                        //if an oppenent soldier exsist in this coord try capturing him!
                        if (board.IsOpponentPiece(player, coordinate))
                        {
                            IList<Coordinate> capturesList = CalculatesCoordsToJumpTo(board, board[i], coordinate, player);
                            // captureList is all the coordinates presenting the board after the capture.
                            foreach (Coordinate coord in capturesList)
                            {
                                Board nBoard = board.Copy(board);
                                nBoard.UpdateBoard(board[i], coord); 
                                IsBecameAKing(nBoard, coord);
                                newBoards.Add(nBoard);
                                IList<Coordinate> temp = new List<Coordinate>();

                                temp.Add(board[i]);
                                temp.Add(coord);
                                newBoardsPositions.Add(nBoard, temp);

                               // temp= temp.Concat(board[i]).ToList();

                                //newBoardsPositions.Add<nBoard,board[i]

                            }

                        }
                        if (!board.IsAloacted(coordinate))
                        {
                            //create new board that represnt board after the move
                            Board nBoard = board.Copy(board);
                            nBoard.UpdateBoard(board[i], coordinate);  
                            IsBecameAKing(nBoard,coordinate);
                            newBoards.Add(nBoard);
                            IList<Coordinate> temp = new List<Coordinate>();
                            temp.Add(board[i]);
                            temp.Add(coordinate);
                            newBoardsPositions.Add(nBoard, temp);
                        }
                    }                   
                }
            }

            return newBoardsPositions;
        }

        
        /// <summary>
        /// Checks if Piece on coordinate became a King
        /// </summary>
        /// <param name="board"></param>
        /// <param name="coordinate"></param>
        public bool IsBecameAKing(Board board, Coordinate coordinate)
        {
            int index;
            if ((board[index=board.Search(coordinate)].Status == Piece.BlackPiece) && (coordinate.X == 1))
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

        public int NumberOfPlayerPieces(Board board,Player player)
        {
            switch (player.ToString())
            {
                case "White":
                    {
                        return board.NumberOfWhiteKings + board.NumberOfWhitePieces;
                    }
                case "Black":
                    {
                        return board.NumberOfBlcakKings + board.NumberOfBlackPieces;
                    }
            }

            return 0;
        }

        public bool IsPlayerBlocked(Board board, Player player)
        {
            var optionalBoards = CalculateNewBoardsFromCoordinates(board, player);
            var count = optionalBoards.Count();
            return !(count > 0);
        }


        /// <summary>
        /// Calculate jumps including multiple jumps and best capture route
        /// </summary>
        /// <param name="board"></param>
        /// <param name="oponentCoordinate"></param>
        /// <param name="player"></param>
        /// <param name="srcCoordinate"></param>
        public IList<Coordinate> CalculatesCoordsToJumpTo(Board board, Coordinate srcCoordinate, Coordinate oponentCoordinate, Player player)
        {
            IList<Coordinate> resultCoords= new List<Coordinate>();
            int srcX= srcCoordinate.X;
            int srcY=srcCoordinate.Y;
            int oponentX=oponentCoordinate.X;
            int oponentY=oponentCoordinate.Y;
            int destX, destY;

            if(srcX<oponentX) destX=oponentX+1;
            else destX=oponentX-1;

            if(srcY<oponentY) destY=oponentY+1;
            else destY=oponentY-1;
            
            Coordinate dest = board[destX,destY];
            if (dest == null)
            {
                return null;
            }
            if (dest.X == destX && dest.Y == destY)
            {
                if (dest.Status != Piece.None)
                    return null;
                resultCoords.Add(dest);
                dest.Status = srcCoordinate.Status;
                IsBecameAKing(board,dest);
                IList<Coordinate> moreOptionalCaptures = GetMovesInDirection(board, dest, player);
                IList<Coordinate> maxEats = new List<Coordinate>();
                int max = 0;
                foreach (var coord in moreOptionalCaptures)
                {
                    if (board.IsOpponentPiece(player, coord))
                    {
                        IList<Coordinate> temp = CalculatesCoordsToJumpTo(board, dest, coord, player);
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
                if (maxEats.Count>0)
                {
                    resultCoords = maxEats;

                }
            }

            return resultCoords;
        }
    }
}