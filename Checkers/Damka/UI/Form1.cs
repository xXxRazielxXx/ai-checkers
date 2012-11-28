using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CheckersModel;
using Interfaces;


namespace UI
{
    public partial class Form1 : Form
    {
        private IBoardState currBoard;
        private PictureBox[,] squares;
        private Dictionary<int, SquareData> squaresData;
        private IBoardState initialBoard;

        private Player player;
        private Dictionary<Player, IPlayer> players;
        private IPlayerCreator playerCreator;
        public Form1(IPlayerCreator playerCreator,IBoardState initialBoard)
        {
            this.initialBoard = initialBoard;
            squaresData = new Dictionary<int, SquareData>();
            this.playerCreator = playerCreator;

            InitializeComponent();
        }

        public static System.Drawing.Bitmap Resize1(System.Drawing.Bitmap value, int newWidth, int newHeight)
        {
            System.Drawing.Bitmap resizedImage = new System.Drawing.Bitmap(newWidth, newHeight);
            System.Drawing.Graphics.FromImage((System.Drawing.Image)resizedImage).DrawImage(value, 0, 0, newWidth, newHeight);
            return (resizedImage);
        }

        #region PaintBoard
        public void PaintBoard()
        {
            this.CreateBoard();
            this.PaintTools();
            this.PaintSquareNumbers();

        }

