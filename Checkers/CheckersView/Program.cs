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
            Program p = new Program();
            p.StartGameWithHuman(board);
        }

        public void ShowPlayerChange(Player turn)
        {
            Console.WriteLine("{0}'s turn", turn.ToString());
            
        }

        public  void StartGameWithHuman(Board board)
        {
            var humanColor = Player.None;
            var pcColor = Player.None;
            bool firstTurn = true;
            var rule = new Rules();
            var print = new PrintBoardState();
            int depth = 7;
            while (true)
            {
                //first turn is always the human's
                Console.WriteLine("Your turn, please enter a move in format 'from cell number' 'to cell number'");
            HumanTurn:
                var input=Console.ReadLine();
                IList<Coordinate> coords = ParseStrToCoords(input, board);
                var srcCoord = coords.First();
                var destCoord = coords.Last();
                if (firstTurn)
                {
                    humanColor = board.GetPlayer(srcCoord);
                    pcColor = board.GetOpponent(humanColor);
                    firstTurn = false;
                }
                else if (humanColor != board.GetPlayer(srcCoord))
                {
                   Console.WriteLine("Wrong Input");
                   goto HumanTurn;
                }
                if ((rule.InBounds(board, srcCoord.X, srcCoord.Y)) && (rule.InBounds(board, destCoord.X, destCoord.Y))&&board.IsValidMove(srcCoord,destCoord))
                {
                    board.UpdateBoard(srcCoord,destCoord);// What if human captured a pc soldier
                    print.DrawBoard(board);
                }
                else
                {
                    Console.WriteLine("Wrong Input");
                    goto HumanTurn;
                }
                ShowPlayerChange(pcColor);
                var miniMax =new MiniMax();
                miniMax.MinMax(board, depth, pcColor, true,ref srcCoord,ref destCoord);
                if ((rule.InBounds(board, srcCoord.X, srcCoord.Y)) && (rule.InBounds(board, destCoord.X, destCoord.Y)) && board.IsValidMove(srcCoord, destCoord))
                {
                    board.UpdateBoard(srcCoord, destCoord);
                    print.DrawBoard(board);
                }
            }
        }

        public IList<Coordinate> ParseStrToCoords(string input, Board board)
        {
            IList<Coordinate> moves= new List<Coordinate>();
            char delimiterChar = ' ';
            string[] word = input.Split(delimiterChar);
            Coordinate srcCoord = board[Int32.Parse(word[0])];
            moves.Add(srcCoord);
            Coordinate destCoord = board[Int32.Parse(word[1])];
            moves.Add(destCoord);
            return moves;
        }

    }
}
