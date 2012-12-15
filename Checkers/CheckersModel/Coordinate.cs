namespace CheckersModel
{
    public class Coordinate
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="srcCoordinate"></param>
        public Coordinate(Coordinate srcCoordinate)
        {
            this.X = srcCoordinate.X;
            this.Y = srcCoordinate.Y;
            this.Status = srcCoordinate.Status;
        }

        /// <summary>
        /// default constructor
        /// </summary>
        public Coordinate()
        {
        }

        /// <summary>
        /// Check if 2 coordinates are equal
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        protected bool Equals(Coordinate other)
        {
            return Status == other.Status && X == other.X && Y == other.Y;
        }

        /// <summary>
        /// Overide equals function to check equality of ccordinates
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Coordinate) obj);
        }

        /// <summary>
        /// Coordinate hashcode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (int) Status;
                hashCode = (hashCode*397) ^ X;
                hashCode = (hashCode*397) ^ Y;
                return hashCode;
            }
        }

        /// <summary>
        ///     Coordinate status property
        /// </summary>
        public Piece Status { get; set; }

        /// <summary>
        ///     X coordinate property
        /// </summary>
        public int X { get; set; }

        /// <summary>
        ///     Y coordinate property
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// implement operator ==
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Coordinate a, Coordinate b)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object) a == null) || ((object) b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.X == b.X && a.Y == b.Y && a.Status == b.Status;
        }

        /// <summary>
        /// Implement operator !=
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(Coordinate a, Coordinate b)
        {
            return !(a == b);
        }
    }
}