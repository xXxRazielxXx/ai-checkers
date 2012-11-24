using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CheckersModel;

namespace CheckersEngine
{
    public class Rules
    {
        /// <summary>
        /// Get all valid moves for a coordinate
        /// </summary>
        /// <param name="board"></param>
        /// <param name="coordinate"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public IList<Coordinate> ValidMoves(Board board, Coordinate coordinate, Player player)
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
                if (InBounds(board,coordinate.X + 1,coordinate.Y - 1))
                {
                    var temp2 = new Coordinate {X = coordinate.X + 1, Y = coordinate.Y - 1};
                    coordinateList.Add(temp2);

                }

                //Check in bounds
                if (InBounds(board,coordinate.X - 1,coordinate.Y + 1))
                {
                    var temp3 = new Coordinate {X = coordinate.X - 1, Y = coordinate.Y + 1};
                    coordinateList.Add(temp3);

                }

                //Check in bounds
                if (InBounds(board,coordinate.X - 1,coordinate.Y - 1))
                {
                    var temp4 = new Coordinate {X = coordinate.X - 1, Y = coordinate.Y - 1};
                   coordinateList.Add(temp4);
                }
            }
            return coordinateList;
        }

        /// <summary>
        /// Check if in bound
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
        /// Get walks
        /// </summary>
        /// <param name="board"></param>
        /// <param name="coordinate"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public IList<Coordinate> GetWalks(Board board, Coordinate coordinate, Player player)
        {
            var coordinateList = ValidMoves(board, coordinate, player);
            if (coordinateList != null)
            {
                if (board.IsOwner(player, coordinate))
                {
                    foreach (var item in coordinateList)
                    {
                        if (board.IsBlack(coordinate)&&(board.IsPiece(coordinate)))
                        {
                            if (item.X > coordinate.X)
                            {
                                coordinateList.Remove(item);
                            }
                        }
                        if (board.IsWhite(coordinate) && (board.IsPiece(coordinate)))
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

        public void CanJump(Board board, Coordinate coordinate,Player player)
        {
            var coordinateList = GetWalks(board, coordinate, player);
            foreach (var item in coordinateList)
            {
                if (board.IsBlack(coordinate) && board.IsPiece(coordinate))
                {
                   if (InBounds(board, item.X - 1, item.Y - 1))
                   {
                       var temp1 = new Coordinate {X=item.X - 1, Y=item.Y - 1};
                      if (board.Search(temp1))
                      {
                          
                      }
                   }
                    if (InBounds(board, item.X - 1, item.Y + 1))
                    {
                        
                    }
                }
            }
        }

        //  בהינתן שחקן ורשימה מעודכנת של קורדינטות מסתכלת קורדינטה קורדינטה אם היא תפוסה יש מצב חדש של הלוח
        // אם הקורדינטה הזו תפוסה, באיזה צבע היא תפוסה, 
        // צבע שלי - לא מצב חדש, חייל שלא שלי אז אבדוק אם אפשר לאכול אותו
        // 
        //
        //

        
    }
}
