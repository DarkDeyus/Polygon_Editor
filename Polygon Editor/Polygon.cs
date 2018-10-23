using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polygon_Editor
{
    class Polygon
    {
        public List<Vertex> vertices;
        public List<Edge> edges;

        public Polygon()
        {
            vertices = new List<Vertex>();
            edges = new List<Edge>();
        }
    }
}
