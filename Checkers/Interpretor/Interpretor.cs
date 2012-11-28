using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using CheckersModel;
using Interfaces;


namespace Interpretor
{
    public class PlayerCreator:IPlayerCreator
    {

        public IPlayer CreatePcPlayer(Player playerColor)
        {
            IPlayer play = new Play(playerColor);
            return play;
        }
    }

    public class Move:IMove
    {
        public IBoardState Board { get; private set; }
    }

    public class Play:IPlayer
    {
        private Player playerColor;
        public Play(Player color)
        {
            this.playerColor = color;
        }
        public IMove GetBoardMove(IBoardState boardState)
        {
            throw new NotImplementedException();
        }
    }

    public class BoardState:IBoardState
    {
        private readonly Board board;
        private Piece[,] boardCells;
        public BoardState(int size)
        {
            this.board = new Board(size);
            this.boardCells = new Piece[8, 8];
            int k = 32;
                for (int i = 0; i < board.Rows; i++)
                {
                    int j = i%2 == 0 ? 1 : 0;
                    
                    for (;j < board.Columns;k--, j+=2)
                    {
                        boardCells[i , j] = board[k].Status;
                    }
                }
        }
        public Piece[,] BoardCells
        {
            get
            {
                return boardCells;
            }
            set
            {
                boardCells=value;
            }
        }
        public IBoardState GetBoardState(Player player, MoveType moveType, Point position)
        {
            throw new NotImplementedException();
        }

        public GameState GetGameState(Player player)
        {
            throw new NotImplementedException();
        }
    }
}
