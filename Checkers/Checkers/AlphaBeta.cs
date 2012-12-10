using System;
using System.Collections;
using System.Collections.Generic;
using CheckersEngine;
using CheckersModel;

namespace checkersengine
{   
    public class Alphabeta
    {
        private readonly Rules rule = new Rules();
        /// <summary>
        /// alpha beta algorithm
        /// boardcoordlist- each item in this map contains a new board (representing a new state) and a list of 2 coords representing the move that brought to this new state
        /// mapBoardSrcDestCap- each item in this map contains the new board and source- dest coords (like above) and a list of captured coordinate (in case the new board is a result of a capture)
        /// </summary>
        /// <param name="board"></param>
        /// <param name="depth"></param>
        /// <param name="alpha"></param>
        /// <param name="beta"></param>
        /// <param name="player"></param>
        /// <param name="maxplayer"></param>
        /// <param name="srcCoord"></param>
        /// <param name="destCoord"></param>
        /// <param name="updateBoard"></param>
        /// <returns></returns>
        public int AlphaBeta(Board board, int depth, int alpha, int beta, Player player, bool maxplayer,
                             ref Coordinate srcCoord, ref Coordinate destCoord, ref Board updateBoard, ref IList<Coordinate> captures )
        {
            var robj = new Rules();
            if (depth == 0 || rule.IsBoardLeaf(player, board)) // is node a terminal node
            {
                var obj = new HeuristicFunction();
                return obj.Evaluate(board,player);
            }
            IDictionary<IList<Coordinate>, IList<Coordinate>> capturesAvailable = robj.FindCaptures(board, player);
            IDictionary<Board, IList<Coordinate>> boardCoordsList;
           // IDictionary<IDictionary<Board,IList<Coordinate>>,IList<Coordinate>> mapBoardSrcDestCap;
            if (capturesAvailable.Count > 0)
            {
                boardCoordsList = robj.CreateNewBoards(board, capturesAvailable);

            }
            else
            {
                boardCoordsList = robj.CalculateNewBoardsFromCoordinates(board, player);
            }

            //mapBoardSrcDestCap = robj.ConvertToMapWithCaptures(boardCoordsList, capturesAvailable);
         
            var minsrcCoord = new Coordinate();
            var mindestCoord = new Coordinate();
            var maxsrcCoord = new Coordinate();
            var maxdestCoord = new Coordinate();
            var maxCapturesList= new List<Coordinate>();
            var minCapturesList = new List<Coordinate>();
            var minBoard = new Board();
            var maxBoard = new Board();
            if (maxplayer)
            {
                foreach (var newState in boardCoordsList)
                {
                    Coordinate newSrcCoord = newState.Value[0];
                    Coordinate newDestCoord = newState.Value[1];
                    IList<Coordinate> capturesList = robj.MapContainsCoords(capturesAvailable, newSrcCoord, newDestCoord);
                    IList<Coordinate> tempCapList= new List<Coordinate>();
                    int res = AlphaBeta(newState.Key, depth - 1, alpha, beta, board.GetOpponent(player), !maxplayer, ref newSrcCoord, ref newDestCoord, ref updateBoard, ref tempCapList);                 
                    if (res > alpha)
                    {                        
                        alpha = res;
                        maxsrcCoord = newState.Value[0];
                        maxdestCoord = newState.Value[1];
                        maxBoard = newState.Key.Copy();
                        maxCapturesList=new List<Coordinate>(capturesList);
                    }
                    if (beta <= alpha)
                    {
                        break;
                    }
                    if (capturesList.Count > 0)
                    {
                        capturesAvailable.Remove(capturesList);
                    }
                }
                srcCoord = maxsrcCoord;
                destCoord = maxdestCoord;
                updateBoard = maxBoard.Copy();
                captures = maxCapturesList;
                return alpha;
            }
            else
            {
                foreach (var newState in boardCoordsList)
                {                   
                    Coordinate newSrcCoord = newState.Value[0];
                    Coordinate newDestCoord = newState.Value[1];
                    IList<Coordinate> capturesList = robj.MapContainsCoords(capturesAvailable, newSrcCoord, newDestCoord);
                    IList<Coordinate> tempCapList = new List<Coordinate>();
                    int res = AlphaBeta(newState.Key, depth - 1, alpha, beta, board.GetOpponent(player), !maxplayer, ref newSrcCoord, ref newDestCoord, ref updateBoard, ref tempCapList);                             
                    if (res < beta)

                    {
                        beta = res;
                        minsrcCoord = newState.Value[0];
                        mindestCoord = newState.Value[1];
                        minBoard = newState.Key.Copy();
                        minCapturesList = new List<Coordinate>(capturesList);
                    }
                    if (beta <= alpha)
                    {
                        break;
                    }
                    if (capturesList.Count > 0)
                    {
                        capturesAvailable.Remove(capturesList);
                    }
                }
                srcCoord = minsrcCoord;
                destCoord = mindestCoord;
                updateBoard = minBoard.Copy();
                captures = minCapturesList;
                return beta;
            }
        }
    }
    }