using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Drawing;
using System.ComponentModel;

namespace Various_MazeRunner
{
    //Type of search to use.
    public enum SearchAlgorithm
    {
        BFS,
        AStar
    }

    public partial class Form1 : Form
    {
        //Stores values of settings when maze was last run.
        private class LastSettingsValues
        {
            public LastSettingsValues(bool allowDiagonalMovement, bool doNotCrossCorners)
            {
                _allowDiagonalMovement = allowDiagonalMovement;
                _doNotCrossCorners = doNotCrossCorners;
            }

            private readonly bool _allowDiagonalMovement;
            private readonly bool _doNotCrossCorners;

            public bool SettingsChanged(bool allowDiagonalMovement, bool doNotCrossCorners)
            {
                if (allowDiagonalMovement != _allowDiagonalMovement
                    || doNotCrossCorners != _doNotCrossCorners)
                    return true;
                else
                    return false;
            }
        }
        /* StopWatch is used to track precise intervals.
         * It's being used to determine how fast the search
         * is. */
        private Stopwatch _stopWatch = new Stopwatch();
        //Contains our grid of nodes.
        private Node[,] Nodes;
        //Contains visuals for our grid.
        private NodeVisual[,] _nodeVisuals = null;
        //width and height of each NodeVisual.
        public int NodeVisualSize { get; private set; }
        //Columns and rows of grid
        private int _columns { get; set; }
        private int _rows { get; set; }
        //Start and End of the maze.
        private Node _start { get; set; }
        private Node _end { get; set; }
        //Used for displaying information about the maze in results.
        private int _openNodes;
        private int _closedNodes;
        //Instance of search used to run the maze.
        private Search _search = null;
        //Text loaded from maze file.
        private string _mazeText = string.Empty;
        //Last settings when grid was built.
        private LastSettingsValues _lastSettingsValues = null;
        //Last setting on visualization speed when update timer ticked. Lazy method of reading form controls in a background thread.
        private int _visualizationSpeed = 0;
        //Called when form is initialized.
        public Form1()
        {
            InitializeComponent();
        }

        private void BuildMazeGrid(ref string maze)
        {
            //Reset information about maze grid.
            _columns = 0;
            _rows = 0;
            _openNodes = 0;
            _closedNodes = 0;
            Nodes = null;
            _start = null;
            _end = null;

            //Break maze into a string array by splitting carriage returns, while removing empty entries.
            string[] mazeLines = maze.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.None);
            //Calculate largest column size for maze
            for (int i = 0; i < mazeLines.Length; i++)
            {
                _columns = Math.Max(mazeLines[i].Length, _columns);
            }
            //set rows to length of maze
            _rows = mazeLines.Length;

            //Create NodeVisual objects after determining grid size.
            ResetNodeVisuals();

            //Initialize Nodes to size of maze grid
            Nodes = new Node[_columns, _rows];

            //Hide to reduce redraws.
            //NodeVisualsPanel.Visible = false;


            //Will hold grid coordinates for the Start and End node.
            Coordinates startCoordinates = null;
            Coordinates endCoordinates = null;

            //First populate the Nodes grid with each 
            for (int row = 0; row < _rows; row++)
            {
                //Pad the right of our string with char 0 to fill expected column count.
                string line = mazeLines[row].PadRight(_columns, (char)0);

                /* Read each char from 0 to our column count in line.
                 * Since we padded line to match column count, this won't cause errors. */
                for (int column = 0; column < _columns; column++)
                {

                    //Store char data for easy reading
                    char data = line[column];
                    bool closed = false;
                    Coordinates coordinates = new Coordinates(column, row);
                    /* Determine if data indicates a closed node, the
                     * start of the maze, the end of the maze, or
                     * an open node. */
                    switch (data)
                    {
                        case ' ':
                            break;
                        case 'S':
                            startCoordinates = coordinates;
                            break;
                        case 'E':
                            endCoordinates = coordinates;
                            break;
                        default:
                            closed = true;
                            break;
                    }

                    if (closed)
                        _closedNodes++;
                    else
                        _openNodes++;

                    //Create a new node visual and assign it to Nodevisuals column and row.
                    NodeVisual nodeVisual = new NodeVisual(this, coordinates, NodeVisualSize, closed, NodeVisualsPanel);
                    _nodeVisuals[column, row] = nodeVisual;
                    //Create a new node and assign it to our Nodes grid using current column and row.
                    Node node = new Node(coordinates, closed, nodeVisual);
                    Nodes[column, row] = node;
                }
            }

