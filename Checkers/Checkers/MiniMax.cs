using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CheckersModel;


namespace CheckersEngine
{
    public class MiniMax
    {
        private const int treeDepth = 100;
        //player=1 means max ,player=0 means min
        public int MinMax(Board board, int depth,Player player, bool minormax, ref Coordinate srcCoord, ref Coordinate destCoord)
        {
            if (depth >= treeDepth)
            {
                var obj = new HeuristicFunction();
                return obj.Evaluate(board);
            }
            else
            {
                int min = int.MaxValue;
                int max = int.MinValue;
                var robj = new Rules();
                IDictionary<Board, IList<Coordinate>> boardCoordsList = robj.CalculateNewBoardsFromCoordinates(board,player);
                var minsrcCoord = new Coordinate();
                var mindestCoord = new Coordinate();
                var maxsrcCoord = new Coordinate();
                var maxdestCoord = new Coordinate();
                
                foreach (KeyValuePair<Board,IList<Coordinate>> newState in boardCoordsList)
                {
                    Coordinate newSrcCoord = new Coordinate();
                    Coordinate newDestCoord = new Coordinate();
                    int res = MinMax(newState.Key, (depth + 1), player, !minormax, ref newSrcCoord, ref newDestCoord);
                    if (res > max)
                    {
                        max = res;
                        maxsrcCoord = newSrcCoord;
                        maxdestCoord = newDestCoord;
                    }
                    if (res < min)
                    {
                        min = res;
                        minsrcCoord = newSrcCoord;
                        mindestCoord = newDestCoord;
                    }

                }

                if (minormax)
                {
                    srcCoord = maxsrcCoord;
                    destCoord = maxdestCoord;
                    return max;
                   
                }
                else
                {
                    srcCoord = minsrcCoord;
                    destCoord = mindestCoord;
                    return min;
                }
            }
        }
    }
}