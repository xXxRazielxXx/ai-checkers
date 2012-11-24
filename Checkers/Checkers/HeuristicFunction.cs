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
            int king_constant=10;
            int soldier_constant=1;
            int NumOfWhites = board.NumberOfWhitePieces;
            int NumOfBlacks = board.NumberOfBlackPieces;
            int NumWhiteKings = board.NumberOfWhiteKings;
            int NumBlackKings = board.NumberOfBlcakKings;

            int num_of_white_sold = (NumOfWhites - NumWhiteKings);
            int num_of_black_sold = (NumOfBlacks - NumBlackKings);

            return (((num_of_black_sold * soldier_constant) + (NumBlackKings* king_constant)) -((num_of_white_sold*soldier_constant)+(NumWhiteKings*king_constant)));
        }
    }
}
