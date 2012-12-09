﻿using System;
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
        /// Safe write to a file, one access
        /// </summary>
        /// <param name="srcCoordinate"></param>
        /// <param name="destCoordinate"></param>
        /// <param name="path"></param>
        public void WriteToFile(FileStream stream, Coordinate srcCoordinate, Coordinate destCoordinate, string path, Player player)
        {
            while (true)
            {
                //string path = @"F:\Projects\Checkers\sync.txt";
                if (File.Exists(path))
                {
                    FileInfo fileInfo = new FileInfo(path);
                    if (fileInfo.IsReadOnly)
                    {
                        Console.WriteLine("Access is denied");
                    }
                }

                byte[] byteData = null;
                string source = "[" + srcCoordinate.X.ToString(CultureInfo.InvariantCulture) + "," +
                                srcCoordinate.Y.ToString(CultureInfo.InvariantCulture) + "]";
                string destanation = "[" + destCoordinate.X.ToString(CultureInfo.InvariantCulture) + "," +
                                     destCoordinate.Y.ToString(CultureInfo.InvariantCulture) + "]";

                string result = source + " " + destanation+" "+player.ToString();
                byteData = Encoding.ASCII.GetBytes(result);
                stream.Write(byteData, 0, byteData.Length);
                break;
            }
        }


        /// <summary>
        /// Safely read from a file and convert to our coordinate data
        /// </summary>
        /// <param name="board"></param>
        /// <param name="path"></param>
        public IList<Coordinate> ReadFromFile(FileStream stream, Board board, string path,out string playerColor)
        {
            IList<Coordinate> cor = new List<Coordinate>();
            if (File.Exists(@path))
            {
                string temp = string.Empty;
                byte[] byteData = new byte[stream.Length];
                stream.Read(byteData, 0, (int) stream.Length);
                string content = Encoding.ASCII.GetString(byteData);
                //cor = ShmulToCoordinate(board, content);
                cor = debugging(board, content,out temp);
                playerColor = temp;
                return cor;
            }
            else
            {
                //Console.WriteLine("File is not located in the specific location");
                playerColor = string.Empty;
                return cor;
            }
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
            int x = Int32.Parse(word[0].Substring(1, 1));
            int y = Int32.Parse(word[0].Substring(3, 1));
            coords.Add(new Coordinate {X = x, Y = y});
            x = Int32.Parse(word[1].Substring(1, 1));
            y = Int32.Parse(word[1].Substring(3, 1));
            coords.Add(new Coordinate {X = x, Y = y});
            if (coords.Count > 0)
            {
                foreach (var item in coords)
                {
                    item.X++;
                    item.Y = 8 - item.Y;
                }
            }
            coords[0].Status = board[board.Search(coords[0])].Status;
            coords[1].Status = board[board.Search(coords[1])].Status;
            playerColor = word[2];
            return coords;
        }

        private IList<Coordinate> debugging(Board board, string coordinates, out string playerColor)
        {
            IList<Coordinate> coords = new List<Coordinate>();
            const char delimiterChar = ' ';
            string[] word = coordinates.Split(delimiterChar);
            int x = Int32.Parse(word[0].Substring(1, 1));
            int y = Int32.Parse(word[0].Substring(3, 1));
            coords.Add(new Coordinate {X = x, Y = y});
            x = Int32.Parse(word[1].Substring(1, 1));
            y = Int32.Parse(word[1].Substring(3, 1));
            coords.Add(new Coordinate {X = x, Y = y});
            coords[0].Status = board[board.Search(coords[0])].Status;
            coords[1].Status = board[board.Search(coords[1])].Status;
            playerColor = word[2];
            return coords;
        }
    }
}