using System.Collections.Generic;

namespace XO
{
    class Node
    {
        public double Hash;
        public List<Edge> Moves = new List<Edge>();

        public Node(double hash)
        {
            Hash = hash;
        }

        public void AddMove(Edge e)
        {
            Moves.Add(e);
        }

        public override string ToString()
        {
            string s = "";
            s += "S" + Hash.ToString() + ": ";
            foreach (Edge e in Moves)
            {
                s += "\nM" + e.Hash.ToString() + " -> " + e.Child.ToString();
            }
            return s;
        }
    }
}
