using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CheckersModel;
using CheckersEngine;


namespace CheckersEngine
{
    class MiniMax
    {
        int treeDepth = 100;
        //player=1 means max player=0 means min
        public int MinMax(Board board, int depth, bool player)
        {

            if (depth >= treeDepth)
            {
                HeuristicFunction obj = new HeuristicFunction();
                return obj.HeuristicFunction(board);
            }
            else
            {
                int min = int.MaxValue;
                int max = int.MinValue;
                Rules Robj = new Rules();
                IList<Board> boardList = Robj.NextGamePositions(board);
                foreach (Board b in boardList)
                {
                    int res =MinMax( b, (depth+1), !player);
                    if( res > max) max=res;
                    if (res < min) min= res;
                       
                }
                             
                if(player) return max;
                else return min;               
            }
            
        }

    }
}
