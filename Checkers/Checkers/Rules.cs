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
            if (board.IsOwner(player, coordinate))
            {
                if ((board.IsWhite(coordinate)) && (coordinate.X + 1 < board.Rows && coordinate.Y + 1 < board.Columns))
                {
                    var temp1 = new Coordinate {X = coordinate.X + 1, Y = coordinate.Y + 1};
                    coordinateList.Add(temp1);
                }
                if (board.IsWhite(coordinate) && (coordinate.X + 1 < board.Rows && coordinate.Y - 1 > 0))
                {
                    var temp2 = new Coordinate {X = coordinate.X + 1, Y = coordinate.Y - 1};
                    coordinateList.Add(temp2);
                }
                if ((board.IsBlack(coordinate) && (coordinate.X - 1 > 0 && coordinate.Y + 1 < board.Columns)))
                {
                    var temp3 = new Coordinate {X = coordinate.X - 1, Y = coordinate.Y + 1};
                    coordinateList.Add(temp3);
                }
                if ((board.IsBlack(coordinate)&&(coordinate.X - 1 > 0 && coordinate.Y - 1 > 0)))
                {
                    var temp4 = new Coordinate {X = coordinate.X - 1, Y = coordinate.Y - 1};
                    coordinateList.Add(temp4);
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
