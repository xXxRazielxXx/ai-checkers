
using CheckersModel;

namespace CheckersEngine
{
    internal class Program
    {
        private static void Main()
        {
            var board = new Board();
            board.InitializeBoard(8);
            var print = new PrintBoardState();
            print.DrawBoard(board);
        }
    }
}