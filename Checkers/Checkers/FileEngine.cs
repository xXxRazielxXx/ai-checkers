using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using CheckersModel;

namespace CheckersEngine
{
    public class FileEngine
    {
        /// <summary>
        /// Safe write to a file, one access (write first line source and dest and player) (write second line which cooordinate were captured)
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="srcCoordinate"></param>
        /// <param name="destCoordinate"></param>
        /// <param name="captureList"></param>
        /// <param name="path"></param>
        /// <param name="player"></param>
        public void WriteToFile(FileStream stream, Coordinate srcCoordinate, Coordinate destCoordinate,IList<Coordinate> captureList, string path,
                                Player player)
        {
            while (true)
            {
                if (File.Exists(path))
                {
                    FileInfo fileInfo = new FileInfo(path);
                    if (fileInfo.IsReadOnly)
                    {
                        Console.WriteLine("Access is denied");
                    }
                }

                //Enter Source ,destanation coordinates and who's the player
                byte[] byteData = null;
                string source = "[" + srcCoordinate.X.ToString(CultureInfo.InvariantCulture) + "," +
                                srcCoordinate.Y.ToString(CultureInfo.InvariantCulture) + "]";
                string destanation = "[" + destCoordinate.X.ToString(CultureInfo.InvariantCulture) + "," +
                                     destCoordinate.Y.ToString(CultureInfo.InvariantCulture) + "]";

                string result = source + " " + destanation;
               
                //Captures list if there are captures
                if (captureList.Count > 0)
                {
                    result += " ";
                    foreach (var capture in captureList)
                    {
                        result += "[" + capture.X.ToString(CultureInfo.InvariantCulture) + "," +
                                          capture.Y.ToString(CultureInfo.InvariantCulture) + "]"+" ";
                    }
                    
                }

                string playerColor;
                //Convert our coordinate to Smhul and Limor moves
               result = CoordinateToShmul(result, out playerColor);
                
                if (player == Player.Black)
                {
                    playerColor = "B";
                }
                else if (player == Player.White)
                {
                    playerColor = "W";
                }
                result += playerColor;
                byteData = Encoding.ASCII.GetBytes(result);
                stream.Write(byteData, 0, byteData.Length);
                
                break;
            }
        }


        /// <summary>
        /// Safely read from a file and convert to our coordinate data and 
        /// First list is source and destanation coordinates and second list (if there are) capture list
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="board"></param>
        /// <param name="path"></param>
        /// <param name="playerColor"></param>
        public IList<Coordinate> ReadFromFile(FileStream stream, Board board, string path, out Player playerColor)
        {
            IList<Coordinate> cor = new List<Coordinate>();
            string temp=string.Empty;
            if (File.Exists(@path))
            {
               byte[] byteData = new byte[stream.Length];
                stream.Read(byteData, 0, (int) stream.Length);
                string content = Encoding.ASCII.GetString(byteData);
                //cor = ShmulToCoordinate(board, content);
                cor = debugging(board, content, out temp);
            }
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
        /// <returns>A list of coordinates</returns>
        private IList<Coordinate> ShmulToCoordinate(Board board, string coordinates, out string playerColor)
        {
            IList<Coordinate> coords = new List<Coordinate>();
            const char delimiterChar = ' ';
            string[] word = coordinates.Split(delimiterChar);

            for (int i = 0; i < word.Length-1; i++)
            {
                int x = Int32.Parse(word[i].Substring(1, 1));
                int y = Int32.Parse(word[i].Substring(3, 1));
                coords.Add(new Coordinate { X = x, Y = y });
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
        /// <param name="playerColor"></param>
        /// <returns></returns>
        private string CoordinateToShmul(string coordinates, out string playerColor)
        {
            IList<Coordinate> coords = new List<Coordinate>();
            const char delimiterChar = ' ';
            string[] word = coordinates.Split(delimiterChar);
            for (int i = 0; i < word.Length-1; i++)
            {
                int x = Int32.Parse(word[i].Substring(1, 1));
                int y = Int32.Parse(word[i].Substring(3, 1));
                coords.Add(new Coordinate { X = x, Y = y });
            }

            //If list is not empty
            if (coords.Count > 0)
            {
                //Convert our coordinates to Shmul and Limor moves
                foreach (var item in coords)
                {
                    item.X=8-item.X;
                    item.Y--;
                }
            }
            coordinates = string.Empty;
            foreach (var coordinate in coords)
            {
                string temp = "[" + coordinate.X.ToString(CultureInfo.InvariantCulture) + "," +
                            coordinate.Y.ToString(CultureInfo.InvariantCulture) + "]"+" ";
                coordinates += temp;
            }

            playerColor = word[word.Length - 1];
            return coordinates;
        }

        /// <summary>
        /// For debug
        /// </summary>
        /// <param name="board"></param>
        /// <param name="coordinates"></param>
        /// <param name="playerColor"></param>
        /// <returns></returns>
        private IList<Coordinate> debugging(Board board, string coordinates, out string playerColor)
        {
            IList<Coordinate> coords = new List<Coordinate>();
            const char delimiterChar = ' ';
            string[] word = coordinates.Split(delimiterChar);
            for (int i = 0; i < word.Length-1; i++)
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