using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using CheckersModel;

namespace CheckersEngine
{
    public class FileEngine
    {
        /// <summary>
        /// Safe write to a file, one access (write source and dest in format: [x1,y1] [x2,y2]) (write which cooordinate were captured in format [x3,y3] and then player (B\W))
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="srcCoordinate"></param>
        /// <param name="destCoordinate"></param>
        /// <param name="captureList"></param>
        /// <param name="path"></param>
        /// <param name="player"></param>
        public void WriteToFile(FileStream stream, Coordinate srcCoordinate, Coordinate destCoordinate,
                                IList<Coordinate> captureList, string path,
                                Player player)
        {
            while (true)
            {
                //Checks if file exists and then check if file is read-only- maybe access by rival team
                if (File.Exists(path))
                {
                    FileInfo fileInfo = new FileInfo(path);
                    if (fileInfo.IsReadOnly)
                    {
                        Console.WriteLine("Access is denied");
                    }
                }

                //Enter Source ,destanation coordinates 
                string source = "[" + srcCoordinate.X.ToString(CultureInfo.InvariantCulture) + "," +
                                srcCoordinate.Y.ToString(CultureInfo.InvariantCulture) + "]";
                string destanation = "[" + destCoordinate.X.ToString(CultureInfo.InvariantCulture) + "," +
                                     destCoordinate.Y.ToString(CultureInfo.InvariantCulture) + "]";

                string result = source + " " + destanation;

                //Captures list if there are captures
                if (captureList.Count > 0)
                {
                    foreach (var capture in captureList)
                    {
                        result += " " + "[" + capture.X.ToString(CultureInfo.InvariantCulture) + "," +
                                  capture.Y.ToString(CultureInfo.InvariantCulture) + "]";
                    }
                }

                string playerColor = string.Empty;
                //Convert our coordinate to Smhul and Limor moves (rival team)
                result = CoordinateToShmul(result);

                //Enter who's the player
                if (player == Player.Black)
                {
                    playerColor = "B";
                }
                else if (player == Player.White)
                {
                    playerColor = "W";
                }
                result += playerColor;

                //Convert the stream to ASCIII and write the data to file
                byte[] byteData = Encoding.ASCII.GetBytes(result);
                stream.Write(byteData, 0, byteData.Length);

                break;
            }
        }


        /// <summary>
        /// Safely read from a file and convert to our coordinate data and 
        /// First list is source, destanation coordinates and (if there are) capture list
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="board"></param>
        /// <param name="path"></param>
        /// <param name="playerColor">return the last played player</param>
        /// <returns>returns a coordinate list which repesnt the move and captures</returns>
        public IList<Coordinate> ReadFromFile(FileStream stream, Board board, string path, out Player playerColor)
        {
            IList<Coordinate> cor = new List<Coordinate>();
            string temp = string.Empty;
            if (File.Exists(@path))
            {
                byte[] byteData = new byte[stream.Length];
                stream.Read(byteData, 0, (int) stream.Length);
                string content = Encoding.ASCII.GetString(byteData);
                cor = ShmulToCoordinate(board, content, out temp);
            }

            //Who made the last move
            if (temp == "B")
            {
                playerColor = Player.Black;
            }
            else if (temp == "W")
            {
                playerColor = Player.White;
            }
            else
            {
                playerColor = Player.None;
            }
            return cor;
        }

        /// <summary>
        /// Convert Shmul and Limor moves to our coordinate
        /// </summary>
        /// <param name="board"></param>
        /// <param name="coordinates"></param>
        /// <param name="playerColor"></param>
        /// <returns>A list of coordinates</returns>
        private IList<Coordinate> ShmulToCoordinate(Board board, string coordinates, out string playerColor)
        {
            IList<Coordinate> coords = new List<Coordinate>();
            const char delimiterChar = ' ';
            string[] word = coordinates.Split(delimiterChar);

            for (int i = 0; i < word.Length - 1; i++)
            {
                int x = Int32.Parse(word[i].Substring(1, 1));
                int y = Int32.Parse(word[i].Substring(3, 1));
                coords.Add(new Coordinate {X = x, Y = y});
            }

            //If list is not empty
            if (coords.Count > 0)
            {
                //Convert to our ccordinates and update their status
                foreach (var item in coords)
                {
                    item.X = 8 - item.X;
                    item.Y++;
                    item.Status = board[board.Search(item)].Status;
                }
            }
            playerColor = word[word.Length - 1];
            return coords;
        }

        /// <summary>
        /// Convert our coordinates to Shmul and Limor moves
        /// </summary>
        /// <param name="coordinates"></param>
        /// <returns></returns>
        private string CoordinateToShmul(string coordinates)
        {
            IList<Coordinate> coords = new List<Coordinate>();
            const char delimiterChar = ' ';
            string[] word = coordinates.Split(delimiterChar);
            for (int i = 0; i < word.Length; i++)
            {
                int x = Int32.Parse(word[i].Substring(1, 1));
                int y = Int32.Parse(word[i].Substring(3, 1));
                coords.Add(new Coordinate {X = x, Y = y});
            }

            //If list is not empty
            if (coords.Count > 0)
            {
                //Convert our coordinates to Shmul and Limor moves
                foreach (var item in coords)
                {
                    item.X = 8 - item.X;
                    item.Y--;
                }
            }
            coordinates = string.Empty;
            foreach (var coordinate in coords)
            {
                string temp = "[" + coordinate.X.ToString(CultureInfo.InvariantCulture) + "," +
                              coordinate.Y.ToString(CultureInfo.InvariantCulture) + "]" + " ";
                coordinates += temp;
            }
            return coordinates;
        }

        /// <summary>
        /// For debug
        /// </summary>
        /// <param name="board"></param>
        /// <param name="coordinates"></param>
        /// <param name="playerColor"></param>
        /// <returns></returns>
        private IList<Coordinate> Debugging(Board board, string coordinates, out string playerColor)
        {
            IList<Coordinate> coords = new List<Coordinate>();
            const char delimiterChar = ' ';
            string[] word = coordinates.Split(delimiterChar);
            for (int i = 0; i < word.Length - 1; i++)
            {
                int x = Int32.Parse(word[i].Substring(1, 1));
                int y = Int32.Parse(word[i].Substring(3, 1));
                coords.Add(new Coordinate {X = x, Y = y});
            }
            foreach (var item in coords)
            {
                item.Status = board[board.Search(item)].Status;
            }
            playerColor = word[word.Length - 1];

            return coords;
        }
    }
}