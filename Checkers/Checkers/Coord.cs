namespace Checkers
{
    public class Coord
    {
        private CoordStatus coordStatus = CoordStatus.Empty;

        /// <summary>
        /// CoordStatus property 
        /// </summary>
        public CoordStatus Status
        {
            get { return coordStatus; }
            set { coordStatus = value; }
        }

        /// <summary>
        /// King property
        /// </summary>
        public bool King { get; set; }

        /// <summary>
        /// X coordinate property
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Y coordinate property
        /// </summary>
        public int Y { get; set; }

        //public int Value { get; set; } - heuristic value
        //public bool KingRow(){} - did i reach to a row
        //public bool findValidMoves(){} 
        //public bool findValidJumps(){}


    }
}