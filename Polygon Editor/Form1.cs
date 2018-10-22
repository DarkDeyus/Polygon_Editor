using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Polygon_Editor
{
    //TODO : PACK IT IN A POLYGON CLASS
    //TODO : PACK IT IN A POLYGON CLASS
    //TODO : PACK IT IN A POLYGON CLASS
    //TODO : PACK IT IN A POLYGON CLASS
    //TODO : PACK IT IN A POLYGON CLASS
    //TODO : PACK IT IN A POLYGON CLASS
    //TODO : PACK IT IN A POLYGON CLASS
    public partial class Form1 : Form
    {
        #region Variables

        // Is the editor in create mode, if not, it is in edit mode
        bool create = true;
        Vertex selectedVertex;
        Edge selectedEdge;
        
        bool isMoving;
        ContextMenu edgeContextMenu;
        ContextMenu vertexContextMenu;
        List<Vertex> vertices = new List<Vertex>();
        List<Edge> edges = new List<Edge>();

        #endregion

        public Form1()
        {
            InitializeComponent();
            edgeContextMenu = new ContextMenu();
            edgeContextMenu.MenuItems.Add("Add/remove a vertical restriction", AddRemoveVerticalRestriction_Click);
            edgeContextMenu.MenuItems.Add("Add/remove a horizontal restriction", AddRemoveHorizontalRestriction_Click);
            edgeContextMenu.MenuItems.Add("Add a vertex in the middle of this edge", AddVertexInTheMiddleOfEdge_Click);

            vertexContextMenu = new ContextMenu();
            vertexContextMenu.MenuItems.Add("Add/remove a degree restriction", AddRemoveAngleRestriction_Click);
            vertexContextMenu.MenuItems.Add("Delete this vertex", RemoveVertex_Click);          

        }

        List<Vertex> DeepClone(List<Vertex> list)
        {
            List<Vertex> copy = new List<Vertex>();
            foreach (Vertex vertex in list)
                copy.Add(vertex.Clone());
            return copy;
        }
        void RemoveVertex_Click(object sender, EventArgs e)
        {
            RemoveVertex(selectedVertex);         
        }
        void AddVertexInTheMiddleOfEdge_Click(object sender, EventArgs e)
        {
            AddVertexInTheMiddleOfEdge(selectedEdge);
        }
        void AddRemoveHorizontalRestriction_Click(object sender, EventArgs e)
        {
            AddRemoveHorizontalRestriction(selectedEdge, edges);
        }
        void AddRemoveVerticalRestriction_Click(object sender, EventArgs e)
        {
            AddRemoveVerticalRestriction(selectedEdge, edges);
        }
        void AddRemoveAngleRestriction_Click(object sender, EventArgs e)  
        {
            AddRemoveAngleRestriction(selectedVertex, edges, vertices);
        }
        void AddRemoveHorizontalRestriction(Edge e, List<Edge> edges)
        {
            //remove restriction
            if (e.restriction == Enums.EdgeRestriction.horizontal)
            {
                e.restriction = Enums.EdgeRestriction.none;
                pictureBox1.Refresh();
            }

            else if (e.restriction == Enums.EdgeRestriction.none)
            {
                int index = edges.FindIndex(v => v.From == e.From && v.To == e.To);

                if (edges[(index + 1) % edges.Count].restriction == Enums.EdgeRestriction.horizontal || edges[(edges.Count + index - 1) % edges.Count].restriction == Enums.EdgeRestriction.horizontal)
                    MessageBox.Show("Two adjacent edges can't have the same type of restriction");
                else
                {
                    e.restriction = Enums.EdgeRestriction.horizontal;
                    fixConstraints(vertices, edges, e, null, 0, 0);
                    pictureBox1.Refresh();
                }
                    
            }
            //Silently fail if wrong restriction
            return;
        }
        void AddRemoveVerticalRestriction(Edge e, List<Edge> edges)
        {
            //remove restriction
            if (e.restriction == Enums.EdgeRestriction.vertical)
            {
                e.restriction = Enums.EdgeRestriction.none;
                pictureBox1.Refresh();
            }

            else if (e.restriction == Enums.EdgeRestriction.none)
            {
                int index = edges.FindIndex(v => v.From == e.From && v.To == e.To);

                if (edges[(index + 1) % edges.Count].restriction == Enums.EdgeRestriction.vertical || edges[(edges.Count + index - 1) % edges.Count].restriction == Enums.EdgeRestriction.vertical)
                    MessageBox.Show("Two adjacent edges can't have the same type of restriction");
                else
                {
                    e.restriction = Enums.EdgeRestriction.vertical;
                    fixConstraints(vertices, edges, e, null, 0, 0);
                    pictureBox1.Refresh();
                }
                    
            }
            //Silently fail if wrong restriction
            return;
        }

        //TODO : FIX
        void AddRemoveAngleRestriction(Vertex v, List<Edge> edgesList, List<Vertex> verticesList)
        {
            //remove restriction
            if (v.restriction == Enums.VertexRestriction.angle)
            {
                v.restriction = Enums.VertexRestriction.none;
                pictureBox1.Refresh();
            }
            else if (v.restriction == Enums.VertexRestriction.none)
            {
                //int dot(int vx,int vy,int ux,int uy)
                //{
                //    return vx * ux + vy * uy;
                //}

                //double Angle(Edge first, Edge second)
                //{
                //    int vx = first.From.X - first.To.X;
                //    int vy = first.From.Y - first.To.Y;
                //    int ux = second.From.X - second.To.X;
                //    int uy = second.From.Y - second.To.Y;

                //    int dotProduct = dot(vx, vy, ux, uy);

                //    double magv = Math.Sqrt(dot(vx, vy, vx, vy));
                //    double magu = Math.Sqrt(dot(ux, uy, ux, uy));

                //    double cos = dotProduct / magv / magu;

                //    double angle = Math.Acos(cos);
                //    //to get the result in degrees
                //    angle = angle * (180 / Math.PI);
                //    return angle;
                //}

                double Angle(Vertex vertex, List<Vertex> list)
                {
                    int index = verticesList.FindIndex(u => u.X == v.X && u.Y == v.Y);
                    return 0;
                }


                //int index = verticesList.FindIndex(u => u.X == v.X && u.Y == v.Y);
                //double a = Angle(edgesList[(edgesList.Count + index - 1) % edgesList.Count], edgesList[index]);
                double a = 0;
                string value = a.ToString();
                if (InputBox.Show("Angle restriction", "Please input chosen angle for restriction ( 0° < α < 360° )", ref value) == DialogResult.OK)
                {
                    if (double.TryParse(value, out double result) && result > 0 && result < 360)
                    {
                        v.angle = result;
                        v.restriction = Enums.VertexRestriction.angle;
                        pictureBox1.Refresh();
                    }
                    else
                        MessageBox.Show("Please input a correct value of the angle");
                }


            }
            //Silently fail if wrong restriction
            return;
        }
        void RemoveVertex(Vertex vertex)
        {
            if (vertices.Count <= 3)
            {
                MessageBox.Show("Minimal number of vertices in a polygon can't be lower than 3");
            }
            else
            {
                vertex.restriction = Enums.VertexRestriction.none;
                int index = vertices.FindIndex(v => v.X == vertex.X && v.Y == vertex.Y);
                Vertex nextVertex = vertices[(index + 1) % vertices.Count];
                Vertex previousVertex = vertices[(vertices.Count + index - 1) % vertices.Count];
                vertices.RemoveAt(index);
                //Edge to the right to the vertex
                edges.RemoveAt(index);
                //edge to the left of the vertex, need to add edges.Count, as negative numbers and modulo aren't that good together
                int count = (edges.Count + index - 1) % edges.Count;
                edges[count] = new Edge(previousVertex, nextVertex);
                pictureBox1.Refresh();
            }
        }
        void AddVertexInTheMiddleOfEdge(Edge e)
        {
            int index = edges.FindIndex(v => v.From == e.From && v.To == e.To);
            Vertex vertex = new Vertex((e.From.X + e.To.X) / 2, (e.From.Y + e.To.Y) / 2);
            vertices.Insert(index + 1, vertex);
            edges.Insert(index, new Edge(vertex, e.To));
            edges.Insert(index, new Edge(e.From, vertex));
            edges.Remove(e);
            pictureBox1.Refresh();

        }
        void drawLine(Pen pen, Edge edge, PaintEventArgs e)
        {
            
            void plotLineLow(int x0, int y0, int x1, int y1, PaintEventArgs paintEvent)
            {
                int dx = x1 - x0;
                int dy = y1 - y0;
                int yi = 1;
                if (dy < 0)
                {
                    yi = -1;
                    dy = -dy;
                }

                int D = 2 * dy - dx;
                int y = y0;
                for (int x = x0; x < x1; x++)
                {
                    e.Graphics.FillRectangle(Constants.brush, x, y, 1, 1);
                    if (D > 0)
                    {
                        y = y + yi;
                        D = D - 2 * dx;
                    }
                    D = D + 2 * dy;
                }
            }

            void plotLineHigh(int x0, int y0, int x1, int y1, PaintEventArgs paintEvent)
            {
                int dx = x1 - x0;
                int dy = y1 - y0;
                int xi = 1;
                if (dx < 0)
                {
                    xi = -1;
                    dx = -dx;
                }

                int D = 2 * dx - dy;
                int x = x0;
                for (int y = y0; y < y1; y++)
                {
                    e.Graphics.FillRectangle(Constants.brush, x, y, 1, 1);
                    if (D > 0)
                    {
                        x = x + xi;
                        D = D - 2 * dy;
                    }
                    D = D + 2 * dx;
                }
            }
          
            void plotLine(int x0, int y0, int x1, int y1, PaintEventArgs paintEvent)
            {
                if (Math.Abs(y1 - y0) < Math.Abs(x1 - x0))
                {
                    if (x0 > x1)
                        plotLineLow(x1, y1, x0, y0, paintEvent);
                    else
                        plotLineLow(x0, y0, x1, y1, paintEvent);

                }
                else
                {
                    if (y0 > y1)
                        plotLineHigh(x1, y1, x0, y0, paintEvent);
                    else
                        plotLineHigh(x0, y0, x1, y1, paintEvent);
                }
            }

            //e.Graphics.DrawLine(pen, edge.From.X, edge.From.Y, edge.To.X, edge.To.Y);

            plotLine(edge.From.X, edge.From.Y, edge.To.X, edge.To.Y, e);
        }
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (create)
            {
                switch (e.Button)
                {
                    case (MouseButtons.Left):
                        Vertex v = new Vertex(e.X, e.Y);
                        int index = Vertex.containsVertex(vertices, Constants.leeway, v);
                        //no overlapping vertices
                        if (index == -1)
                        {
                            int count = vertices.Count;
                            vertices.Add(v);
                            if (count > 0)
                                edges.Add(new Edge(vertices[count - 1], vertices[count]));
                            pictureBox1.Refresh();
                        }
                        //clicked the first vertex, we need to close the polygon 
                        else if (index == 0)
                        {
                            int count = vertices.Count;
                            if (count > 2)
                            {
                                edges.Add(new Edge(vertices[count - 1], vertices[0]));
                                pictureBox1.Refresh();
                                create = false;
                                createNewPolygonToolStripMenuItem1.Enabled = false;
                            }
                        }
                        break;

                }
            }
            else
            {
                switch (e.Button)
                {
                    //showing context menu
                    case (MouseButtons.Right):
                        Vertex u = new Vertex(e.X, e.Y);
                        int index1 = Vertex.containsVertex(vertices, Constants.leeway, u);

                        if (index1 != -1)
                        {
                            selectedVertex = vertices[index1];
                            vertexContextMenu.Show(this, new Point(e.X, e.Y));
                        }

                        else
                        {
                            index1 = Edge.containsEdge(edges, Constants.leeway, e.X, e.Y);
                            if (index1 != -1)
                            {
                                selectedEdge = edges[index1];
                                //places the menu at the pointer position
                                edgeContextMenu.Show(this, new Point(e.X, e.Y));
                            }

                        }

                        break;

                }
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            SolidBrush mySolidBrush = new SolidBrush(Color.Black);
            foreach (Edge edge in edges)
            {
                drawLine(Constants.pen, edge, e);
                switch(edge.restriction)
                {
                    case Enums.EdgeRestriction.horizontal:
                        e.Graphics.DrawIcon(Properties.Resources.horizontal, (edge.From.X + edge.To.X - Constants.iconSize) /2 , (edge.From.Y + edge.To.Y - Constants.iconSize) /2 + Constants.distance);
                        break;
                    case Enums.EdgeRestriction.vertical:
                        e.Graphics.DrawIcon(Properties.Resources.vertical, (edge.From.X + edge.To.X - Constants.iconSize) / 2 + Constants.distance, (edge.From.Y + edge.To.Y - Constants.iconSize) / 2 );
                        break;
                }
            }

            foreach (Vertex v in vertices)
            {
                Rectangle rect = new Rectangle(v.X - Constants.vertexSize / 2, v.Y - Constants.vertexSize / 2, Constants.vertexSize, Constants.vertexSize);
                e.Graphics.FillEllipse(mySolidBrush, rect);
                e.Graphics.DrawEllipse(Constants.pen, rect);

                if(v.restriction == Enums.VertexRestriction.angle)
                    e.Graphics.DrawIcon(Properties.Resources.angle, v.X - Constants.iconSize / 2 , v.Y + Constants.distance - Constants.iconSize / 2 );
                
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            edges.Clear();
            vertices.Clear();
            pictureBox1.Refresh();
            createNewPolygonToolStripMenuItem1.Enabled = true;
            create = true;
        }

        private void createNewPolygonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            create = true;
        }

        private void setADefaultPolygonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clearToolStripMenuItem.PerformClick();

            // prepare the preset polygon
            List<Vertex> presetVertices = new List<Vertex>();
            List<Edge> presetEdges = new List<Edge>();
            Vertex a = new Vertex(200, 175, Enums.VertexRestriction.angle, 53);
            Vertex b = new Vertex(300, 100);
            Vertex c = new Vertex(500, 100);
            Vertex d = new Vertex(500, 250);
            Vertex f = new Vertex(300, 250);
            presetVertices.Add(a);
            presetVertices.Add(b);
            presetVertices.Add(c);
            presetVertices.Add(d);
            presetVertices.Add(f);
            presetEdges.Add(new Edge(a, b));
            presetEdges.Add(new Edge(b, c, Enums.EdgeRestriction.horizontal));
            presetEdges.Add(new Edge(c, d, Enums.EdgeRestriction.vertical));
            presetEdges.Add(new Edge(d, f, Enums.EdgeRestriction.horizontal));
            presetEdges.Add(new Edge(f, a));


            vertices = presetVertices;
            edges = presetEdges;
            create = false;
            createNewPolygonToolStripMenuItem1.Enabled = false;
            pictureBox1.Refresh();

        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            int index = Vertex.containsVertex(vertices, Constants.leeway, new Vertex(e.X, e.Y));
            if (index != -1)
                selectedVertex = vertices[index];

            if (!create && selectedVertex != null && e.Button == MouseButtons.Left)
            {
                isMoving = true;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                isMoving = false;

            if (create || selectedVertex == null)
                return;
            
            if (e.Button != MouseButtons.Right)
                selectedVertex = null;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (create || selectedVertex == null || !isMoving)
                return;

            int x = pictureBox1.PointToClient(Cursor.Position).X;
            int y = pictureBox1.PointToClient(Cursor.Position).Y;

            //Keep it from being outside of window

            if (x < 0)
                x = 0;
            if (x > (pictureBox1.Right - pictureBox1.Left))
                x = pictureBox1.Right - pictureBox1.Left;

            if (y < 0)
                y = 0;
            if (y > (pictureBox1.Bottom - pictureBox1.Top))
                y = pictureBox1.Bottom - pictureBox1.Top;

            
            if(fixConstraints(vertices, edges, null, selectedVertex, x, y))
            {
                selectedVertex.X = x;
                selectedVertex.Y = y;
            }
            pictureBox1.Refresh();
        }
        /*
         *                     v1
         *                       \
         *                        \
         *                       / \
         *             degree-> |  v2 <- we're here
         *                       \ /
         *                        / <- restriction 
         *                       /      
         *                      v3
         * 
         */ 
        (int x, int y) GetNewPoint(Vertex v1, Vertex v2, int degree , Enums.EdgeRestriction restriction, Vertex v3)
        {
            double DegreeToRadian(int deg)
            {
               return deg * Math.PI / 180; 
            }
            int newx = int.MaxValue;
            int newy = int.MaxValue;
            // new line with correct degree between the lines
            double a = Math.Tan(DegreeToRadian(degree));
            double b = v1.Y - a * v1.X;
            
            switch(restriction)
            {
                case Enums.EdgeRestriction.horizontal:
                    newy = v3.Y;

                    newx =  a == 0 ? (v3.X == b ? v3.X : int.MaxValue) : (int)((v3.Y - b) / a);
                    /*
                    if (a == 0)
                    {
                        // y = ax + b --a=0--> y = b
                        if (v3.X == b)
                            newx = b;
                        else
                            //will never be correct
                            newx = int.MaxValue;
                    }
                    else
                    newx = (int)((v3.Y - b) / a); 
                    */
                    break;
                case Enums.EdgeRestriction.vertical:
                    newx = v3.X;
                    newy = (int)(a * newx + b);
                    break;
                case Enums.EdgeRestriction.none:
                    // Find closest point on a line from given point
                    // ??????????????????????????????????????????????
                    break;
                    
            }
            return (newx, newy);
        }
        //TODO: FIX IT, I HATE YOU ANGLES >:C
        private bool fixConstraints(List<Vertex> vertices, List<Edge> edges, Edge e, Vertex oldVertex, int newX, int newY )
        {
            // Keeping a copy, as there is a possiblity of not being able to fullfill the constraints (for example angle - user inputting angle 180 when the angle is created by perpendicular line segments -> always 90*
            List<Vertex> verticesCopy = DeepClone(vertices);

            int startIndex;
            int currentVertex;
            int nextVertex;
            int count = 0;
            //Passed edge, so added a constraint on edge
            if (oldVertex == null)
            {
                //One point is correct, so i am taking the one with lower index in the list
                currentVertex = startIndex = vertices.FindIndex(u => u.X == e.From.X && u.Y == e.From.Y);
            }
            //Passed a vertex, so either moved it or added an angle constraint 
            else
            {
                currentVertex = startIndex = vertices.FindIndex(u => u.X == oldVertex.X && u.Y == oldVertex.Y);
                vertices[currentVertex].X = newX;
                vertices[currentVertex].Y = newY;
            }

            nextVertex = (startIndex + 1) % vertices.Count;           
            bool correct = false;
            //go right and fix
            do
            {//add vertex restriction plz
                Edge connecting = edges[currentVertex];
                if (connecting.restriction == Enums.EdgeRestriction.none)
                {
                    correct = true;
                }
                else if (connecting.restriction == Enums.EdgeRestriction.horizontal)
                {
                    if (vertices[nextVertex].Y == vertices[currentVertex].Y)
                        correct = true;
                    else
                        vertices[nextVertex].Y = vertices[currentVertex].Y;

                }
                else if (connecting.restriction == Enums.EdgeRestriction.vertical)
                {
                    if (vertices[nextVertex].X == vertices[currentVertex].X)
                        correct = true;
                    else
                        vertices[nextVertex].X = vertices[currentVertex].X;
                }
                count++;
                currentVertex = nextVertex;
                nextVertex = (nextVertex + 1) % vertices.Count;
            }
            while (!correct && currentVertex != startIndex);
            // Returned to the starting vertex, can't fix it
            if(currentVertex == startIndex)
            {
                vertices = verticesCopy;
                return false;
            }
            currentVertex = startIndex;
            nextVertex = (startIndex - 1 + vertices.Count) % vertices.Count;
            correct = false;
            //go left and fix
           while ( !correct && currentVertex != (startIndex + count) % vertices.Count )
            {
                Edge connecting = edges[nextVertex];
                if (connecting.restriction == Enums.EdgeRestriction.none)
                {
                    correct = true;
                }
                else if (connecting.restriction == Enums.EdgeRestriction.horizontal)
                {
                    if (vertices[nextVertex].Y == vertices[currentVertex].Y)
                        correct = true;
                    else
                        vertices[nextVertex].Y = vertices[currentVertex].Y;

                }
                else if (connecting.restriction == Enums.EdgeRestriction.vertical)
                {
                    if (vertices[nextVertex].X == vertices[currentVertex].X)
                        correct = true;
                    else
                        vertices[nextVertex].X = vertices[currentVertex].X;
                }

                currentVertex = nextVertex;
                nextVertex = (nextVertex - 1 + vertices.Count) % vertices.Count;
            }

           if(currentVertex == (startIndex + count) % vertices.Count)
            {
                vertices = verticesCopy;
                return false;
            }

            return true;
        }
    }
}
