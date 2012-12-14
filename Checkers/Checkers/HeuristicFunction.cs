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
        private readonly Random random = new Random();

        public int Evaluate(Board board, Player player)
        {
            PrintBoardState print = new PrintBoardState();
            print.DrawBoard(board);
            //Console.ReadKey();
            const int kingConstant = 120;
            const int soldierConstant = 100;
            int score = 0;
            int blackCaptures = 0;
            int whiteCaptures = 0;
            int blackCanBeCaptured = 0;
            int whiteCanBeCaptured = 0;

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
                   // blackCanBeCaptured += (CanBeCaptured(board, board[i], Player.Black))*50;

                }
                else if (board.GetPlayer(board[i]) == Player.White)
                {
                    whiteCaptures += (CanCapture(board, board[i], Player.White)/(numOfWhiteSold + numWhiteKings))*30;
                     //whiteCanBeCaptured += (CanBeCaptured(board, board[i], Player.White)) * 50;
                }
            }
            score += (blackCaptures - whiteCaptures);
           // score += (blackCanBeCaptured - whiteCanBeCaptured);
            return (player == Player.Black) ? score : -score;

        }

        public int EvaluateOld(Board board, Player player)
        {
            int score = 0;
            int safeBlack = 0;
            int safeWhite = 0;
            int BlackscloseToKing = 0;
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
                    // safeBlack += Safeness(board, board[i], Player.Black) * 10;
                    BlackscloseToKing += CloseToBecomeAKing(board[i])*5;
                    blackCanBeCaptured += CanBeCaptured(board, board[i], Player.Black)*60;
                    blackCaptures += CanCapture(board, board[i], Player.Black)*30;
                }
                else if (board.GetPlayer(board[i]) == Player.White)
                {
                    //safeWhite += Safeness(board,board[i], Player.White)*10;
                    whitescloseToKing += CloseToBecomeAKing(board[i])*5;
                    whiteCanBeCaptured += CanBeCaptured(board, board[i], Player.White)*60;
                    whiteCaptures += CanCapture(board, board[i], Player.White)*30;

                }
            }
            //int safe = safeBlack - safeWhite;
            int closeToking = BlackscloseToKing - whitescloseToKing;
            int canBeCapture = blackCanBeCaptured - whiteCanBeCaptured;
            int captures = blackCaptures - whiteCaptures;
            //score += safe;
            score += closeToking;
            score += canBeCapture;
            score += captures;

            const int RANDOMIZER_MIN = -10;
            const int RANDOMIZER_MAX = 10;
            return (player == Player.Black) ? score : -score + random.Next(RANDOMIZER_MIN, RANDOMIZER_MAX);
            //if (player == Player.Black)
            //{
            //    return score - whiteCanBeCaptured;
            //}
            //else
            //{
            //    return -score + blackCanBeCaptured;
            //}
        }

        ///// <summary>
        ///// huristic function- foreach soldier of player calculate the follwong parameters: is a king, is safe, can capture, can be captured
        ///// </summary>
        ///// <param name="board"></param>
        ///// <param name="player"></param>
        ///// <returns></returns>
        //public int Evaluate(Board board, Player player)
        //{
        //    //PrintBoardState print = new PrintBoardState();
        //    //print.DrawBoard(board);
        //    const int kingConstant = 10;
        //    const int soldierConstant = 1;
        //    int numOfWhiteSold = board.NumberOfWhitePieces; 
        //    int numOfBlackSold = board.NumberOfBlackPieces;
        //    int diffSold = 0;
        //    int playerSum = 0;
        //    int opponentSum = 0;
        //    int diffKings, diffCaptures, diffCanBeCaptured = 0;
        //    int numPlyKings = 0, numPlycap = 0, numPlyCanBeCap = 0, numOppkings = 0, numOppcap = 0, numOppCabBeCap = 0;
        //    for (int i = 1; i <= 32; i++)
        //    {
        //        int coordSum = 0;
        //        if (board.IsKing(board[i]))
        //            coordSum += kingConstant;
        //        //else if (board.IsSoldier(board[i]))
        //        //  coordSum += soldierConstant;

        //        int Safe = Safeness(board, board[i], board.GetPlayer(board[i]));
        //        coordSum += Safe;
        //        int cap = CanCapture(board, board[i], board.GetPlayer(board[i]));
        //        coordSum += cap;
        //        int close = CloseToBecomeAKing(board[i]);
        //        coordSum += close;
        //        int captured = CanBeCaptured(board, board[i], board.GetPlayer(board[i]));
        //        coordSum += captured;

        //        if (board.IsOwner(player, board[i]))
        //        {
        //            playerSum += coordSum;
        //            numPlycap += cap;
        //            numPlyCanBeCap += captured;
        //        }
        //        else if (board.IsOwner(board.GetOpponent(player), board[i]))
        //        {
        //            opponentSum += coordSum;
        //            numOppcap += cap;
        //            numOppCabBeCap += captured;
        //        }
        //    }
        //    if (player == Player.Black)
        //    {
        //        numPlyKings = board.NumberOfBlackKings;
        //        numOppkings = board.NumberOfWhiteKings;
        //        diffSold = numOfBlackSold - numOfWhiteSold;

        //    }
        //    else
        //    {
        //        numPlyKings = board.NumberOfWhiteKings;
        //        numOppCabBeCap = board.NumberOfBlackKings;
        //        diffSold = numOfWhiteSold - numOfBlackSold;
        //    }
        //    diffKings = numPlyKings - numOppkings;
        //    diffCaptures = numPlycap - numOppcap;
        //    diffCanBeCaptured = numPlyCanBeCap - numOppCabBeCap;
        //    return
        //        (int) Math.Round(((float) diffKings*0.3) + ((float) diffCaptures*0.5) +((float)diffSold*0.9) - ((float) diffCanBeCaptured*1.5));
        //    //return playerSum - opponentSum;
        //}


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
            IDictionary<IList<Coordinate>, Coordinate> captures = new Dictionary<IList<Coordinate>, Coordinate>();
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
            foreach (var CID in coordsInDir)
            {
                if (board.GetPlayer(board[CID.X, CID.Y]) == board.GetOpponent(player) &&
                    rule.CoordsToCaptureAndDest(board, CID, coord, board.GetOpponent(player)).Count > 0)
                    num++;
            }
            foreach (var CFB in coordsfrombehind)
            {
                if (board.GetPlayer(board[CFB.X, CFB.Y]) == board.GetOpponent(player) && board.IsKing(coord) &&
                    rule.CoordsToCaptureAndDest(board, CFB, coord, board.GetOpponent(player)).Count > 0)
                    num++;
            }
            return num;
        }

        public int Evaluate2(Board board, Player player)
        {
            int score = 0;
            int numWhiteKings = board.NumberOfWhiteKings;
            int numBlackKings = board.NumberOfBlackKings;
            int numOfWhiteSold = board.NumberOfWhitePieces;
            int numOfBlackSold = board.NumberOfBlackPieces;

            int blackScore = (numOfBlackSold) + (numBlackKings);
            int whiteScore = (numOfWhiteSold) + (numWhiteKings);

            score += blackScore - whiteScore;
            return (player == Player.Black) ? score : -score;
        }
    }
}