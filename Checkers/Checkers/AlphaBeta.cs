//using System;
//using CheckersModel;

//namespace CheckersEngine
//{
//    public class AlphaBeta
//    {
//        private const bool maxplayer = true;

//        public int Alphabeta(Board board, int depth, int alpha, int beta, bool player)
//        {
//            if (depth == 0 || board.IsTerminal(player)) // is node is a terminal node
//            {
//                var obj = new HeuristicFunction();
//                return obj.Evaluate(board);
//            }
//            if (player == maxplayer)
//            {
//                foreach (var item in node.list(player)) //implement node
//                {
//                    alpha = Math.Max(alpha, Alphabeta(item, depth - 1, alpha, beta, !player));
//                    if (beta < alpha)
//                    {
//                        break;
//                    }
//                }
//                return alpha;
//            }
//            else
//            {
//                foreach (var item in node.list(player)) //implement node
//                {
//                    beta = Math.Min(beta, Alphabeta(item, depth - 1, alpha, beta, !player));
//                    if (beta < alpha)
//                    {
//                        break;
//                    }
//                }

//                return beta;
//            }
//        }
//    }
//}

//Alphabeta(root, depth,int.MinValue,int.MaxValue)