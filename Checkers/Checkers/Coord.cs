using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Checkers
{
    public class Coord
    {
        private int x;
        private int y;
        private bool king = false;
        private CoordStatus coordstatus=CoordStatus.Empty;

        public CoordStatus status
        {
            get { return coordstatus; }
            set { coordstatus = value; }
        }

        public bool King
        {
            get { return king; }
            set { king = value; }
        }

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

    }

}
