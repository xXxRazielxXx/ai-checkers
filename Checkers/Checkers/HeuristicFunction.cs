using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CheckersModel;

namespace CheckersEngine
{
    public class HeuristicFunction
    {
        // calculates the pieces 'safeness' 
        // assumption: the computer is the black player
        public int Evaluate(Board board)
        {
            const int kingConstant = 10;
            const int soldierConstant = 1;
            int numOfWhites = board.NumberOfWhitePieces;
            int numOfBlacks = board.NumberOfBlackPieces;
            int numWhiteKings = board.NumberOfWhiteKings;
            int numBlackKings = board.NumberOfBlcakKings;

            int numOfWhiteSold = (numOfWhites - numWhiteKings);
            int numOfBlackSold = (numOfBlacks - numBlackKings);

            return (((numOfBlackSold*soldierConstant) + (numBlackKings*kingConstant)) -
                    ((numOfWhiteSold*soldierConstant) + (numWhiteKings*kingConstant)));
        }
    }
}