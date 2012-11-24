using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CheckersModel;

namespace CheckersEngine
{
    public class Rules
    {
        private IList<Coordinate> ValidMoves(Board board, Coordinate coordinate, Player player)
        {
            IList<Coordinate> coordinateList = new List<Coordinate>();
            //Check that player own the piece on that coordinate
            if (board.IsOwner(player, coordinate))
            {

                //Check that the selected piece on board is white and in bounds
                if ((board.IsWhite(coordinate)) && (coordinate.X + 1 < board.Rows && coordinate.Y + 1 < board.Columns))
                {
                    var temp1 = new Coordinate {X = coordinate.X + 1, Y = coordinate.Y + 1};
                    //Looks if the move is for an empty place
                    if (board.Search(temp1))
                    {
                        coordinateList.Add(temp1);
                    }
                }

                //Check that the selected piece on board is white and in bounds
                if ((board.IsWhite(coordinate) && (coordinate.X + 1 < board.Rows && coordinate.Y - 1 > 0)))
                {
                    var temp2 = new Coordinate {X = coordinate.X + 1, Y = coordinate.Y - 1};
                    //Looks if the move is for an empty place
                    if (board.Search(temp2))
                    {
                        coordinateList.Add(temp2);
                    }
                }

                //Check that the selected piece on board is black and in bounds
                if ((board.IsBlack(coordinate) && (coordinate.X - 1 > 0 && coordinate.Y + 1 < board.Columns)))
                {
                    var temp3 = new Coordinate {X = coordinate.X - 1, Y = coordinate.Y + 1};
                    //Looks if the move is for an empty place
                    if (board.Search(temp3))
                    {
                        coordinateList.Add(temp3);
                    }
                }
                //Check that the selected piece on board is black and in bounds
                if ((board.IsBlack(coordinate)&&(coordinate.X - 1 > 0 && coordinate.Y - 1 > 0)))
                {
                    var temp4 = new Coordinate {X = coordinate.X - 1, Y = coordinate.Y - 1};
                    //Looks if the move is for an empty place
                    if (board.Search(temp4))
                    {
                        coordinateList.Add(temp4);
                    }
                }
            }
            return coordinateList;
        }

        private bool GetWalks(Coordinate coordinate, Player player, int direction)
        {

            return false;
        }
    }
}
