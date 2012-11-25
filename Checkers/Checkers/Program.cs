
using CheckersModel;

namespace CheckersEngine
{
    internal class Program
    {
        private static void Main()
        {
            var board = new Board(8);
            var print = new PrintBoardState();
            print.DrawBoard(board);
        }
    }
}