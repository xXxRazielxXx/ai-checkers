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
            int numWhiteKings = board.NumberOfWhiteKings;
            int numBlackKings = board.NumberOfBlcakKings;
            int numOfWhiteSold = board.NumberOfWhitePieces; ;
            int numOfBlackSold = board.NumberOfBlackPieces; ;

            return (((numOfBlackSold*soldierConstant) + (numBlackKings*kingConstant)) -
                    ((numOfWhiteSold*soldierConstant) + (numWhiteKings*kingConstant)));
        }
    }
}