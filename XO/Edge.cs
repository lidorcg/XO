using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XO
{
    [DebuggerDisplay("{Explored}")]
    class Edge
    {
        public double Hash;
        public List<Node> Children = new List<Node>();
        public double Score = 0;
        public bool Explored = false;

        public Edge(double h)
        {
            Hash = h;
        }

        public void Add(Node c)
        {
            if (!Children.Contains(c))
            {
                Children.Add(c);
            }
        }
    }
}
