namespace Various_MazeRunner
{
    //Coordinates holds a column and row position within a 2D array.
    public class Coordinates
    {
        //Initializes Coordinates
        public Coordinates(int column, int row)
        {
            //Set Column and Row
            Column = column;
            Row = row;
        }

        //Column and Row of this coordinate.
        public int Column { get; }
        public int Row { get; }
    }
}
