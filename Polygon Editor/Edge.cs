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
        /// Returns index of first overlapping vEdge or -1 if there is none </summary>  
        static public int containsEdge(List<Edge> edges, int leeway, int x, int y)
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
        static public bool inVicinityOfEdge(Edge edge, int leeway, int x, int y)
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
        
    
    }
}
