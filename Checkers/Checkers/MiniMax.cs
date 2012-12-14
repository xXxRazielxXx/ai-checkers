using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CheckersModel;


namespace CheckersEngine
{
    public class MiniMax
    {
        private const int treeDepth = 10;
        //player=1 means max ,player=0 means min
        public int MinMax(Board board, int depth, Player player, bool minormax, ref Coordinate srcCoord, ref Coordinate destCoord, ref Board updateBoard, ref IList<Coordinate> captures)
        {
            var robj = new Rules();
            if ((depth >= treeDepth)||robj.IsBoardLeaf(player,board))
            {
                var obj = new HeuristicFunction();
                return obj.Evaluate(board,player);
            }           
            int max = int.MinValue;

            IDictionary<IList<Coordinate>, IList<Coordinate>> capturesAvailable = robj.FindCaptures(board, player);
            IDictionary<Board, IList<Coordinate>> boardCoordsList;
            if (capturesAvailable.Count > 0)
            {
                boardCoordsList = robj.CreateNewBoards(board, capturesAvailable);

            }
            else
            {
                boardCoordsList = robj.CalculateNewBoardsFromCoordinates(board,player);
            }                    
            var maxsrcCoord = new Coordinate();
            var maxdestCoord = new Coordinate();
            var maxCapturesList = new List<Coordinate>();
            var maxBoard = new Board();

            foreach (KeyValuePair<Board,IList<Coordinate>> newState in boardCoordsList)
            {
                Coordinate newSrcCoord = new Coordinate(newState.Value[0]); 
                Coordinate newDestCoord = new Coordinate( newState.Value[1]);
                IList<Coordinate> capturesList = robj.MapContainsCoords(capturesAvailable, newSrcCoord, newDestCoord);
                IList<Coordinate> tempCapList = new List<Coordinate>();
                int res = -MinMax(newState.Key, (depth + 1), board.GetOpponent(player), minormax, ref newSrcCoord, ref newDestCoord, ref updateBoard, ref tempCapList);
                if (res > max)
                {
                    max = res;
                    maxsrcCoord = newSrcCoord;
                    maxdestCoord = newDestCoord;
                    maxBoard = newState.Key.Copy();
                    maxCapturesList = new List<Coordinate>(capturesList);
                }

            }

                srcCoord = maxsrcCoord;
                destCoord = maxdestCoord;
                updateBoard = maxBoard.Copy();
                captures = maxCapturesList;
                return max;
        }
    }
}