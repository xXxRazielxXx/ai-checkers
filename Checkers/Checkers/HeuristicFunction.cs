using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CheckersModel;

namespace CheckersEngine
{
    public class HeuristicFunction
    {
        //// calculates the pieces 'safeness' 
        //// assumption: the computer is the black player
        //public int Evaluate_old(Board board)
        //{
        //    const int kingConstant = 10;
        //    const int soldierConstant = 1;          
        //    int numWhiteKings = board.NumberOfWhiteKings;
        //    int numBlackKings = board.NumberOfBlackKings;
        //    int numOfWhiteSold = board.NumberOfWhitePieces; ;
        //    int numOfBlackSold = board.NumberOfBlackPieces; ;

        //    return (((numOfBlackSold*soldierConstant) + (numBlackKings*kingConstant)) -
        //            ((numOfWhiteSold*soldierConstant) + (numWhiteKings*kingConstant)));
        //}

        /// <summary>
        /// huristic function- foreach soldier of player calculate the follwong parameters: is a king, is safe, can capture, can be captured
        /// </summary>
        /// <param name="board"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public int Evaluate(Board board, Player player)
        {
            //PrintBoardState print = new PrintBoardState();
            //print.DrawBoard(board);
            const int kingConstant = 10;
            const int soldierConstant = 1;
            int playerSum = 0;
            int opponentSum = 0;
            for (int i = 1; i <= 32; i++)
            {
                int coordSum = 0;
                if (board.IsKing(board[i]))
                    coordSum += kingConstant;
                else if (board.IsSoldier(board[i]))
                    coordSum += soldierConstant;

                coordSum += Safeness(board, board[i], board.GetPlayer(board[i]));
                coordSum += CanCapture(board, board[i], board.GetPlayer(board[i]));

                if (board.IsOwner(player, board[i]))
                {
                    playerSum += coordSum;
                }
                else if(board.IsOwner(board.GetOpponent(player),board[i]))
                {
                    opponentSum += coordSum;
                }
            }
            return playerSum - opponentSum;
        }


        /// <summary>
        /// Check the safeness of a coordinate (can be captured)
        /// </summary>
        /// <param name="board"></param>
        /// <param name="coord"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        private int Safeness(Board board, Coordinate coord, Player player)
        {
            int playerCoords = 0;
            int opponentCoords = 0;
            Rules rule = new Rules();
            if (coord.X == 1 || coord.X == 8 || coord.Y == 1 || coord.Y == 8)
            {
                return 4;
            }
            IList<Coordinate> coordinates = rule.OptionalMoves(board, coord, player);
            foreach (var coordinate in coordinates)
            {
                if (player == board.GetPlayer(coordinate))
                {
                    playerCoords++;
                }
                else if (board.GetOpponent(player) == board.GetPlayer(coordinate))
                {
                    opponentCoords++;
                }
            }
            return playerCoords - opponentCoords;
        }

        /// <summary>
        /// Checkes whether the coordinate can capture
        /// </summary>
        /// <param name="board"></param>
        /// <param name="coordinate"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        private int CanCapture(Board board, Coordinate coordinate, Player player)
        {
            Rules rule = new Rules();
            IList<Coordinate> coordinatesinDirection = rule.GetMovesInDirection(board, coordinate, player);
            IDictionary<IList<Coordinate>, Coordinate> captures = new Dictionary<IList<Coordinate>,Coordinate>();
            int max = 0;
            foreach (var cid in coordinatesinDirection)
            {
                if (board.IsOpponentPiece(player, cid))
                {
                    captures = rule.CoordsToCaptureAndDest(board, coordinate, cid, player);
                    if (captures.Count > 0)
                    {
                        if (captures.ElementAt(0).Key.Count > max)
                        {
                            max = captures.ElementAt(0).Key.Count;
                        }
                    }
                }
            }
            return max;
        }
    }
}