            //Set the Start node using coordinates.
            if (startCoordinates != null)
            { 
                _start = Nodes[startCoordinates.Column, startCoordinates.Row];
                _start.NodeVisual.ColorNodeVisual(Color.Green);
            }
            //Set the End node using coordinates.
            if (endCoordinates != null)
            { 
                _end = Nodes[endCoordinates.Column, endCoordinates.Row];
                _end.NodeVisual.ColorNodeVisual(Color.BlueViolet);
            }

            //Cycle through all nodes in our grid to find their neighboring nodes.
            for (int column = 0; column < _columns; column++)
            {
                for (int row = 0; row < _rows; row++)
                {
                    //Assign to a local variable to make the code easier to read.
                    Node node = Nodes[column, row];
                    //If node isn't closed: calls a method which will set the neighbors list to a specified node.
                    if (!node.Closed)
                        SetNeighbors(node, AllowDiagonalCheckbox.Checked, DontCrossCornersCheckbox.Checked);
                }
            }
        }


        //Finds a nodes neighbors and assigns them.
        private void SetNeighbors(Node node, bool moveDiagonally, bool dontCrossCorners)
        {
            /* Specify normal offsets.
             * N, S, W, E */
            int[] normalOffsetA = new int[] { 0, 0, -1, +1 };
            int[] normalOffsetB = new int[] { -1, +1, 0, 0 };

            Coordinates coordinates = node.Coordinates;

            //Go through each normal offset
            for (int offsetStep = 0; offsetStep < 4; offsetStep++)
            {
                /* Find the new coordinates of a given nodes coordinates + an offset step.
                 * If coordinates of a node is 0, 2 and offset step is 1, -1 the result
                 * coordinates would be 1, 1. */
                int stepA = coordinates.Column + normalOffsetA[offsetStep];
                int stepB = coordinates.Row + normalOffsetB[offsetStep];
                //generate coordinates based of steps
                Coordinates neighborCoordinates = new Coordinates(stepA, stepB);

                //If neighborCoordinates are out of grid bounds skip this neighbor
                if (neighborCoordinates.Column < 0 || neighborCoordinates.Row < 0 || neighborCoordinates.Column >= _columns || neighborCoordinates.Row >= _rows)
                    continue;

                //Assign the neighbor node as the specified neighborCoordinates
                Node neighborNode = Nodes[neighborCoordinates.Column, neighborCoordinates.Row];
                //If the neighbor node isn't closed, add it as a neighbor to node.
                if (!neighborNode.Closed)
                    node.AddNeighbor(new Neighbor(neighborNode, false));
            }

            //If add corners perform a loop through corner neighbors.
            if (moveDiagonally)
            {
                /* Specify corner offsets.
                 * NW, NE, SW, SE. */
                int[] diagonalOffsetA = new int[] { -1, -1, +1, +1 };
                int[] diagonalOffsetB = new int[] { -1, +1, -1, +1 };
                //Go through each diagonal offset
                for (int offsetStep = 0; offsetStep < 4; offsetStep++)
                {
                    /* Find the new coordinates of a given nodes coordinates + an offset step.
                     * If coordinates of a node is 0, 2 and offset step is 1, -1 the result
                     * coordinates would be 1, 1. */
                    int stepA = coordinates.Column + diagonalOffsetA[offsetStep];
                    int stepB = coordinates.Row + diagonalOffsetB[offsetStep];
                    //Generate coordinates based of steps
                    Coordinates neighborCoordinates = new Coordinates(stepA, stepB);

                    //If neighborCoordinates are out of grid bounds skip this neighbor
                    if (neighborCoordinates.Column < 0 || neighborCoordinates.Row < 0 || neighborCoordinates.Column >= _columns || neighborCoordinates.Row >= _rows)
                        continue;

                    if (dontCrossCorners)
                    {
                        Coordinates neighborEdgeA = new Coordinates(node.Coordinates.Column, node.Coordinates.Row + diagonalOffsetB[offsetStep]);
                        Coordinates neighborEdgeB = new Coordinates(node.Coordinates.Column + diagonalOffsetA[offsetStep], node.Coordinates.Row);

                        Node neighborEdgeNodeA = Nodes[neighborEdgeA.Column, neighborEdgeA.Row];
                        Node neighborEdgeNodeB = Nodes[neighborEdgeB.Column, neighborEdgeB.Row];

                        if (neighborEdgeNodeA.Closed || neighborEdgeNodeB.Closed)
                            continue;

                    }
                    //Assign the neighbor node as the specified neighborCoordinates
                    Node neighborNode = Nodes[neighborCoordinates.Column, neighborCoordinates.Row];
                    //If the neighbor node isn't closed, add it as a neighbor to node.
                    if (!neighborNode.Closed)
                        node.AddNeighbor(new Neighbor(neighborNode, true));
                }
            }
        }

