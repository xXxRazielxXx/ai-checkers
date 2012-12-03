using System;
using CheckersModel;
using CheckersEngine;
using System.Collections.Generic;

namespace checkersengine
{
    public class alphabeta
    {      
        Rules rule = new Rules();
        public int alphabeta(Board board, int depth, int alpha, int beta,Player player, bool maxplayer, ref Coordinate srcCoord, ref Coordinate destCoord)
        {
            if (depth == 0 || rule.IsBoardLeaf(player, board) ) // is node a terminal node
            {
                var obj = new HeuristicFunction();
                return obj.Evaluate(board);
            }
            IDictionary<Board, IList<Coordinate>> boardCoordsList = rule.CalculateNewBoardsFromCoordinates(board,player);
            var minsrcCoord = new Coordinate();
            var mindestCoord = new Coordinate();
            var maxsrcCoord = new Coordinate();
            var maxdestCoord = new Coordinate();  
            if (maxplayer)
            {                     
                foreach (KeyValuePair<Board,IList<Coordinate>> newState in boardCoordsList) //implement node
                {
                    int max=int.MinValue;
                    Coordinate newSrcCoord = newState.Value[0]; 
                    Coordinate newDestCoord = newState.Value[1];
                    int res= alphabeta(newState.Key, depth - 1, alpha, beta, player, !maxplayer,ref newSrcCoord,ref newDestCoord);
                    if(res>max)
                    {
                        maxsrcCoord= newSrcCoord;
                        maxdestCoord=newDestCoord;
                    }                    
                    if (beta < alpha)
                    {
                        break;
                    }
                    srcCoord = maxsrcCoord;
                    destCoord = maxdestCoord;
                }

                return alpha;
            }
            else
            {
                foreach (KeyValuePair<Board,IList<Coordinate>> newState in boardCoordsList) //implement node
                {
                    int min=int.MaxValue;
                    Coordinate newSrcCoord = newState.Value[0]; 
                    Coordinate newDestCoord = newState.Value[1];
                    int res = alphabeta(newState.Key, depth - 1, alpha, beta, player,!maxplayer,ref newSrcCoord, ref newDestCoord);
                    if(res< min)
                    {
                        minsrcCoord =newSrcCoord;
                        mindestCoord = newDestCoord;
                    }
                    if (beta < alpha)
                    {
                        break;
                    }
                }
                 srcCoord = minsrcCoord;
                 destCoord = mindestCoord;
                return beta;
            }
        }
    }
}

