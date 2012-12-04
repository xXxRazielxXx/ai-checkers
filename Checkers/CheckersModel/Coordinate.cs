using System.Collections;

namespace CheckersModel
{
    public class Coordinate
    {

        public Coordinate(Coordinate srcCoordinate)
        {
            this.X = srcCoordinate.X;
            this.Y = srcCoordinate.Y;
            this.Status = srcCoordinate.Status;
        }

        public Coordinate()
        {
        }

        protected bool Equals(Coordinate other)
        {
            return Status == other.Status && X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Coordinate) obj);
        }

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


        public static bool operator ==(Coordinate a, Coordinate b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
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

        public static bool operator !=(Coordinate a, Coordinate b)
        {
            return !(a == b);
        }

    }
}