        /// <summary>
        /// Begins searching the Nodes grid for a path.
        /// </summary>
        private void TraverseMaze()
        {
            //Reset nodes.
            ResetNodes();

            SearchAlgorithm searchAlgorithm = SearchAlgorithm.AStar;
            switch (SearchAlgorithmCombo.SelectedIndex)
            {
                case 0:
                    searchAlgorithm = SearchAlgorithm.BFS;
                    break;
                case 1:
                    searchAlgorithm = SearchAlgorithm.AStar;
                    break;
            }

            _search = new Search(Nodes, _start, _end, searchAlgorithm);
            /* Loop until StepMaze returns true.
             * Returning true indicates that the maze search is over
             * but not necessarily that a solution was found. */

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            //Create a new thread
            BackgroundWorker backgroundWorker = new BackgroundWorker();

            backgroundWorker.DoWork += (sender, args) =>
            {
                while (!_search.Finished)
                {
                    _search.StepSearch();
                    if (_visualizationSpeed > 0)
                        Thread.Sleep(_visualizationSpeed);
                }
            };
            /* Set time taken to complete search. The value
             * returned is (current time - start time) */
            backgroundWorker.RunWorkerCompleted += (sender, args) =>
            {
                stopwatch.Stop();
                Debug.Print(SearchAlgorithmCombo.Text + " completed in " + stopwatch.ElapsedMilliseconds.ToString() + "ms.");
                //An exception occurred.
                if (args.Error != null) 
                    Debug.Print(args.Error.ToString());

                VisualizationSpeedUpdate.Enabled = false;

                //If the FoundPath count is 0, a path could not be found.
                if (_search.FoundPath.Count == 0)
                {
                    Debug.Print("A path could not be found. Time " + stopwatch.ElapsedMilliseconds.ToString() + "ms.");
                }
                //If the FoundPath is greater than 0, a path has been found.
                else
                {
                    ColorFoundPath(_search.FoundPath);
                }

                //Search has ended, allow another search.
                ResetSearchButton_Click(null, null);

            };

            backgroundWorker.RunWorkerAsync(); // starts the background worker

        }

        //Colors found path nodes.
        private void ColorFoundPath(List<Node> foundPath)
        {
            /* Go through each node in found path and color it
             * as found. It's usually much quicker to go through found path
             * separately than to check if every node in grid
             * is within found path. */
            for (int i = 0; i < foundPath.Count; i++)
            {
                Node node = foundPath[i];
                //Color node in found path based on node type.
                if (node == _start)
                    node.NodeVisual.ColorNodeVisual(Color.Green);
                else if (node == _end)
                    node.NodeVisual.ColorNodeVisual(Color.BlueViolet);
                else
                    node.NodeVisual.ColorNodeVisual(Color.Aqua);
            }
        }

        /* Resets all nodes. See void Reset() for more information.
         * This would be called if the maze stayed the same, and only the start or
         * end location has changed. */
        private void ResetNodes()
        {
            for (int column = 0; column < _columns; column++)
            {
                for (int row = 0; row < _rows; row++)
                {
                    Node node = Nodes[column, row];
                    //Only need to reset if not a closed node.
                    if (!node.Closed)
                    {
                        //Call reset on node.
                        node.Reset();
                        //Color back to original color.
                        if (node == _start)
                            node.NodeVisual.ColorNodeVisual(Color.Green);
                        else if (node == _end)
                            node.NodeVisual.ColorNodeVisual(Color.BlueViolet);
                        else
                            node.NodeVisual.ColorNodeVisual(Color.White);
                    }
                    
                }
            }
        }

