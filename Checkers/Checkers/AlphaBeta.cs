﻿using System.Collections.Generic;
using CheckersEngine;
using CheckersModel;

namespace checkersengine
{   
    public class Alphabeta
    {
        private readonly Rules rule = new Rules();

        public int AlphaBeta(Board board, int depth, int alpha, int beta, Player player, bool maxplayer,
                             ref Coordinate srcCoord, ref Coordinate destCoord)
        {
            if (depth == 0 || rule.IsBoardLeaf(player, board)) // is node a terminal node
            {
                var obj = new HeuristicFunction();
                return obj.Evaluate(board);
            }
            IDictionary<Board, IList<Coordinate>> boardCoordsList = rule.CalculateNewBoardsFromCoordinates(board, player);
            var minsrcCoord = new Coordinate();
            var mindestCoord = new Coordinate();
            var maxsrcCoord = new Coordinate();
            var maxdestCoord = new Coordinate();
            if (maxplayer)
            {
                foreach (var newState in boardCoordsList) //implement node
                {
                    int max = int.MinValue;
                    Coordinate newSrcCoord = newState.Value[0];
                    Coordinate newDestCoord = newState.Value[1];
                    int res = AlphaBeta(newState.Key, depth - 1, alpha, beta, player, !maxplayer, ref newSrcCoord, ref newDestCoord);                 
                    if (res > max)
                    {
                        maxsrcCoord = newSrcCoord;
                        maxdestCoord = newDestCoord;
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
                foreach (var newState in boardCoordsList) //implement node
                {
                    int min = int.MaxValue;
                    Coordinate newSrcCoord = newState.Value[0];
                    Coordinate newDestCoord = newState.Value[1];

                    int res = AlphaBeta(newState.Key, depth - 1, alpha, beta, player, !maxplayer, ref newSrcCoord, ref newDestCoord);                             
                    if (res < min)

                    {
                        minsrcCoord = newSrcCoord;
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