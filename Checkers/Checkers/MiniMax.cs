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
        public int MinMax(Board board, int depth,Player player, bool minormax, ref Coordinate srcCoord, ref Coordinate destCoord,ref Board updateBoard)
        {
            var robj = new Rules();
            if ((depth >= treeDepth)||robj.IsBoardLeaf(player,board))
            {
                var obj = new HeuristicFunction();
                return obj.Evaluate(board,player);
            }
            int min = int.MaxValue;
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
            var minsrcCoord = new Coordinate();
            var mindestCoord = new Coordinate();
            var maxsrcCoord = new Coordinate();
            var maxdestCoord = new Coordinate();
            var minBoard = new Board();
            var maxBoard = new Board();

            foreach (KeyValuePair<Board,IList<Coordinate>> newState in boardCoordsList)
            {
                Coordinate newSrcCoord = new Coordinate(newState.Value[0]); 
                Coordinate newDestCoord = new Coordinate( newState.Value[1]); 
                int res = -MinMax(newState.Key, (depth + 1), board.GetOpponent(player), minormax, ref newSrcCoord, ref newDestCoord, ref updateBoard);
                if (res > max)
                {
                    max = res;
                    maxsrcCoord = newSrcCoord;
                    maxdestCoord = newDestCoord;
                    maxBoard = newState.Key.Copy();
                }

            }

                srcCoord = maxsrcCoord;
                destCoord = maxdestCoord;
                updateBoard = maxBoard.Copy();
                return max;
        }
    }
}