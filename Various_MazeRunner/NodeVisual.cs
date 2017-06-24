using System.Drawing;
using System.Windows.Forms;

namespace Various_MazeRunner
{

    public class NodeVisual
    {
        //Not a very classy class. Could definitely be reworked.
        public NodeVisual(Form1 form1, Coordinates coordinates, int nodeVisualSize, bool closed, Panel parent)
        {
            Form1 = form1;
            Coordinates = coordinates;
            NodeVisualSize = nodeVisualSize;
            Parent = parent;

            if (closed)
                ColorNodeVisual(Color.Black);
            else
                ColorNodeVisual(Color.White);
        }

        private Coordinates Coordinates;
        private Panel Parent;
        private int NodeVisualSize;
        //References the Form1 which created this object.
        private readonly Form1 Form1;

        //Colors the picturebox property of this object to a specified value.
        public void ColorNodeVisual(Color color)
        {
            //this.BackColor = color;
            Brush brush = new SolidBrush(color);
            Graphics graphics = Parent.CreateGraphics();
            graphics.FillRectangle(brush, Coordinates.Column * NodeVisualSize, Coordinates.Row * NodeVisualSize, NodeVisualSize, NodeVisualSize);

        }

    }

}