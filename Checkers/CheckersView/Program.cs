using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using CheckersEngine;
using CheckersModel;
using Interfaces;
using checkersengine;

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
            Console.WriteLine("for PC vs Human Press 1, for PC vs PC press 2");
            string type = Console.ReadLine();
            if (type == "1")
            {
                p.StartGameWithHuman(board);
            }
            else if (type == "2")
            {
                p.StartGameWithPC(board);
            }
         
            return;            
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
            int depth = 5;            
            while (true)
            {
                //first turn is always the human's
                Console.WriteLine("Your turn, please enter a move in format 'from cell number' 'to cell number'");
            HumanTurn:
                bool capMove = false;
                var input=Console.ReadLine();      
                IList<Coordinate> coords = ParseStrToCoords(input, board);
                if (coords == null)
                {
                    Console.WriteLine("Wrong Input");
                    goto HumanTurn; 
                }

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
                   Console.WriteLine("This is not your piece , please enter cell number which allocated with your piece color");
                   goto HumanTurn;
                }
                IDictionary<IList<Coordinate>, IList<Coordinate>> capturesAvailable = rule.FindCaptures(board, humanColor);
                if (capturesAvailable.Count > 0)
                {
                    IList<Coordinate> captures = MapContainsCoords(capturesAvailable, srcCoord, destCoord);
                    if (captures.Count == 0)
                    {
                        Console.WriteLine("You must capture maximum opponent soldiers on board");
                        goto HumanTurn;
                    }
                    else
                    {
                        foreach (var coordinate in captures)
                        {
                            board[coordinate.X,coordinate.Y].Status=Piece.None;
                            board.UpdateCapturedSoldiers(coordinate,pcColor);
                            capMove = true;
                        }
                    }
                }                
                if (capMove || rule.IsValidMove(board,srcCoord,destCoord, humanColor))
                {
                    board.UpdateBoard(srcCoord,destCoord);
                    rule.IsBecameAKing(board, board[destCoord.X, destCoord.Y]);
                    print.DrawBoard(board);
                }
                else
                {
                    Console.WriteLine("This is not a valid move, please enter again");
                    goto HumanTurn;
                }

                GameState game = GetGameState(humanColor, board);
                if (game == GameState.Lost)
                {
                    Console.WriteLine("{0} Lost the game and {1} won", humanColor.ToString(), pcColor.ToString());
                    break;
                }
                if (game == GameState.Won)
                {
                    Console.WriteLine("{0} Won", humanColor.ToString());
                    break;
                }


                ShowPlayerChange(pcColor);
                //var miniMax =new MiniMax();
                var alphaBeta = new Alphabeta();
                Board temp = new Board();                
                depth = rule.DefineDepth(board);
                alphaBeta.AlphaBeta(board, depth, Int32.MinValue, Int32.MaxValue, pcColor, true, ref srcCoord,ref destCoord, ref temp);
                //miniMax.MinMax(board, depth, pcColor, true, ref srcCoord, ref destCoord, ref temp);
                if ((rule.InBounds(board, srcCoord.X, srcCoord.Y)) && (rule.InBounds(board, destCoord.X, destCoord.Y)))
                {
                    board = temp.Copy();
                    print.DrawBoard(board);
                }
                game = GetGameState(humanColor, board);
                if (game == GameState.Lost)
                {
                    Console.WriteLine("{0} Lost the game and {1} won",humanColor.ToString(),pcColor.ToString());
                    break;
                }
                if (game == GameState.Won)
                {
                    Console.WriteLine("{0} Won",humanColor.ToString());
                    break;
                }
            }
        }

        /// <summary>
        /// Parse input to coordinates
        /// </summary>
        /// <param name="input"></param>
        /// <param name="board"></param>
        /// <returns></returns>
        public IList<Coordinate> ParseStrToCoords(string input, Board board)
        {
            IList<Coordinate> moves= new List<Coordinate>();
            const char delimiterChar = ' ';
            string[] word = input.Split(delimiterChar);
            try
            {
            Coordinate srcCoord = new Coordinate(board[Int32.Parse(word[0])]);
            moves.Add(srcCoord);
            Coordinate destCoord = new Coordinate(board[Int32.Parse(word[1])]);
            moves.Add(destCoord);
            }
            catch (Exception e) { return null; }            
            return moves;
        }

        /// <summary>
        /// Check GameState
        /// </summary>
        /// <param name="player"></param>
        /// <param name="board"></param>
        /// <returns></returns>
        public GameState GetGameState(Player player, Board board)
        {
            var rule = new Rules();

            var numberplayerPieces = rule.NumberOfPlayerPieces(board, player);
            var isplayerBlocked = rule.IsPlayerBlocked(board, player);
            if (numberplayerPieces == 0 || isplayerBlocked)
            {
                return GameState.Lost;
            }
            var numberopponentPieces = rule.NumberOfPlayerPieces(board, board.GetOpponent(player));
            var isopponentBlocked = rule.IsPlayerBlocked(board, board.GetOpponent(player));
            if (numberopponentPieces == 0 || isopponentBlocked)
            {
                return GameState.Won;
            }
            else
            {
                return GameState.Undetermined;
            }
        }


        /// <summary>
        /// Checks if the move (source, dest) is contained in the map
        /// </summary>
        /// <param name="capturesAvailable"></param>
        /// <param name="srcCoord"></param>
        /// <param name="destCoord"></param>
        /// <returns> if move contained in map returns the captured coordinates, else return empty (move isn't in map)</returns>
        public IList<Coordinate> MapContainsCoords(IDictionary<IList<Coordinate>, IList<Coordinate>> capturesAvailable,
                                      Coordinate srcCoord, Coordinate destCoord)
        {
            IList<Coordinate> captures=new List<Coordinate>();
            foreach (var item in capturesAvailable)
            {
                if (item.Key.First().X == srcCoord.X && item.Key.First().Y == srcCoord.Y && item.Key.Last().X == destCoord.X && item.Key.Last().Y == destCoord.Y)
                {
                    captures = item.Value;
                    return captures;
                }
            }
            return captures;
        }


        public void StartGameWithPC(Board board)
        {
            var oppColor = Player.None;
            var pcColor = Player.None;
            //bool firstTurn = true;
            var rule = new Rules();
            var print = new PrintBoardState();
            int depth = 5;
            Coordinate srcCoord = new Coordinate();
            Coordinate destCoord = new Coordinate();

            FileEngine file = new FileEngine();

            //define path to share file.
            Console.WriteLine("Please Enter Path Line:");
            string path = Console.ReadLine();

            //define colors.
            Console.WriteLine("Opponent color is white? [Yes/No]");
            string opponentColor = Console.ReadLine();
            oppColor = opponentColor == "Yes" ? Player.White : Player.Black;
            pcColor = oppColor == Player.White ? Player.Black : Player.White;

            //define who starts.
            Console.WriteLine("Opponent Starts? [Yes/No]");
            string opponentStarts = Console.ReadLine();
            if (opponentStarts == "Yes")
                goto OppTurn;
            else
                goto MyTurn;


        OppTurn:
            try
            {
                using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    bool capMove = false;
                    IList<Coordinate> oppMove = new List<Coordinate>();
                    while (oppMove.Count == 0)
                    {
                        oppMove = file.ReadFromFile(stream,board, path);
                    }                    
                    srcCoord = oppMove.First();
                    destCoord = oppMove.Last();
                    if (board.GetPlayer(destCoord) != oppColor)
                    {
                        //Console.WriteLine("This is Not your piece");
                        stream.Close();
                        goto OppTurn;
                    }
                    IDictionary<IList<Coordinate>, IList<Coordinate>> capturesAvailable = rule.FindCaptures(board,
                                                                                                            oppColor);
                    if (capturesAvailable.Count > 0)
                    {
                        IList<Coordinate> captures = MapContainsCoords(capturesAvailable, srcCoord, destCoord);
                        if (captures.Count == 0)
                        {
                            Console.WriteLine("You must capture maximum opponent soldiers on board");
                            goto OppTurn;
                        }
                        else
                        {
                            foreach (var coordinate in captures)
                            {
                                board[coordinate.X, coordinate.Y].Status = Piece.None;
                                board.UpdateCapturedSoldiers(coordinate, pcColor);
                                capMove = true;
                            }
                        }
                    }
                    if (capMove || rule.IsValidMove(board, srcCoord, destCoord, oppColor))
                    {
                        board.UpdateBoard(srcCoord, destCoord);
                        rule.IsBecameAKing(board, board[destCoord.X, destCoord.Y]);
                        print.DrawBoard(board);
                    }
                    else
                    {
                        Console.WriteLine("This is not a valid move, please enter again");
                        goto OppTurn;
                    }

                    //check if game has been determined
                    GameState game = GetGameState(oppColor, board);
                    if (game == GameState.Lost)
                    {
                        Console.WriteLine("{0} Lost the game and {1} won", oppColor.ToString(), pcColor.ToString());
                        return;
                    }
                    if (game == GameState.Won)
                    {
                        Console.WriteLine("{0} Won", oppColor.ToString());
                        return;
                    }
                }
            }
            catch (Exception)
            {

                goto OppTurn;
            }


            MyTurn:
            try
            {
                using (var stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
                {
                    ShowPlayerChange(pcColor);
                    var alphaBeta = new Alphabeta();
                    Board temp = new Board();
                    alphaBeta.AlphaBeta(board, depth, Int32.MinValue, Int32.MaxValue, pcColor, true, ref srcCoord,
                                        ref destCoord,
                                        ref temp);
                    if ((rule.InBounds(board, srcCoord.X, srcCoord.Y)) &&
                        (rule.InBounds(board, destCoord.X, destCoord.Y)))
                    {
                        board = temp.Copy();
                        print.DrawBoard(board);
                    }
                    file.WriteToFile(stream, srcCoord, destCoord, path);
                    GameState game = GetGameState(oppColor, board);
                    if (game == GameState.Lost)
                    {
                        Console.WriteLine("{0} Lost the game and {1} won", oppColor.ToString(), pcColor.ToString());
                        return;
                    }
                    if (game == GameState.Won)
                    {
                        Console.WriteLine("{0} Won", oppColor.ToString());
                        return;
                    }
                }
                Thread.Sleep(5000);
                goto OppTurn;
            }
            catch (Exception e)
            {
                goto MyTurn;
            }
        }
    }
}
