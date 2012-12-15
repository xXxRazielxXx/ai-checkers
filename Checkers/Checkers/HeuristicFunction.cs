using System;
using System.Collections.Generic;
using System.Linq;
using CheckersModel;

namespace CheckersEngine
{
    public class HeuristicFunction
    {
        private readonly Random random = new Random();


        /// <summary>
        /// Evalution function which counts number of pieces per player and calculates if player can be captured
        /// </summary>
        /// <param name="board"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public int Evaluate(Board board, Player player)
        {
            const int kingConstant = 120;
            const int soldierConstant = 100;
            int score = 0;
            int blackCaptures = 0;
            int whiteCaptures = 0;

            int numWhiteKings = board.NumberOfWhiteKings;
            int numBlackKings = board.NumberOfBlackKings;
            int numOfWhiteSold = board.NumberOfWhitePieces;
            int numOfBlackSold = board.NumberOfBlackPieces;

            int numOfSoldOnBoard = numWhiteKings + numBlackKings + numOfWhiteSold + numOfBlackSold;
            if (numOfSoldOnBoard <= 7)
                return Evaluate2(board, player);

            int blackScore = (numOfBlackSold*soldierConstant) + (numBlackKings*kingConstant);
            int whiteScore = (numOfWhiteSold*soldierConstant) + (numWhiteKings*kingConstant);
            score += ((blackScore - whiteScore)*200)/(blackScore + whiteScore);
            score += blackScore - whiteScore;

            for (int i = 1; i <= 32; i++)
            {
                if (board.GetPlayer(board[i]) == Player.Black)
                {
                    blackCaptures += (CanCapture(board, board[i], Player.Black)/(numOfBlackSold + numBlackKings))*30;
                }
                else if (board.GetPlayer(board[i]) == Player.White)
                {
                    whiteCaptures += (CanCapture(board, board[i], Player.White)/(numOfWhiteSold + numWhiteKings))*30;
                }
            }
            score += (blackCaptures - whiteCaptures);
            return (player == Player.Black) ? score : -score;
        }

        /// <summary>
        /// An old revion of evalution function - our evaluation got better and better when playing thus we upgraded to the new one
        /// </summary>
        /// <param name="board"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public int EvaluateOld(Board board, Player player)
        {
            int score = 0;
            int blackscloseToKing = 0;
            int whitescloseToKing = 0;
            int whiteCanBeCaptured = 0;
            int blackCanBeCaptured = 0;
            int blackCaptures = 0;
            int whiteCaptures = 0;

            const int kingConstant = 130;
            const int soldierConstant = 100;
            int numWhiteKings = board.NumberOfWhiteKings;
            int numBlackKings = board.NumberOfBlackKings;
            int numOfWhiteSold = board.NumberOfWhitePieces;
            int numOfBlackSold = board.NumberOfBlackPieces;

            int blackScore = (numOfBlackSold*soldierConstant) + (numBlackKings*kingConstant);
            int whiteScore = (numOfWhiteSold*soldierConstant) + (numWhiteKings*kingConstant);
            score += ((blackScore - whiteScore)*200)/(blackScore + whiteScore);
            score += blackScore - whiteScore;

            for (int i = 1; i <= 32; i++)
            {
                if (board.GetPlayer(board[i]) == Player.Black)
                {
                    blackscloseToKing += CloseToBecomeAKing(board[i])*5;
                    blackCanBeCaptured += CanBeCaptured(board, board[i], Player.Black)*60;
                    blackCaptures += CanCapture(board, board[i], Player.Black)*30;
                }
                else if (board.GetPlayer(board[i]) == Player.White)
                {
                    whitescloseToKing += CloseToBecomeAKing(board[i])*5;
                    whiteCanBeCaptured += CanBeCaptured(board, board[i], Player.White)*60;
                    whiteCaptures += CanCapture(board, board[i], Player.White)*30;
                }
            }
            int closeToking = blackscloseToKing - whitescloseToKing;
            int canBeCapture = blackCanBeCaptured - whiteCanBeCaptured;
            int captures = blackCaptures - whiteCaptures;
            score += closeToking;
            score += canBeCapture;
            score += captures;

            const int randomizerMin = -10;
            const int randomizerMax = 10;
            return (player == Player.Black) ? score : -score + random.Next(randomizerMin, randomizerMax);
        }

        /// <summary>
        /// Check the safeness of a coordinate (can be captured). if the sold is on boundary than safe (4) else return the 
        /// delta between number of player soldiers and number of Opp soldiers around the coord. 
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
            int max = 0;
            foreach (var cid in coordinatesinDirection)
            {
                if (board.IsOpponentPiece(player, cid))
                {
                    IDictionary<IList<Coordinate>, Coordinate> captures = rule.CoordsToCaptureAndDest(board, coordinate,
                                                                                                      cid, player);
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

        /// <summary>
        /// if piece is close to became a king 
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        private int CloseToBecomeAKing(Coordinate coord)
        {
            if (coord.Status == Piece.BlackKing || coord.Status == Piece.WhiteKing)
            {
                return 0;
            }
            else
            {
                if (coord.Status == Piece.BlackPiece)
                {
                    return 8 - coord.X;
                }
                else if (coord.Status == Piece.WhitePiece)
                {
                    return 8 - (8 - coord.X);
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// returns the number of ways in which soldier on coord can be captured;
        /// </summary>
        /// <param name="board"></param>
        /// <param name="coord"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public int CanBeCaptured(Board board, Coordinate coord, Player player)
        {
            int num = 0;
            Rules rule = new Rules();
            IList<Coordinate> optionalCoords = rule.OptionalMoves(board, coord, player);
            IList<Coordinate> coordsInDir = rule.GetMovesInDirection(board, coord, player);

            //collect all coords behind coord
            IList<Coordinate> coordsfrombehind = optionalCoords.Where(opCor => !coordsInDir.Contains(opCor)).ToList();
            foreach (var cid in coordsInDir)
            {
                if (board.GetPlayer(board[cid.X, cid.Y]) == board.GetOpponent(player) &&
                    rule.CoordsToCaptureAndDest(board, cid, coord, board.GetOpponent(player)).Count > 0)
                    num++;
            }


            foreach (var cfb in coordsfrombehind)
            {
                if (board.GetPlayer(board[cfb.X, cfb.Y]) == board.GetOpponent(player) && board.IsKing(coord) &&
                    rule.CoordsToCaptureAndDest(board, cfb, coord, board.GetOpponent(player)).Count > 0)
                    num++;
            }
            return num;
        }

        /// <summary>
        /// Additional evaluation function which funcation as a "hummer" to finish game in case only >7 pieces on board
        /// </summary>
        /// <param name="board"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public int Evaluate2(Board board, Player player)
        {
            int score = 0;
            int numWhiteKings = board.NumberOfWhiteKings;
            int numBlackKings = board.NumberOfBlackKings;
            int numOfWhiteSold = board.NumberOfWhitePieces;
            int numOfBlackSold = board.NumberOfBlackPieces;

            int blackScore = (numOfBlackSold) + (numBlackKings);
            int whiteScore = (numOfWhiteSold) + (numWhiteKings);

            score += ((blackScore - whiteScore)*200)/(blackScore + whiteScore);
            score += blackScore - whiteScore;

            return (player == Player.Black) ? score : -score;
        }
    }
}