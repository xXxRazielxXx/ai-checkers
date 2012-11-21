namespace Checkers
{
    public class Coordinate
    {
        private Checker checker = new Checker();

        /// <summary>
        ///     Checker property
        /// </summary>
        public Checker Checker
        {
            get { return checker; }
        }

        /// <summary>
        ///     X coordinate property
        /// </summary>
        public int X { get; set; }

        /// <summary>
        ///     Y coordinate property
        /// </summary>
        public int Y { get; set; }

    }
}