        public void CreateBoard()
        {
            this.boardPanel.Controls.Clear();

            int squreSize = (int)(boardPanel.Width / 8);
            squares = new PictureBox[8, 8];
            for (int i = 0; i < 8; i++)
            {
                int delta = ((i % 2) == 0) ? 0 : 1;
                for (int j = 0; j < 8; j++)
                {
                    squares[i, j] = new PictureBox();
                    squares[i, j].Click += new EventHandler(OnTileClicked);
                    squares[i, j].Size = new Size(squreSize,squreSize);

                    bool isDark = true;
                    Image img = Bitmap.FromFile(@"F:\Projects\Checkers\Checkers\Damka\UI\darkTile.png");
                    if ((j+delta) % 2 == 0)
                    {
                        isDark = false;
                        img = Bitmap.FromFile(@"F:\Projects\Checkers\Checkers\Damka\UI\lightTile.png");
                    }
                    squares[i, j].Tag = new SquareData(isDark,new Point(j,i));
                    squares[i, j].Image = Form1.Resize1(new Bitmap(img), squreSize, squreSize);
                    squares[i, j].BackColor = Color.Transparent;
                }
            }

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    squares[i, j].Location = new Point(i * squreSize, j * squreSize);
                    this.boardPanel.Controls.Add(squares[i, j]);
                }
            }
        }

        private SquareData srcMove = null;
        void OnTileClicked(object sender, EventArgs e)
        {
            if ((players == null) ||
                !(players[player] is UserPlayer))
                return;

            if (srcMove == null)
            {
                srcMove = (SquareData)((PictureBox)sender).Tag;
            }
            else
            {
                SquareData dstMove = (SquareData)((PictureBox)sender).Tag;
                this.PerformMove(srcMove.Number, dstMove.Number);
                srcMove = null;
            }
        }

        private void PaintTools()
        {
            int squreSize = (int)(boardPanel.Width / 8);
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    SquareData squareData = (SquareData)this.squares[i,j].Tag;
                    if (currBoard.BoardCells[i, j] == Piece.None)
                        continue;

                    Image img = Bitmap.FromFile(@"F:\Projects\Checkers\Checkers\Damka\UI\blackOnDarkTile.png");
                    if (currBoard.BoardCells[i, j] == Piece.BlackPiece)
                    {
                        if (squareData.IsDark)
                        {
                            img = Bitmap.FromFile(@"F:\Projects\Checkers\Checkers\Damka\UI\blackOnDarkTile.png");
                        }
                        else
                        {
                            img = Bitmap.FromFile(@"F:\Projects\Checkers\Checkers\Damka\UI\blackOnLightTile.png");
                        }
                    }
                    else if (currBoard.BoardCells[i, j] == Piece.BlackKing)
                    {
                        if (squareData.IsDark)
                        {
                            img = Bitmap.FromFile(@"F:\Projects\Checkers\Checkers\Damka\UI\blackQueenOnDarkTile.png");
                        }
                        else
                        {
                            img = Bitmap.FromFile(@"F:\Projects\Checkers\Checkers\Damka\UI\blackQueenOnLightTile.png");
                        }
                    }
                    else if (currBoard.BoardCells[i, j] == Piece.WhitePiece)
                    {
                        if (squareData.IsDark)
                        {
                            img = Bitmap.FromFile(@"F:\Projects\Checkers\Checkers\Damka\UI\whiteOnDarkTile.png");
                        }
                        else
                        {
                            img = Bitmap.FromFile(@"F:\Projects\Checkers\Checkers\Damka\UI\whiteOnLightTile.png");
                        }
                    }
                    else if (currBoard.BoardCells[i, j] == Piece.WhiteKing)
                    {
                        if (squareData.IsDark)
                        {
                            img = Bitmap.FromFile(@"F:\Projects\Checkers\Checkers\Damka\UI\whiteQueenOnDarkTile.png");
                        }
                        else
                        {
                            img = Bitmap.FromFile(@"F:\Projects\Checkers\Checkers\Damka\UI\whiteQueenOnLightTile.png");
                        }
                    }

                    squares[j,i].Image = Form1.Resize1(new Bitmap(img), squreSize, squreSize);
                }
            }
        }
        private void PaintSquareNumbers()
        {
            squaresData.Clear();

            Font font = new Font("Arial", 15, FontStyle.Regular, GraphicsUnit.Pixel);
            SolidBrush brush = new SolidBrush(Color.Black);

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            int squreSize = (int)(boardPanel.Width / 8);
            Point atpoint = new Point(squreSize - 10, squreSize - 10);
            int count = 1;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    SquareData squareData = (SquareData)this.squares[i, j].Tag;
                    if (squareData.IsDark)
                    {
                        ((SquareData)squares[j, i].Tag).Number = count;
                        squaresData.Add(count, squareData);
                        Graphics.FromImage(squares[j, i].Image).DrawString(count.ToString(), font, brush, atpoint, sf);
                        ++count;
                    }
                }
            }
        }
        #endregion PaintBoard

        private void PerformMove(int src, int dst)
        {
            if (!(squaresData.ContainsKey(src)) || !(squaresData.ContainsKey(dst)))
            {
                MessageBox.Show("Invalid move");
                return;
            }

            SquareData srcSquare = squaresData[src];
            SquareData dstSquare = squaresData[dst];

            int deltaX = dstSquare.Position.X - srcSquare.Position.X;
            int deltaY = dstSquare.Position.Y - srcSquare.Position.Y;

            if (((Math.Abs(deltaX) != 1) ||
                 (Math.Abs(deltaY) != 1)) &&
                ((Math.Abs(deltaX) != 2) ||
                 (Math.Abs(deltaY) != 2)))
            {
                MessageBox.Show("Invalid move");
                return;
            }

            if ((Math.Abs(deltaX) == 2) &&
                 (Math.Abs(deltaY) == 2))
            {
                deltaX -= Math.Sign(deltaX);
                deltaY -= Math.Sign(deltaY);
            }

            MoveType moveType = this.ConvertToMove(deltaX,deltaY);
            IBoardState newBoard = currBoard.GetBoardState(player, moveType, srcSquare.Position);
            if (newBoard != null)
            {
                this.PlayMove(newBoard);
            }
            else
            {
                MessageBox.Show("Invalid move");
            }
        }
        private void PlayMove(IBoardState newBoard)
        {
            currBoard = newBoard;

            GameState stateAfterMove = currBoard.GetGameState(player);

            if (player == Player.White)
            {
                player = Player.Black;
            }
            else
            {
                player = Player.White;
            }
            this.PaintBoard();
            playersTurn.BackColor = this.ConvertToColor();

            if (stateAfterMove == GameState.Won)
            {
                MessageBox.Show("Won!!!!");
                players = null;
                return;
            }

            if (players[player] is UserPlayer)
            {
                return;
            }
            else
            {
                //MessageBox.Show("", "", MessageBoxButtons.YesNo);
            }

            IMove pcMove = players[player].GetBoardMove(currBoard);
            this.PlayMove(pcMove.Board);
        }
        private Color ConvertToColor()
        {
            return (player == Player.Black) ? Color.Black : Color.White;
        }
        private MoveType ConvertToMove(int deltaX, int deltaY)
        {
            if ((deltaX == 1) && (deltaY == 1))
            {
                return MoveType.BottomRight;
            }
            else if ((deltaX == 1) && (deltaY == -1))
            {
                return MoveType.UpperRight;
            }
            else if ((deltaX == -1) && (deltaY == 1))
            {
                return MoveType.BottomLeft;
            }
            else
            {
                return MoveType.UpperLeft;
            }
        }

        private void performMoveButton_Click(object sender, EventArgs e)
        {
            this.PerformMove(int.Parse(moveSrcTextbox.Text), int.Parse(moveDstTextbox.Text));
        }

        private void remotePCVsPCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ChooseStartPlayer();
            DialogResult res = MessageBox.Show("Local PC player is black?", "Choose local PC color", MessageBoxButtons.YesNo);
            if (res == DialogResult.Yes)
            {
            }
            else
            {
            }
        }

        private void pCVsPCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ChooseStartPlayer();

            Player pcPlayer = this.ChoosePCPlayer();
            Player pc2Player = (pcPlayer == Player.Black) ? Player.White : Player.Black;

            players = new Dictionary<Player, IPlayer>();
            players.Add(pcPlayer, playerCreator.CreatePcPlayer(pcPlayer));
            players.Add(pc2Player, playerCreator.CreatePcPlayer(pc2Player));

            currBoard = initialBoard;
            this.PaintBoard();

            // first player is the PC
            IMove pcMove = players[player].GetBoardMove(currBoard);
            this.PlayMove(pcMove.Board);
        }

        private void playerVsPCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ChooseStartPlayer();

            Player pcPlayer = this.ChoosePCPlayer();
            Player userPlayer = (pcPlayer == Player.Black) ? Player.White : Player.Black;

            players = new Dictionary<Player, IPlayer>();
            players.Add(pcPlayer, playerCreator.CreatePcPlayer(pcPlayer));
            players.Add(userPlayer, new UserPlayer(userPlayer));

            currBoard = initialBoard;
            this.PaintBoard();

            // first player is the PC
            if (pcPlayer == player)
            {
                IMove pcMove = players[player].GetBoardMove(currBoard);
                this.PlayMove(pcMove.Board);
            }
        }

        private void twoPlayersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ChooseStartPlayer();
        }

        private void ChooseStartPlayer()
        {
            DialogResult res = MessageBox.Show("White starts?", "Choose starter", MessageBoxButtons.YesNo);
            player = (res == DialogResult.Yes) ? Player.White : Player.Black;
            playersTurn.BackColor = this.ConvertToColor();
        }

        private Player ChoosePCPlayer()
        {
            DialogResult res = MessageBox.Show("PC player with Black?", "Choose pc player color", MessageBoxButtons.YesNo);
            return (res == DialogResult.Yes) ? Player.Black : Player.White;
        }
    }

    public class UserPlayer : IPlayer
    {
        private Player userPlayerColor;
        public UserPlayer(Player userPlayerColor)
        {
            this.userPlayerColor = userPlayerColor;
        }

        public IMove GetBoardMove(IBoardState boardState)
        {
            return null;
        }
    }

    public class SquareData
    {
        public SquareData(bool isDark,Point position)
        {
            this.IsDark = isDark;
            this.Position = position;
        }

        public bool IsDark
        { get; set; }

        public Point Position
        { get; set; }

        public int Number
        {get;set;}
    }
}
