//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Checkers
//{
//    public class AlphaBeta
//    {
//        private bool maxplayer = true;

//        public int Alphabeta(Node node, int depth,int alpha, int beta, bool player )
//        {
//          if (depth == 0 || node.IsTerminal(player)) // is node is a terminal node
//          {
//              return node.HeuristicFunction(player); //Implement heuristic function
//          }
//            if (player == maxplayer)
//            {
//                return alpha;
//            }

//        }
//    }
//}
