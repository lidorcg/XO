using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace XO
{
    class Bot
    {
        private List<Node> Knowledge = new List<Node>();
        private Game.State Me;
        private Node MyState;
        private Stack<Object> Trail = new Stack<Object>();

        public Bot(Game.State me)
        {
            Me = me;
        }

        public int[] Play(Game game)
        {
            Perceive(game);
            Decide(game);
            return Act();
        }

        private void Perceive(Game game)
        {
            if (IKnowThis(Hash(game.GetBoard())))
                return;
            Learn(game.GetBoard());
        }

        private bool IKnowThis(double hash)
        {
            Node n = SearchMemory(hash, MyState);
            if (n == null)
            {
                foreach (Node node in Knowledge)
                {
                    n = SearchMemory(hash, node);
                    if (n != null)
                    {
                        Connect(n);
                        MyState = n;
                        MyState.Explored = true;
                        Trail.Push(MyState);
                        return true;
                    }
                }
            }
            else
            {
                MyState = n;
                MyState.Explored = true;
                Trail.Push(MyState);
                return true;
            }
            return false;
        }

        private Node SearchMemory(double hash, Node n)
        {
            if (n != null)
            {
                if (hash == n.Hash)
                    return n;
                foreach (Edge e in n.Moves)
                {
                    foreach (Node n1 in e.Children)
                    {
                        Node node = SearchMemory(hash, n1);
                        if (node != null)
                            return node;
                    }
                }
            }
            return null;
        }

        private void Learn(Game.State[,] board)
        {
            Node n = new Node(Hash(board));
            LearnMoves(n, board);
            Connect(n);
            MyState = n;
            MyState.Explored = true;
            Trail.Push(MyState);
        }

        private void LearnMoves(Node n, Game.State[,] board)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == Game.State.Blank)
                    {
                        double hash = Hash(new int[2] { i, j });
                        n.Add(new Edge(hash));
                    }
                }
            }
        }

        private void Connect(Node n)
        {
            if (Trail.Count != 0)
            {
                Edge e = (Edge)Trail.Peek();
                e.Add(n);
            }
            else if(!Knowledge.Contains(n))
                Knowledge.Add(n);
        }


        private void Decide(Game game)
        {
            if (HasNewMove())
                return;
            else
                FindBestMove();
        }

        private bool HasNewMove()
        {
            foreach (Edge e in MyState.Moves)
            {
                if (!e.Explored)
                {
                    e.Explored = true;
                    Trail.Push(e);
                    return true;
                }
            }
            return false;
        }

        private void FindBestMove()
        {
            Edge m = MyState.Moves[0];
            foreach (Edge e in MyState.Moves)
            {
                if (e.Score > m.Score)
                {
                    m = e;
                }
            }
            Trail.Push(m);
        }


        private int[] Act()
        {
            Edge e = (Edge)Trail.Peek();
            return Decode(e.Hash);
        }


        private double Hash(Game.State[,] board)
        {
            double hash = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    double n = i * 1 + j * 3;
                    if (board[i, j] == Game.State.Blank)
                    {
                        hash += 0.0 * Math.Pow(3.0, n);
                    }
                    else if (board[i, j] == Me)
                    {
                        hash += 1.0 * Math.Pow(3.0, n);
                    }
                    else
                    {
                        hash += 2.0 * Math.Pow(3.0, n);
                    }
                }
            }
            return hash;
        }

        private double Hash(int[] move)
        {
            double n = move[0] * 1 + move[1] * 3;
            double hash = 1.0 * Math.Pow(3.0, n);
            return hash;
        }

        private int[] Decode(double p)
        {
            int[] move = new int[2];
            int ans = 0;
            while (p != 1 && p !=2)
            {
                p /= 3;
                ans++;
            }
            move[0] = ans % 3;
            move[1] = ans / 3;
            return move;
        }


        public void FeedBack(Game.State r)
        {
            if (r == Game.State.Win)
            {
                Edge m = (Edge)Trail.Pop();
                m.Score += 100;
                m.Explored = true;
            }
            else if (r == Game.State.Draw)
            {
                goto end;
            }
            else
            {
                Edge m = (Edge)Trail.Pop();
                m.Score -= 100;
                m.Explored = true;
            }

            while (Trail.Count > 0)
            {
                if (Trail.Count % 2 == 0)
                {
                    Edge m = (Edge)Trail.Pop();
                    foreach (Node n in m.Children)
                    {
                        if (!n.Explored)
                            m.Explored = false;
                        m.Score += n.Score;
                    }
                    m.Score /= m.Children.Count;
                }
                else if (Trail.Count % 2 == 1)
                {
                    Node s = (Node)Trail.Pop();
                    double score = double.MinValue;
                    foreach (Edge e in s.Moves)
                    {
                        if (!e.Explored)
                            s.Explored = false;
                        if (e.Score >= score)
                            score = e.Score;
                    }
                    s.Score = score;
                }
            }

            end:
                MyState = null;
                Trail = new Stack<Object>();
        }

        public double f(double n)
        {
            return Math.E / Math.Pow(Math.E, n);
        }


        private string PrintStack()
        {
            string s = "";
            foreach (Edge e in Trail)
            {
                s += "M" + e.Hash.ToString() + " -> ";
            }
            return s;
        }

        public override string ToString()
        {
            return Me.ToString();
        }
    }
}
