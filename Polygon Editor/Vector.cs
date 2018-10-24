using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polygon_Editor
{
    public class Vector
    {
        public double X { get; set; }
        public double Y { get; set; }
        private double tolerance = 0.000001;
        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }
        public Vector GetPerpendicularVector()
        {
            Vector vector = new Vector(Y, X * (-1));
            if (vector.Y > 0) vector *= (-1);
            if (vector.Y == 0 && vector.X < 0) vector *= (-1);
            return vector;
        }
        public Vector ToUnitVector()
        {
            double l = Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2));
            if (l < tolerance) return this;
            return new Vector(X / l, Y / l);
        }
        public static Vector operator *(Vector vector, double a)
        {
            vector.X *= a;
            vector.Y *= a;
            return vector;
        }        
        public double ToLine()
        {
            return Y / X;
        }       
        public (Vector, Vector) GetComponent(Vector v1, Vector v2)
        {
            Vector c1;
            Vector c2;
            double a1;
            double a2;
            double b;
            double x1;
            double y1;
            double x2;
            double y2;
            if (v1.X == 0 && v2.X != 0)
            {
                a2 = v2.ToLine();
                b = Y - a2 * X;
                x1 = 0;
                y1 = b;

                x2 = X;
                y2 = X * a2;
                c1 = new Vector(x1, y1);
                c2 = new Vector(x2, y2);
                return (c1, c2);
            }
            if (v2.X == 0 && v1.X != 0)
            {
                a1 = v1.ToLine();
                x1 = X;
                y1 = X * a1;

                b = Y - a1 * X;
                x2 = 0;
                y2 = b;
                c1 = new Vector(x1, y1);
                c2 = new Vector(x2, y2);
                return (c1, c2);
            }
            a1 = v1.ToLine();
            a2 = v2.ToLine();
            b = Y - a2 * X;
            x1 = b / (a1 - a2);
            y1 = a1 * x1;

            b = Y - a1 * X;
            x2 = b / (a2 - a1);
            y2 = a2 * x2;
            c1 = new Vector(x1, y1);
            c2 = new Vector(x2, y2);
            return (c1, c2);
        }
    }
}
