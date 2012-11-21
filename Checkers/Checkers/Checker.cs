using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Checkers
{
    public class Checker
    {
        public Checker()
        {
            Status = PlayerColor.Empty;
        }

        /// <summary>
        /// Queen property
        /// </summary>
        public bool Queen { get; set; }

        /// <summary>
        /// CoordStatus property 
        /// </summary>
        public PlayerColor Status { get; set; }
    }
}
