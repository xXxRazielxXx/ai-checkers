
namespace CheckersModel
{
    public class Coordinate
    {
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
            if (((object)a == null) || ((object)b == null))
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