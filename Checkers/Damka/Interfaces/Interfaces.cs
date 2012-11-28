using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

    //public enum CellType 
    //{
    //    Empty=Piece.None,
    //    Black=Piece.BlackPiece,
    //    White=Piece.WhitePiece,
    //    BlackQueen=Piece.BlackKing,
    //    WhiteQueen=Piece.WhiteKing
    //}

    //public enum Player
    //{
    //    Black=CheckersModel.Player.Black,
    //    White=CheckersModel.Player.White
    //}

    public interface IMove
    {
        IBoardState Board { get; }
    }

    public enum GameState
    {
        Won,
        Lost,
        Undetermined
    }

    public interface IBoardState
    {
        Piece[,] BoardCells { get; set; }
        IBoardState GetBoardState(Player player, MoveType moveType, Point position);
        GameState GetGameState(Player player);
    }

    public interface IPlayer
    {
        IMove GetBoardMove(IBoardState boardState);
    }
    public interface IPlayerCreator
    {
        IPlayer CreatePcPlayer(Player playerColor);
    }
}
