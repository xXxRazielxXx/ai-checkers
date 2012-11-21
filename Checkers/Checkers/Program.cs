namespace Checkers
{
    internal class Program
    {
        private static void Main()
        {
            Rules rules = new Rules();
            
            var board = new Board();
            board.InitializeBoard(8);
            rules.ValidMoves(board,board[31]);
        }
    }
}