﻿namespace CheckersModel
{
    /// <summary>
    /// Different pieces types
    /// </summary>
    public enum Piece
    {
        /// <summary>
        /// This indicates an invalid piece.  i.e. Invalid square
        /// </summary>
        Illegal,
        /// <summary>
        /// Empty player on Coord
        /// </summary>
        None,
        /// <summary>
        /// Black player on Coord
        /// </summary>
        BlackPiece,
        /// <summary>
        /// White player on Coord
        /// </summary>
        WhitePiece,
        /// <summary>
        /// Black king piece
        /// </summary>
        BlackKing,
        /// <summary>
        /// White king piece
        /// </summary>
        WhiteKing
    }
}