        //Shows an open file dialog to select a maze, then runs requested maze.
        private void but_LoadMazeText_Click(object sender, EventArgs e)
        {
            //Erase current maze.
            NodeVisualsPanel.Invalidate();
            //Create a new OpenFileDialog
            OpenFileDialog fileDialog = new OpenFileDialog();
            //fileDialog will only show text files and directories.
            fileDialog.Filter = "txt files (*.txt)|*.txt";
            //If a file was successfully selected.
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                //Load the selected file into a stream reader, reading to the end of the file.
                StreamReader streamReader = new StreamReader(fileDialog.FileName);
                //Append the entire text into a string builder
                //MazeText.Append(streamReader.ReadToEnd());
                _mazeText = streamReader.ReadToEnd();
                //Close the stream reader, then begin running the maze.
                streamReader.Close();
                //Builds maze grid using text from file.
                BuildMazeGrid(ref _mazeText);
            }
            //Enable button to load another maze
            LoadMazeButton.Enabled = true;
        }

        /* Creates node visuals to the current Columns and Rows
         * setting for the grid. Also deregisters node visuals to previous
         * grid if aren't null. */
        private void ResetNodeVisuals()
        {
            //Exit method if rows or columns are zero. Cannot make a grid without each being at least 1.
            if (_columns == 0 || _rows == 0)
                return;

            //Determine size of each nodevisual.
            double itemWidth = (NodeVisualsPanel.Width) / _columns;
            double itemHeight = (NodeVisualsPanel.Height) / _rows;

            //Set NodeVisualSize to the smallest axis so it doesn't overflow.
            if (itemHeight < itemWidth)
                NodeVisualSize = Convert.ToInt32(Math.Floor(itemHeight));
            else
                NodeVisualSize = Convert.ToInt32(Math.Floor(itemHeight));

            //Reset Node visuals to new grid size.
            _nodeVisuals = new NodeVisual[_columns, _rows];
        }

        //Handle clicks on the node visual panel to set start and end location.
        private void NodeVisualsPanel_MouseUp(object sender, MouseEventArgs e)
        {
            /* Don't allow clicks if load or start button is disabled.
             * This would occur if a maze is being loaded in or if 
             * a search is in progress. */
            if (!LoadMazeButton.Enabled || !StartSearchButton.Enabled)
                return;

            //Don't try to calculate position when a grid hasnt been generated.
            if (_columns == 0 || _rows == 0)
                return;

            //Determine column and row at click location.
            int column = (e.X / NodeVisualSize);
            int row = (e.Y / NodeVisualSize);

            //If invalid click location or grid isnt builr.
            if (_columns == 0 || _rows == 0 || column > _columns || row > _rows || column < 0 || row < 0)
            {
                Debug.Print("Grid isn't built or selected point is out of bounds. Cannot continue.");
                return;
            }

            //Assign to node at clicked location
            Node node = Nodes[column, row];

            //If node is closed we cant set as start or end.
            if (node.Closed)
            {
                Debug.Print("Selected node is closed, cannot use.");
                return;
            }

            //If left button set node as start.
            if (e.Button == MouseButtons.Left)
            {
                //If start is already specified reset its color to white
                if (_start != null)
                    _start.NodeVisual.ColorNodeVisual(Color.White);
                //Assign new start and color it.
                _start = node;
                node.NodeVisual.ColorNodeVisual(Color.Green);
            }
            //If right button set as end.
            else if (e.Button == MouseButtons.Right)
            {
                //If end is already specified reset its color to white
                if (_end != null)
                    _end.NodeVisual.ColorNodeVisual(Color.White);
                //Assign new end and color it.
                _end = node;
                node.NodeVisual.ColorNodeVisual(Color.BlueViolet);
            }


        }

        //Called when Start Search is clicked
        private void StartSearchButton_Click(object sender, EventArgs e)
        {
            if (_start == null || _end == null)
            {
                Debug.Print("Start or End is null. Left click maze to set Start, right click to set End.");
                return;
            }

            SetVisualizationSpeed();
            //Disable start and load maze button and enable cancel.
            StartSearchButton.Enabled = false;
            LoadMazeButton.Enabled = false;
            CancelSearchButton.Enabled = true;

            bool allowDiag = AllowDiagonalCheckbox.Checked;
            bool dontCrossCorners = DontCrossCornersCheckbox.Checked;
            //If grid needs to be rebuilt due to settings change.
            if (_lastSettingsValues == null
                || _lastSettingsValues.SettingsChanged(allowDiag, dontCrossCorners))
                BuildMazeGrid(ref _mazeText);
            //Make a new last settings value.
            _lastSettingsValues = new LastSettingsValues(allowDiag, dontCrossCorners);
            
            VisualizationSpeedUpdate.Enabled = true;
            //Travel maze
            TraverseMaze();
        }

        //Cancels searching and reenables controls.
        private void ResetSearchButton_Click(object sender, EventArgs e)
        {
            if (_search != null)
                _search.Finished = true;

            //Disable cancel button and enable start and load maze
            StartSearchButton.Enabled = true;
            LoadMazeButton.Enabled = true;
            CancelSearchButton.Enabled = false;
        }

        //Form loaded
        private void Form1_Load(object sender, EventArgs e)
        {
            //Load search types and seelct A* as default.
            SearchAlgorithmCombo.Items.Add("Breadth First Search");
            SearchAlgorithmCombo.Items.Add("A* Search");
            SearchAlgorithmCombo.SelectedIndex = 1;
        }


        private void VisualizationSpeedUpdate_Tick(object sender, EventArgs e)
        {
            SetVisualizationSpeed();
        }

        //Sets visualization speed to the slider value.
        private void SetVisualizationSpeed()
        {
            _visualizationSpeed = VisualizationSpeedTrackbar.Value;
        }
    }


}
