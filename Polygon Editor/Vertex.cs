using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polygon_Editor
{
    public class Vertex
    {
        public double X { get; set; }
        public double Y { get; set; }
        public Enums.VertexRestriction restriction;
        public double angle;
        public Vertex(double x, double y, Enums.VertexRestriction restriction = Enums.VertexRestriction.none, double angle = 0)
        {
            X = x;
            Y = y;
            this.restriction = restriction;
            this.angle = angle;

        }

        /// <summary> Function for checking if given vertex v overlaps with already existing one.
        /// vertices - list containing vertices,
        /// leeway - minimum distance between two vertices , that is considered not to be overlapping, 
        /// v - given vertex.
        /// Returns index of first overlapping vertex or -1 if there is none </summary> 
        static public int containsVertex(List<Vertex> vertices, double leeway, Vertex v)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                double a = vertices[i].X - v.X;
                double b = vertices[i].Y - v.Y;

                //Pythagoras theorem, faster to square than to take a square root 
                if (a * a + b * b < leeway * leeway)
                    return i;
            }
            return -1;
        }

        public Vertex Clone()
        {
            return new Vertex(X, Y, restriction, angle);
        }
    }

    
    
}
