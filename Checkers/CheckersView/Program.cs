using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using CheckersEngine;
using CheckersModel;
using Interfaces;
using checkersengine;

namespace CheckersView
{
    internal class Program
    {
        /// <summary>
        /// Console application entry 
        /// </summary>
        private static void Main()
        {
            bool input = true;
            var print = new PrintBoardState();
            //Create new board
            var board = new Board(8);
            Console.WriteLine("Game started");
            //print start board
            print.DrawBoard(board);
            Program p = new Program();
            Console.WriteLine("for PC vs Human Press 1, for PC vs PC press 2");
            while (input)
            {
                string type = Console.ReadLine();
                if (type == "1")
                {
                    input = false;
                    p.StartGameWithHuman(board);
                }
                else if (type == "2")
                {
                    input = false;
                    p.StartGameWithPc(board);
                }
                else
                {
                    Console.WriteLine("Incorrect Input, please try again");
                }
            }
        }

        /// <summary>
        /// Shows Current Player Color
        /// </summary>
        /// <param name="turn"></param>
        public void ShowPlayerChange(Player turn)
        {
            Console.WriteLine("{0}'s turn", turn.ToString());
        }

        /// <summary>
        /// Human VS PC
        /// </summary>
        /// <param name="board"></param>
        public void StartGameWithHuman(Board board)
        {
            int countmovesforDraw = 0;
            var humanColor = Player.None;
            var pcColor = Player.None;
            bool firstTurn = true;
            var rule = new Rules();
            var print = new PrintBoardState();
            while (true)
            {
                //first turn is always the human's
                Console.WriteLine("Your turn, please enter a move in format 'from cell number' 'to cell number'");
                HumanTurn:
                bool capMove = false;
                var input = Console.ReadLine();
                IList<Coordinate> coords = ParseStrToCoords(input, board);
                //If there is no input
                if (coords == null)
                {
                    Console.WriteLine("Wrong Input, please try again");
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
                    //if select coordinate is not your coordinate then show error message
                else if (humanColor != board.GetPlayer(srcCoord))
                {
                    Console.WriteLine(
                        "This is not your piece , please enter cell number which allocated with your piece color");
                    goto HumanTurn;
                }

                //Fins captures
                IDictionary<IList<Coordinate>, IList<Coordinate>> capturesAvailable = rule.FindCaptures(board,
                                                                                                        humanColor);
                //if there are any captures
                if (capturesAvailable.Count > 0)
                {
                    //check if they are contained in captured dictionary by looking on source and destination
                    IList<Coordinate> captures = rule.MapContainsCoords(capturesAvailable, srcCoord, destCoord);
                    //id they are not contained show error message
                    if (captures.Count == 0)
                    {
                        Console.WriteLine("You must capture maximum opponent soldiers on board");
                        goto HumanTurn;
                    }
                    else
                    {
                        //if there are captures and source and destination are corrrect, update board with move and captures
                        foreach (var coordinate in captures)
                        {
                            board[coordinate.X, coordinate.Y].Status = Piece.None;
                            board.UpdateCapturedSoldiers(coordinate, pcColor);
                            capMove = true;
                        }
                    }
                }
                //update board
                if (capMove || rule.IsValidMove(board, srcCoord, destCoord, humanColor))
                {
                    board.UpdateBoard(srcCoord, destCoord);
                    rule.IsBecameAKing(board, board[destCoord.X, destCoord.Y]);
                    print.DrawBoard(board);
                }
                else
                {
                    Console.WriteLine("This is not a valid move, please enter again");
                    goto HumanTurn;
                }

                //check if game is finish by checking if players draw,lost or won
                GameState game = GetGameState(humanColor, board);
                if (GameState.Draw == CheckDraw(board, board[destCoord.X, destCoord.Y], capMove, ref countmovesforDraw))
                {
                    Console.WriteLine("Draw");
                    break;
                }
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

                //switch to opponent (PC)
                ShowPlayerChange(pcColor);
                var alphaBeta = new Alphabeta();
                Board temp = new Board();
                IList<Coordinate> tempCaptures = new List<Coordinate>();
                //Define depth
                int depth = rule.DefineDepth(board);
                //Calling alpha-beta algorithm which return the best move in addition, return source,destination coordinates and a list of captures
                alphaBeta.AlphaBeta(board, depth, Int32.MinValue, Int32.MaxValue, pcColor, true, ref srcCoord,
                                    ref destCoord, ref temp, ref tempCaptures);

                //Verify the move is in bounds, if yes update the board with it
                if ((rule.InBounds(board, srcCoord.X, srcCoord.Y)) && (rule.InBounds(board, destCoord.X, destCoord.Y)))
                {
                    board = temp.Copy();
                    print.DrawBoard(board);
                }
                // check if there were any captures. if yes check for draw
                bool pcCaptured = tempCaptures.Count > 0;
                //check if game was determined
                game = GetGameState(humanColor, board);
                if (GameState.Draw ==
                    CheckDraw(board, board[destCoord.X, destCoord.Y], pcCaptured, ref countmovesforDraw))
                {
                    Console.WriteLine("Draw");
                    break;
                }
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
            IList<Coordinate> moves = new List<Coordinate>();
            const char delimiterChar = ' ';
            string[] word = input.Split(delimiterChar);
            try
            {
                Coordinate srcCoord = new Coordinate(board[Int32.Parse(word[0])]);
                moves.Add(srcCoord);
                Coordinate destCoord = new Coordinate(board[Int32.Parse(word[1])]);
                moves.Add(destCoord);
            }
            catch (Exception)
            {
                return null;
            }
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
        /// Checks if draw and return number of moves (draw - if there are 15 king's moves with no captures of both opponent and player)
        /// </summary>
        /// <param name="board"></param>
        /// <param name="coordinate"></param>
        /// <param name="canCapture"></param>
        /// <param name="count"></param>
        public GameState CheckDraw(Board board, Coordinate coordinate, bool canCapture, ref int count)
        {
            if (board.IsKing(coordinate) && !canCapture)
            {
                count++;
                if (count == 15)
                {
                    return GameState.Draw;
                }
            }
            else
            {
                count = 0;
            }
            return GameState.Undetermined;
        }

        /// <summary>
        /// Pc VS Pc
        /// </summary>
        /// <returns> </returns>
        public void StartGameWithPc(Board board)
        {
            int countmovesforDraw = 0;
            var rule = new Rules();
            var print = new PrintBoardState();
            int depth;
            Coordinate srcCoord = new Coordinate();
            Coordinate destCoord = new Coordinate();

            //Create the file engine who Read\write to file all moves
            FileEngine file = new FileEngine();

            //define that file will be created in the root folder under the name sync.txt
            string path = "sync.txt";


            //define colors.
            Console.WriteLine("Opponent color is white? [Yes/No]");
            Opponet:
            string opponentColor = Console.ReadLine();
            if (!(opponentColor == "Yes" || opponentColor == "yes" || opponentColor == "No" || opponentColor == "no"))
            {
                Console.WriteLine("Invalid input,please try again");
                goto Opponet;
            }
            Player oppColor = (opponentColor == "Yes" || opponentColor == "yes") ? Player.White : Player.Black;
            Player pcColor = oppColor == Player.White ? Player.Black : Player.White;

            //define who starts.
            Console.WriteLine("Opponent Starts? [Yes/No]");
            Start:
            string opponentStarts = Console.ReadLine();
            if (
                !(opponentStarts == "Yes" || opponentStarts == "yes" || opponentStarts == "No" || opponentStarts == "no"))
            {
                Console.WriteLine("Invalid input,please try again");
                goto Start;
            }
            if (opponentStarts == "Yes" || opponentStarts == "yes")
                goto OppTurn;
            goto MyTurn;

            //oppoenent tuen
            OppTurn:
            try
            {
                //open file in path with read permision and no sharing
                using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    ShowPlayerChange(oppColor);
                    Player color = Player.None;
                    bool capMove = false;
                    IList<Coordinate> oppMove = new List<Coordinate>();
                    //read moves and captures (if exists)
                    while (oppMove.Count == 0)
                    {
                        oppMove = file.ReadFromFile(stream, board, path, out color);
                    }
                    //Source
                    srcCoord = oppMove.First();
                    oppMove.RemoveAt(0);
                    //Destination
                    destCoord = oppMove[0];
                    oppMove.RemoveAt(0);
                    //Captures list
                    var capturesOppdid = oppMove; 
                    //if move is not oppoent move or source piece is not opponenet color return to read file
                    if ((color != oppColor) || (board.GetPlayer(srcCoord) != oppColor))
                    {
                        goto OppTurn;
                    }

                    //Find captures
                    IDictionary<IList<Coordinate>, IList<Coordinate>> capturesAvailable = rule.FindCaptures(board,
                                                                                                            oppColor);
                    if (capturesAvailable.Count > 0)
                    {
                        var boolean = rule.MapContainsCoordsOfCaptures(capturesAvailable, srcCoord, destCoord,
                                                                       capturesOppdid);
                        if (!boolean)
                        {
                            Console.WriteLine("You must capture maximum opponent soldiers on board");
                            goto OppTurn;
                        }
                        else
                        {
                            foreach (var coordinate in capturesOppdid)
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
                    if (GameState.Draw ==
                        CheckDraw(board, board[destCoord.X, destCoord.Y], capMove, ref countmovesforDraw))
                    {
                        Console.WriteLine("Draw");
                        return;
                    }
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

            //local turn
            MyTurn:
            try
            {
                using (var stream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
                {
                    ShowPlayerChange(pcColor);
                    var alphaBeta = new Alphabeta();
                    Board temp = new Board();
                    IList<Coordinate> tempCaptures = new List<Coordinate>();
                    depth = rule.DefineDepth(board);
                    alphaBeta.AlphaBeta(board, depth, Int32.MinValue, Int32.MaxValue, pcColor, true, ref srcCoord,
                                        ref destCoord,
                                        ref temp, ref tempCaptures);
                    if ((rule.InBounds(board, srcCoord.X, srcCoord.Y)) &&
                        (rule.InBounds(board, destCoord.X, destCoord.Y)))
                    {
                        board = temp.Copy();
                        print.DrawBoard(board);
                    }

                    //write move to file
                    file.WriteToFile(stream, srcCoord, destCoord, tempCaptures, path, pcColor);

                    //check if game has been determined
                    bool pcCaptured = tempCaptures.Count > 0;
                    GameState game = GetGameState(oppColor, board);
                    if (GameState.Draw ==
                        CheckDraw(board, board[destCoord.X, destCoord.Y], pcCaptured, ref countmovesforDraw))
                    {
                        Console.WriteLine("Draw");
                        return;
                    }
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
            catch (Exception)
            {
                goto MyTurn;
            }
        }
    }
}