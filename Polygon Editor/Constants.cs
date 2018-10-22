using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polygon_Editor
{
    static class Constants
    {
        // Leeway in clicking vertices or edges
        static public int leeway = 20; 
        // Radius of the vertex
        static public int vertexSize = 10;
        // Thickness of the edge line
        static public int edgeThickness = 2;
        // Pen for drawing
        static public Pen pen = new Pen(Color.Black, edgeThickness);
        // Brush for drawing
        static public Brush brush = Brushes.Black;
        //distance of an icon from the vertex/edge
        static public int distance = 20;
        //icon size
        static public int iconSize = 16;

    }
}
