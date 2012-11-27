using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CheckersEngine;
using CheckersModel;

namespace CheckersView
{
    class Program
    {
        static void Main(string[] args)
        {
            var print = new PrintBoardState();
            var board = new Board(8);
            Console.WriteLine("Game started");
            print.DrawBoard(board);
        }

        public void ShowPlayerChange(Player turn)
        {
            Console.WriteLine("{0}'s turn", turn.ToString());
            
        }

        public void StartGameWithHuman(Board board)
        {
            var rule = new Rules();
            int depth = 7;
            while (true)
            {
                //first turn is always the human's
                var input=Console.ReadLine();
                IList<Coordinate> coords = new List<Coordinate>();
               // coords = ParseStrToCoords(input);
                var srcCoord = coords.First();
                var destCoord = coords.Last();
                Player humanColor = board.GetPlayer(srcCoord);
                Player pcColor = board.GetOpponent(humanColor);
                if ((rule.InBounds(board, srcCoord.X, srcCoord.Y)) && (rule.InBounds(board, destCoord.X, destCoord.Y)))
                {
                    board.UpdateBoard(srcCoord,destCoord);// What if human captured a pc soldier
                }
                ShowPlayerChange(pcColor);
                var miniMax =new MiniMax();
                miniMax.MinMax(board, depth, pcColor, true,ref srcCoord,ref destCoord);









            }
        }

    }
}
