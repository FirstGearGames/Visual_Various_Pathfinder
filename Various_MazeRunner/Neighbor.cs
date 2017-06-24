namespace Various_MazeRunner
{
    public class Neighbor
    {
        public Neighbor(Node node, bool diagonal)
        {
            Node = node;
            Diagonal = diagonal;
        }

        //Node which is the neighbor
        public Node Node { get; }
        //True if this neighbor is diagonal to parent
        public bool Diagonal { get; }
    }
}
