using System.Collections.Generic;
using System.Diagnostics;

namespace XO
{
    [DebuggerDisplay("Exp = {Explored}, Scr = {Score}")]
    class Node
    {
        public double Hash;
        public List<Edge> Moves = new List<Edge>();
        public double Score = 0;
        public bool Explored = false;

        public Node(double hash)
        {
            Hash = hash;
        }

        public void Add(Edge e)
        {
            if (!Moves.Contains(e))
            {
                Moves.Add(e);
            }
        }
    }
}
