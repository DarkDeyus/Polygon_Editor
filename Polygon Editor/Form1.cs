using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Polygon_Editor.Enums;

namespace Polygon_Editor
{
    
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
        static public (double, double) PolarToCartesian(double radius, double angle)
        {
            double angleRadian = angle * Math.PI / 180;
            double x = radius * Math.Cos(angleRadian);
            double y = radius * Math.Sin(angleRadian);
            return (x, y);
        }
        static public bool isInsidePolygon(List<Edge> edges, double x, double y)
        {
            int count = 0;
            Edge cross = new Edge(new Vertex(x, y), new Vertex(10000, 10000));
            foreach (Edge e in edges)
            {
                if (Edge.CrosingCheck(e,cross))
                    count++;
            }
            return count % 2 == 1;
        }
        //List<Vertex> DeepClone(List<Vertex> list)
        //{
        //    List<Vertex> copy = new List<Vertex>();
        //    foreach (Vertex vertex in list)
        //        copy.Add(vertex.Clone());
        //    return copy;
        //}
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
            AddRemoveHorizontalRestriction(selectedEdge);
        }
        void AddRemoveVerticalRestriction_Click(object sender, EventArgs e)
        {
            AddRemoveVerticalRestriction(selectedEdge);
        }
        void AddRemoveAngleRestriction_Click(object sender, EventArgs e)
        {
            AddRemoveAngleRestriction(selectedVertex);
        }
        void AddRemoveHorizontalRestriction(Edge e)
        {
            //remove restriction
            if (e.restriction == Enums.EdgeRestriction.horizontal)
            {
                e.restriction = Enums.EdgeRestriction.none;
                pictureBox1.Refresh();
            }

            else if (e.restriction == Enums.EdgeRestriction.none)
            {
                int index = edges.FindIndex(v => v == e);

                if (edges[(index + 1) % edges.Count].restriction == Enums.EdgeRestriction.horizontal || edges[(edges.Count + index - 1) % edges.Count].restriction == Enums.EdgeRestriction.horizontal)
                    MessageBox.Show("Two adjacent edges can't have the same type of restriction");
                else
                {
                    e.restriction = Enums.EdgeRestriction.horizontal;
                    e.To.Y = e.From.Y;
                    pictureBox1.Refresh();
                }

            }
            //Silently fail if wrong restriction
            return;
        }
        void AddRemoveVerticalRestriction(Edge e)
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
                    e.To.X = e.From.X;
                    pictureBox1.Refresh();
                }

            }
            //Silently fail if wrong restriction
            return;
        }
        void AddRemoveAngleRestriction(Vertex v)
        {
            //remove restriction
            if (v.restriction == Enums.VertexRestriction.angle)
            {
                v.restriction = Enums.VertexRestriction.none;
                pictureBox1.Refresh();
            }
            else if (v.restriction == Enums.VertexRestriction.none)
            {
                (float startAngle , float endAngle) = GetAngle(v);
                double currentAngle = Math.Abs(startAngle - endAngle);
                string value = currentAngle.ToString();
                if (InputBox.Show("Angle restriction", "Please input chosen angle for restriction ( 0° < α < 360° )", ref value) == DialogResult.OK)
                {
                    if (double.TryParse(value, out double result) && result > 0 && result < 360)
                    {
                        v.angle = result;
                        v.restriction = Enums.VertexRestriction.angle;
                        SetAngle((int)result, v);
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
                plotLine((int)edge.From.X, (int)edge.From.Y, (int)edge.To.X, (int)edge.To.Y, e);
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
                        e.Graphics.DrawIcon(Properties.Resources.horizontal, (int)(edge.From.X + edge.To.X - Constants.iconSize) /2 , (int)((edge.From.Y + edge.To.Y - Constants.iconSize) /2 + Constants.distance));
                        break;
                    case Enums.EdgeRestriction.vertical:
                        e.Graphics.DrawIcon(Properties.Resources.vertical, (int)((edge.From.X + edge.To.X - Constants.iconSize) / 2 + Constants.distance), (int)(edge.From.Y + edge.To.Y - Constants.iconSize) / 2 );
                        break;
                }
            }

            foreach (Vertex v in vertices)
            {
                Rectangle rect = new Rectangle((int)(v.X - Constants.vertexSize / 2), ((int)v.Y - Constants.vertexSize / 2), Constants.vertexSize, Constants.vertexSize);
                e.Graphics.FillEllipse(mySolidBrush, rect);
                e.Graphics.DrawEllipse(Constants.pen, rect);

                if(v.restriction == Enums.VertexRestriction.angle)
                    e.Graphics.DrawIcon(Properties.Resources.angle, (int)(v.X - Constants.iconSize / 2 ) , (int)(v.Y + Constants.distance - Constants.iconSize / 2) );
                
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

            //selectedVertex.X = x;
            //selectedVertex.Y = y;
            
            KeepConstraints(vertices, edges, selectedVertex, x, y);
            pictureBox1.Refresh();
        }

        //Edge to the right
        public Edge NextEdge(Vertex v)
        {
            int index = vertices.FindIndex(c => c == v);
            return edges[index];
        }
        //Edge to the left
        public Edge PreviousEdge(Vertex v)
        {
            int index = vertices.FindIndex(c => c == v);
            return edges[(index - 1 + edges.Count) % edges.Count];
        }
        //Prev
        public Vertex NextVertex(Vertex v)
        {
            int index = vertices.FindIndex(c => c == v);
            return vertices[(index + 1) % vertices.Count];
        }
        //Edge to the left
        public Vertex PreviousVertex(Vertex v)
        {
            int index = vertices.FindIndex(c => c == v);
            return vertices[(index - 1 + vertices.Count) % vertices.Count];
        }

        private void KeepConstraints(List<Vertex> vertices, List<Edge> edges, Vertex oldVertex, int newX, int newY)
        {           
           
            Vertex currentVertex = oldVertex;

            Vertex nextVertex = NextVertex(currentVertex);

            //Go right

            if (currentVertex.restriction == Enums.VertexRestriction.none && NextEdge(currentVertex).restriction == Enums.EdgeRestriction.none && nextVertex.restriction == Enums.VertexRestriction.none)
            {
                //do nothing, as everything is correct and there is no need to fix anything. You can move the vertex and will keep all constraints
            }
            else if (currentVertex.restriction == Enums.VertexRestriction.angle && nextVertex.restriction == Enums.VertexRestriction.none && NextVertex(nextVertex).restriction == Enums.VertexRestriction.none)
            {
                Vector v = new Vector(newX - oldVertex.X, newY - oldVertex.Y);
                (Vector c1, Vector c2) = v.GetComponent(PreviousEdge(currentVertex).toVector(), NextEdge(currentVertex).toVector());

                nextVertex.X += c1.X;
                nextVertex.Y += c1.Y;
            }
            else
            {
                Edge e1 = NextEdge(currentVertex);
                Edge e2 = NextEdge(nextVertex);
                Vector v = new Vector(newX - oldVertex.X, newY - oldVertex.Y);
                (Vector c1, Vector c2) = v.GetComponent(e1.toVector(), e2.toVector());

                nextVertex.X += c2.X;
                nextVertex.Y += c2.Y;
            }

            //Go left

            Vertex previousVertex = PreviousVertex(currentVertex);

            if (currentVertex.restriction == Enums.VertexRestriction.none && PreviousEdge(currentVertex).restriction == Enums.EdgeRestriction.none && previousVertex.restriction == Enums.VertexRestriction.none)
            {
                //do nothing, as everything is correct and there is no need to fix anything. You can move the vertex and will keep all constraints
            }
            else if (currentVertex.restriction == Enums.VertexRestriction.angle && previousVertex.restriction == Enums.VertexRestriction.none && PreviousVertex(previousVertex).restriction == Enums.VertexRestriction.none)
            {
                Vector v = new Vector(newX - oldVertex.X, newY - oldVertex.Y);
                (Vector c1, Vector c2) = v.GetComponent(PreviousEdge(currentVertex).toVector(), NextEdge(currentVertex).toVector());                
                previousVertex.X += c2.X;
                previousVertex.Y += c2.Y;
            }
            else
            {
                Edge e1 = PreviousEdge(currentVertex);
                Edge e2 = PreviousEdge(previousVertex);
                Vector v = new Vector(newX - oldVertex.X, newY - oldVertex.Y);
                (Vector c1, Vector c2) = v.GetComponent(e1.toVector(), e2.toVector());
                previousVertex.X += c2.X;
                previousVertex.Y += c2.Y;
            }

            currentVertex.X = newX;
            currentVertex.Y = newY;
        }

        //Can break constraints, unfortunately. Kinda a binary search to find the correct angle
        private void SetAngle(int angle, Vertex vertex)
        {
            Vertex v1 = PreviousVertex(vertex);
            Edge e1 = PreviousEdge(vertex);
            Vertex v2 = NextVertex(vertex);
            Edge e2 = NextEdge(vertex);
            Vertex v = v1;
            Edge e = e1;
            (double startAngle, double endAngle) = GetAngle(vertex);
            // either 1 or -1
            int sign = 1;
            // can't start from 1, as it will be too slow
            int mul = 32;
            double currentAngle = Math.Abs(startAngle - endAngle);
            double oldDifference = Math.Abs(currentAngle - angle);
            Vector vector = RotationVector(e, sign);
            v.X += vector.X;
            v.Y += vector.Y;

            (startAngle, endAngle) = GetAngle(vertex);
            currentAngle = Math.Abs(startAngle - endAngle);
            double newDifference = Math.Abs(currentAngle - angle);
            //we moved to far, so we need to go back
            if (oldDifference < newDifference) sign *= -1;
            oldDifference = newDifference;

            // +- 0.5 degree
            while (oldDifference > 0.5)
            {
                //Binary search, move and if we moved to much return and move slower/lesser
                vector = RotationVector(e, mul * sign);
                v.X += vector.X;
                v.Y += vector.Y;
                (startAngle, endAngle) = GetAngle(vertex);
                currentAngle = Math.Abs(startAngle - endAngle);
                newDifference = Math.Abs(currentAngle - angle);
                if (newDifference >= oldDifference)
                {
                    v.X += vector.X * (-2);
                    v.Y += vector.Y * (-2);
                    (startAngle, endAngle) = GetAngle(vertex);
                    currentAngle = Math.Abs(startAngle - endAngle);
                    newDifference = Math.Abs(currentAngle - angle);
                    mul = mul / 2;
                }
                oldDifference = newDifference;
            }
        }
        

        public Vector RotationVector(Edge edge, int mul)
        {
            Vector vector = edge.toVector().GetPerpendicularVector().ToUnitVector() * mul;
            return vector;
        }

        public (float, float) GetAngle(Vertex vertex)
        {
            Edge firstEdge = NextEdge(vertex);
            Edge secondEdge = PreviousEdge(vertex);
            double x = secondEdge.From.X - vertex.X;
            double y = secondEdge.From.Y - vertex.Y;
            //Realised too late Atan2 takes y1 first, then x1. So i just pass the arguments in reverse.
            double startAngle = FindDegree(y, x);
            x = firstEdge.To.X - vertex.X;
            y = firstEdge.To.Y - vertex.Y;
            double endAngle = FindDegree(y, x);

            // We want start < end every time, so just add 360
            if (endAngle < startAngle) endAngle += 360;
            float testAngle = (float)(startAngle + endAngle) / 2;
            (double newx, double newy) = PolarToCartesian(3, testAngle);
            newx += vertex.X;
            newy += vertex.Y;

            //If we didn't take the correct angle , we need to take the other one. We want the angle from inside of the polygon.
            if (!isInsidePolygon(edges, (int)Math.Round(newx, 0), (int)Math.Round(newy)))
            {
                double helpAngle = 360 - Math.Abs(endAngle - startAngle);
                startAngle = endAngle;
                endAngle = startAngle + helpAngle;
            }
            return ((float)startAngle, (float)endAngle);

            
            float FindDegree(double x1, double y1)
            {
                float value = (float)((Math.Atan2(x1, y1) / Math.PI) * 180f);
                if (value < 0) value += 360f;
                return value;
            }
        }

        

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
    //(double x, double y) GetNewPoint(Vertex v1, Vertex v2, int degree , Enums.EdgeRestriction restriction, Vertex v3)
    //{
    //    double DegreeToRadian(int deg)
    //    {
    //       return deg * Math.PI / 180; 
    //    }
    //    double newx = int.MaxValue;
    //    double newy = int.MaxValue;
    //    // new line with correct degree between the lines - WRONG
     //    double a = Math.Tan(DegreeToRadian(degree));
    //    bool verticalLine = degree / 90 == degree % 90; //tg (90*) is -inf/inf
    //    double b = verticalLine? v1.Y : v1.Y - a * v1.X;

    //    switch(restriction)
    //    {
    //        case Enums.EdgeRestriction.horizontal:
    //            newy = v3.Y;

    //            //newx =  a == 0 || a ? (v3.X == b ? v3.X : int.MaxValue) : (int)((v3.Y - b) / a);

    //            if (a == 0)
    //            {
    //                // y = ax + b --a=0--> y = b
    //                if (v3.X == b)
    //                    newx = b;
    //                else
    //                    //will never be correct
    //                    newx = int.MaxValue;
    //            }
    //            else if (verticalLine)
    //            {
    //                newx = v2.X;
    //                newx = (int)((v3.Y - b) / a);
    //            }


    //            break;
    //        case Enums.EdgeRestriction.vertical:

    //            if(verticalLine)
    //            {
    //                newx = v3.X == v2.X ? v3.X : int.MaxValue;
    //            }
    //            else
    //            {
    //                newx = v3.X;
    //                newy = (int)(a * newx + b);
    //            }

    //            break;
    //        case Enums.EdgeRestriction.none:
    //            Point FindPerpendicular(Point start, Point end, Point target)
    //            {
    //                //HORIZONTAL
    //                if (end.Y == start.Y)
    //                {
    //                    if (start.X > end.X)
    //                    {
    //                        Point tmp = start;
    //                        start = end;
    //                        end = tmp;
    //                    }
    //                    if (target.X < start.X)
    //                    {
    //                        return start;
    //                    }
    //                    if (target.X > end.X)
    //                    {
    //                        return end;
    //                    }
    //                    return new Point(target.X, start.Y);
    //                }

    //                if (start.Y > end.Y)
    //                {
    //                    Point tmp = start;
    //                    start = end;
    //                    end = tmp;
    //                }
    //                //VERTICAL
    //                if (end.X == start.X)
    //                {
    //                    if (target.Y < start.Y)
    //                    {
    //                        return start;
    //                    }
    //                    if (target.Y > end.Y)
    //                    {
    //                        return end;
    //                    }
    //                    return new Point(start.X, target.Y);
    //                }

    //                double a1 = (double)(end.Y - start.Y) / (end.X - start.X);
    //                double b1 = start.Y - a1 * start.X;

    //                double aPerp = -1 / a1;
    //                double bPerp = target.Y - aPerp * target.X;

    //                double bMax = end.Y - aPerp * end.X;
    //                double bMin = start.Y - aPerp * start.X;

    //                if (bPerp > bMax)
    //                {
    //                    return end;
    //                }
    //                if (bPerp < bMin)
    //                {
    //                    return start;
    //                }

    //                double x0 = (a1 * (bPerp - b1)) / (a1 * a1 + 1);
    //                double y0 = a1 * x0 + b1;

    //                return new Point((int)x0, (int)y0);

    //            }
    //            int x = pictureBox1.Bottom;
    //            // Find closest point on a line from given point
    //            Point point = FindPerpendicular(new Point((int)v2.X, (int)v2.Y), new Point(x, (int)(a * x + b)), new Point((int)v3.X, (int)v3.Y);
    //            newx = point.X;
    //            newy = point.Y;
    //            break;

    //    }
    //    return (newx, newy);
    //}

    //private bool fixConstraints(List<Vertex> vertices, List<Edge> edges, Edge e, Vertex oldVertex, int newX, int newY )
    //{
    //    // Keeping a copy, as there is a possiblity of not being able to fullfill the constraints (for example angle - user inputting angle 180 when the angle is created by perpendicular line segments -> always 90*
    //    List<Vertex> verticesCopy = DeepClone(vertices);

    //    int startIndex;
    //    int currentVertex;
    //    int nextVertex;
    //    int count = 0;
    //    //Passed edge, so added a constraint on edge
    //    if (oldVertex == null)
    //    {
    //        //One point is correct, so i am taking the one with lower index in the list
    //        currentVertex = startIndex = vertices.FindIndex(u => u.X == e.From.X && u.Y == e.From.Y);
    //    }
    //    //Passed a vertex, so either moved it or added an angle constraint 
    //    else
    //    {
    //        currentVertex = startIndex = vertices.FindIndex(u => u.X == oldVertex.X && u.Y == oldVertex.Y);
    //        vertices[currentVertex].X = newX;
    //        vertices[currentVertex].Y = newY;
    //    }

    //    nextVertex = (startIndex + 1) % vertices.Count;           
    //    bool correct = false;
    //    //go right and fix
    //    do
    //    {//add vertex restriction plz
    //        Edge connecting = edges[currentVertex];
    //        if (connecting.restriction == Enums.EdgeRestriction.none)
    //        {
    //            correct = true;
    //        }
    //        else if (connecting.restriction == Enums.EdgeRestriction.horizontal)
    //        {
    //            if (vertices[nextVertex].Y == vertices[currentVertex].Y)
    //                correct = true;
    //            else
    //                vertices[nextVertex].Y = vertices[currentVertex].Y;

    //        }
    //        else if (connecting.restriction == Enums.EdgeRestriction.vertical)
    //        {
    //            if (vertices[nextVertex].X == vertices[currentVertex].X)
    //                correct = true;
    //            else
    //                vertices[nextVertex].X = vertices[currentVertex].X;
    //        }
    //        count++;
    //        currentVertex = nextVertex;
    //        nextVertex = (nextVertex + 1) % vertices.Count;
    //    }
    //    while (!correct && currentVertex != startIndex);
    //    // Returned to the starting vertex, can't fix it
    //    if(currentVertex == startIndex)
    //    {
    //        vertices = verticesCopy;
    //        return false;
    //    }
    //    currentVertex = startIndex;
    //    nextVertex = (startIndex - 1 + vertices.Count) % vertices.Count;
    //    correct = false;
    //    //go left and fix
    //   while ( !correct && currentVertex != (startIndex + count) % vertices.Count )
    //    {
    //        Edge connecting = edges[nextVertex];
    //        if (connecting.restriction == Enums.EdgeRestriction.none)
    //        {
    //            correct = true;
    //        }
    //        else if (connecting.restriction == Enums.EdgeRestriction.horizontal)
    //        {
    //            if (vertices[nextVertex].Y == vertices[currentVertex].Y)
    //                correct = true;
    //            else
    //                vertices[nextVertex].Y = vertices[currentVertex].Y;

    //        }
    //        else if (connecting.restriction == Enums.EdgeRestriction.vertical)
    //        {
    //            if (vertices[nextVertex].X == vertices[currentVertex].X)
    //                correct = true;
    //            else
    //                vertices[nextVertex].X = vertices[currentVertex].X;
    //        }

    //        currentVertex = nextVertex;
    //        nextVertex = (nextVertex - 1 + vertices.Count) % vertices.Count;
    //    }

    //   if(currentVertex == (startIndex + count) % vertices.Count)
    //    {
    //        vertices = verticesCopy;
    //        return false;
    //    }

    //    return true;
    //}
}


