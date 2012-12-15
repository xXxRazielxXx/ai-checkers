using System.Drawing;
using CheckersModel;

namespace Interfaces
{
    public enum MoveType
    {
        Unknown,
        UpperRight,
        UpperLeft,
        BottomRight,
        BottomLeft
    }

    public interface IMove
    {
        IBoardState Board { get; }
    }

    public enum GameState
    {
        Won,
        Lost,
        Undetermined,
        Draw
    }

    public interface IBoardState
    {
        Piece[,] BoardCells { get; set; }
        Board Board { get; set; }
        GameState DrawGame { get; set; }
        Piece[,] ConvertBoardToBoardState(Board ourBoard);
        Board ConvertBoardStateToBoard(IBoardState boardState);
        Point ConvertPointToCoordinate(int x, int y);
        Point ConvertMoveTypeToCoordinate(Point position, MoveType move);
        IBoardState GetBoardState(Player player, MoveType moveType, Point position, out bool needToContinueEating,out bool mustCapture);
        GameState GetGameState(Player player);
        GameState CheckDraw(Board board, Coordinate coordinate, bool captured);
    }

    public interface IPlayer
    {
        IMove GetBoardMove(IBoardState boardState,int depth);
    }
    public interface IPlayerCreator
    {
        IPlayer CreatePcPlayer(Player playerColor);
    }
}
