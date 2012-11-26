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
                IList<Board> boardList = robj.CalculateNewBoardsFromCoordinates(board,player);
                var minsrcCoord = new Coordinate();
                var mindestCoord = new Coordinate();
                var maxsrcCoord = new Coordinate();
                var maxdestCoord = new Coordinate();
                
                foreach (Board b in boardList)
                {
                    int res = MinMax(b, (depth + 1), player, !minormax, ref srcCoord, ref destCoord);
                    if (res > max) max = res;
                    if (res < min) min = res;

                }

                if (minormax)
                {
                    return max;
                }
                else
                {
                    return min;
                }
            }
        }
    }
}