using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polygon_Editor
{
    public class Edge
    {
        public Vertex From { get; set; }
        public Vertex To { get; set; }
        public Enums.EdgeRestriction restriction;
        public Edge(Vertex from, Vertex to, Enums.EdgeRestriction restriction = Enums.EdgeRestriction.none )
        {
            From = from;
            To = to;
            this.restriction = restriction; 
        }
        /// <summary> Function checking if given point (x, y) overlaps with an edge.
        /// edges - list containing edges,
        /// leeway - minimum distance between point and edge , that is considered not to be overlapping, 
        /// x, y - given point.
        /// Returns index of first overlapping edge or -1 if there is none </summary>  
        static public int containsEdge(List<Edge> edges, double leeway, double x, double y)
        {
            for (int i = 0; i < edges.Count; i++)
            {
                if (inVicinityOfEdge(edges[i], leeway, x, y))
                    return i;
            }
            return -1;
        }

        ///<summary> Function checking if given point (x, y) is in vicinity of the edge.
        /// leeway - minimum distance between point and edge , that is considered not to be overlapping,
        /// x, y - given point </summary>
        static public bool inVicinityOfEdge(Edge edge, double leeway, double x, double y)
        {
            double sqr(double q) { return q * q; }

            double dist2(double rx, double ry, double tx, double ty ) { return sqr(rx - tx) + sqr(ry - ty); }

            double vx = edge.From.X;
            double vy = edge.From.Y;
            double wx = edge.To.X;
            double wy = edge.To.Y;

            double l2 = dist2(vx,vy,wx,wy);
            if (l2 == 0)
                return dist2(x, y, edge.From.X, edge.From.Y) < leeway;

            var t = ((x - vx) * (wx - vx) + (y - vy) * (wy - vy)) / l2;
            t = Math.Max(0, Math.Min(1, t));
            return dist2(x, y, vx + t * (wx - vx), vy + t * (wy - vy)) < leeway * leeway;
        }

        //returns true if edge is intersecting with given edge
         public bool isIntersecting(Edge e2)
        {
            // Given three colinear points p, q, r, the function checks if 
            // point q lies on line segment 'pr' 
            bool onSegment(double px, double py, double qx, double qy, double rx, double ry)
            {
                if (qx <= Math.Max(px, rx) && qx >= Math.Min(px, rx) &&
                    qy <= Math.Max(py, ry) && qy >= Math.Min(py, ry))
                    return true;

                return false;
            }

            // To find orientation of ordered triplet (p, q, r). 
            // The function returns following values 
            // 0 --> p, q and r are colinear 
            // 1 --> Clockwise 
            // 2 --> Counterclockwise 
            int orientation(double px, double py, double qx, double qy, double rx, double ry)
            {
                
                int val = (int)((qy - py) * (rx - qx) - (qx - px) * (ry - qy));
                //colinear or (clock or counterclock wise) 
                return val == 0 ? 0 : ((val > 0) ? 1 : 2); 
            }

            double p1x = From.X;
            double p1y = From.Y;
            double q1x = To.X;
            double q1y = To.Y;
            double p2x = e2.From.X;
            double p2y = e2.From.Y;
            double q2x = e2.To.X;
            double q2y = e2.To.Y;
            
            // Find the four orientations needed for general and 
            // special cases 
            int o1 = orientation(p1x, p1y, q1x, q1y, p2x, p2y);
            int o2 = orientation(p1x, p1y, q1x, q1y, q2x, q2y);
            int o3 = orientation(p2x, p2y, q2x, q2y, p1x, p1y);
            int o4 = orientation(p2x, p2y, q2x, q2y, q1x, q2y);

            // General case 
            if (o1 != o2 && o3 != o4)
                return true;

            // Special Cases 
            // p1, q1 and p2 are colinear and p2 lies on segment p1q1 
            if (o1 == 0 && onSegment(p1x, p1y, p2x, p2y, q1x, q1y)) return true;

            // p1, q1 and q2 are colinear and q2 lies on segment p1q1 
            if (o2 == 0 && onSegment(p1x, p1y, q2x, q2y, q1x, q1y)) return true;

            // p2, q2 and p1 are colinear and p1 lies on segment p2q2 
            if (o3 == 0 && onSegment(p2x, p2y, p1x, p1y, q2x, q2y)) return true;

            // p2, q2 and q1 are colinear and q1 lies on segment p2q2 
            if (o4 == 0 && onSegment(p2x, p2y, q1x, q1y, q2x, q2y)) return true;

            return false; // Doesn't fall in any of the above cases 
            
        }

    
    }
}
