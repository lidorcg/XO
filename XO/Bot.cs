using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace XO
{
    class Bot
    {
        private Node Knowledge;
        private Game.State Me;
        private Node MyState;
        private Stack<Edge> LastMoves;

        public Bot(Game.State me)
        {
            Knowledge = new Node(0.0);
            Me = me;
            MyState = Knowledge;
            LastMoves = new Stack<Edge>();

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    double hash = Hash(new int[2] { i, j });
                    MyState.AddMove(new Edge(MyState, hash));
                }
            }
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
            else
                Learn(game.GetBoard());
        }

        private bool IKnowThis(double hash)
        {
            Node n = SearchMemory(hash, MyState);
            if (n == null)
            {
                n = SearchMemory(hash, Knowledge);
                if (n != null)
                {
                    Connect(n);
                } else
                    return false;
            }
            MyState = n;
            return true;
        }

        private Node SearchMemory(double hash, Node n)
        {
            if (n != null)
            {
                if (hash == n.Hash)
                    return n;
                foreach (Edge e in n.Moves)
                {
                    Node node = SearchMemory(hash, e.Child);
                    if (node != null)
                        return node;
                }
            }
            return null;
        }

        private void Connect(Node n)
        {
            double hash = n.Hash - MyState.Hash;
            Edge e = new Edge(MyState, n, hash);
            MyState.AddMove(e);
        }

        private void Learn(Game.State[,] board)
        {
            Node n = new Node(Hash(board));
            Connect(n);
            MyState = n;
            LearnMoves(board);
        }

        private void LearnMoves(Game.State[,] board)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == Game.State.Blank)
                    {
                        double hash = Hash(new int[2] { i, j });
                        MyState.AddMove(new Edge(MyState, hash));
                    }
                }
            }
        }


        private void Decide(Game game)
        {
            if (Good())
                return;
            else
            {
                if (HasNewMove())
                    return;
                else
                {
                    FindBestMove();
                }
            }
        }

        private bool Good()
        {
            foreach (Edge e in MyState.Moves)
            {
                if (e.Score > 0)
                {
                    LastMoves.Push(e);
                    MyState = e.Child;
                    return true;
                }
            }
            return false;
        }

        private bool HasNewMove()
        {
            foreach (Edge e in MyState.Moves)
            {
                if (e.Child==null)
                {
                    Node n = SearchMemory(MyState.Hash + e.Hash, Knowledge);
                    if (n == null)
                        n = new Node(MyState.Hash + e.Hash);
                    e.Child = n;
                    LastMoves.Push(e);
                    MyState = e.Child;
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
            LastMoves.Push(m);
            MyState = m.Child;
        }


        private int[] Act()
        {
            return Decode(LastMoves.Peek().Hash);
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


        public void FeedBack(Game.State s)
        {
            double c;
            if (s == Game.State.Win)
            {
                c = 1;
            }
            else if (s == Game.State.Draw)
            {
                c = 0;
            }
            else
            {
                c = -1;
            }
            double n = 0;
            while (LastMoves.Count > 0)
            {
                Edge e = LastMoves.Pop();
                e.Score += c * f(n);
                n += 1;
            }
            MyState = Knowledge;
        }

        public double f(double n)
        {
            return Math.E / Math.Pow(Math.E, n);
        }


        private string PrintStack()
        {
            string s = "";
            foreach (Edge e in LastMoves)
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
