using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XO
{
    class Edge
    {
        public Node Parent;
        public Node Child;
        public double Hash;
        public double Score = 0;

        public Edge(Node p, Node c, double h)
        {
            Parent = p;
            Child = c;
            Hash = h;
        }

        public Edge(Node p, double h)
        {
            Parent = p;
            Hash = h;
        }
    }
}
