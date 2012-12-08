using System.Collections.Generic;
using CheckersEngine;
using CheckersModel;

namespace checkersengine
{   
    public class Alphabeta
    {
        private readonly Rules rule = new Rules();

        public int AlphaBeta(Board board, int depth, int alpha, int beta, Player player, bool maxplayer,
                             ref Coordinate srcCoord, ref Coordinate destCoord, ref Board updateBoard)
        {
            var robj = new Rules();
            if (depth == 0 || rule.IsBoardLeaf(player, board)) // is node a terminal node
            {
                var obj = new HeuristicFunction();
                return obj.Evaluate(board,player);
            }
            IDictionary<IList<Coordinate>, IList<Coordinate>> capturesAvailable = robj.FindCaptures(board, player);
            IDictionary<Board, IList<Coordinate>> boardCoordsList;
            if (capturesAvailable.Count > 0)
            {
                boardCoordsList = robj.CreateNewBoards(board, capturesAvailable);

            }
            else
            {
                boardCoordsList = robj.CalculateNewBoardsFromCoordinates(board, player);
            }
         
            var minsrcCoord = new Coordinate();
            var mindestCoord = new Coordinate();
            var maxsrcCoord = new Coordinate();
            var maxdestCoord = new Coordinate();
            var minBoard = new Board();
            var maxBoard = new Board();
            if (maxplayer)
            {
                foreach (var newState in boardCoordsList) //implement node
                {                  
                    Coordinate newSrcCoord = newState.Value[0];
                    Coordinate newDestCoord = newState.Value[1];
                    int res = AlphaBeta(newState.Key, depth - 1, alpha, beta, board.GetOpponent(player), !maxplayer, ref newSrcCoord, ref newDestCoord, ref updateBoard);                 
                    if (res > alpha)
                    {                        
                        alpha = res;
                        maxsrcCoord = newSrcCoord;
                        maxdestCoord = newDestCoord;
                        maxBoard = newState.Key.Copy();
                    }
                    if (beta < alpha)
                    {
                        break;
                    }                       
                }
                srcCoord = maxsrcCoord;
                destCoord = maxdestCoord;
                updateBoard = maxBoard.Copy();
                return alpha;
            }
            else
            {
                foreach (var newState in boardCoordsList) //implement node
                {                   
                    Coordinate newSrcCoord = newState.Value[0];
                    Coordinate newDestCoord = newState.Value[1];

                    int res = AlphaBeta(newState.Key, depth - 1, alpha, beta, board.GetOpponent(player), !maxplayer, ref newSrcCoord, ref newDestCoord, ref updateBoard);                             
                    if (res < beta)

                    {
                        beta = res;
                        minsrcCoord = newSrcCoord;
                        mindestCoord = newDestCoord;
                        minBoard = newState.Key.Copy();
                    }
                    if (beta < alpha)
                    {
                        break;
                    }
                }
                srcCoord = minsrcCoord;
                destCoord = mindestCoord;
                updateBoard = minBoard.Copy();
                return beta;
            }
        }
    }
    }