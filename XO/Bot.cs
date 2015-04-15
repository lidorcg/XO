using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace XO
{
    class Bot
    {
        private KnowledgeContainer Knowledge = new KnowledgeContainer();
        private Game.State Me;
        private State MyState;
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
            if (IKnowThis(Hash(game.GetBoard())));
            else
                Learn(game.GetBoard());
            MyState.Explored = true;
            Knowledge.SaveChanges();
        }

        private bool IKnowThis(int hash)
        {
            State n = SearchMemory(hash, MyState);
            if (n == null)
            {
                n = Knowledge.States.Find(hash);
                if (n != null)
                {
                    Connect(n);
                    MyState = n;
                    Trail.Push(MyState);
                    return true;
                }
            }
            else
            {
                MyState = n;
                Trail.Push(MyState);
                return true;
            }
            return false;
        }

        private State SearchMemory(double hash, State n)
        {
            if (n != null)
            {
                if (hash == n.Hash)
                    return n;
                foreach (Action e in n.Actions)
                {
                    foreach (State n1 in e.NextState)
                    {
                        State node = SearchMemory(hash, n1);
                        if (node != null)
                            return node;
                    }
                }
            }
            return null;
        }

        private void Learn(Game.State[,] board)
        {
            State n = new State { Hash = Hash(board) };
            Knowledge.States.Add(n);
            LearnMoves(n, board);
            Connect(n);
            MyState = n;
            Trail.Push(MyState);
        }

        private void LearnMoves(State n, Game.State[,] board)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == Game.State.Blank)
                    {
                        double hash = Hash(new int[2] { i, j });
                        n.Actions.Add(new Action{Hash = hash});
                    }
                }
            }
        }

        private void Connect(State n)
        {
            if (Trail.Count > 0)
            {
                Action e = (Action)Trail.Peek();
                e.NextState.Add(n);
            }
        }


        private void Decide(Game game)
        {
            if (HasGoodMove())
                return;
            else if (HasNewMove())
                return;
            else
                SomeMove();
        }

        private bool HasGoodMove()
        {
            List<Action> GoodMoves = new List<Action>();
            foreach (Action e in MyState.Actions)
                if (e.Score > 0)
                    GoodMoves.Add(e);
            if (GoodMoves.Count > 0)
            {
                Random random = new Random();
                int randomNumber = random.Next(0, GoodMoves.Count);
                Trail.Push(GoodMoves[randomNumber]);
                return true;
            }
            else
                return false;
        }

        private bool HasNewMove()
        {
            List<Action> NewMoves = new List<Action>();
            foreach (Action e in MyState.Actions)
                if (!e.Explored)
                    NewMoves.Add(e);
            if (NewMoves.Count > 0)
            {
                Random random = new Random();
                int randomNumber = random.Next(0, NewMoves.Count);
                Trail.Push(NewMoves[randomNumber]);
                return true;
            }
            else
                return false;
        }

        private void SomeMove()
        {
            List<Action> NeutralMoves = new List<Action>();
            Action m = new Action { Score = double.MinValue };
            foreach (Action e in MyState.Actions)
            {
                if (e.Score == 0)
                    NeutralMoves.Add(e);
                if (e.Score > m.Score)
                    m = e;
            }     
            if (NeutralMoves.Count > 0)
            {
                Random random = new Random();
                int randomNumber = random.Next(0, NeutralMoves.Count);
                Trail.Push(NeutralMoves[randomNumber]);
            }
            else
                Trail.Push(m);
        }


        private int[] Act()
        {
            Action e = (Action)Trail.Peek();
            e.Explored = true;
            Knowledge.SaveChanges();
            return Decode(e.Hash);
        }


        private int Hash(Game.State[,] board)
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
            return Convert.ToInt32(hash);
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
            Action m = (Action)Trail.Pop();
            if (r == Game.State.Win)
                m.Score = Math.E*100.0;
            else if (r == Game.State.Draw)
                m.Score = 0.0;
            else
                m.Score = -Math.E*100.0;

            while (Trail.Count > 0)
            {
                if (Trail.Count % 2 == 0)
                {
                    m = (Action)Trail.Pop();
                    double score = double.MaxValue;
                    foreach (State n in m.NextState)
                    {
                        if (n.Score < score)
                            score = n.Score;
                    }
                    m.Score = score;
                }
                else if (Trail.Count % 2 == 1)
                {
                    State s = (State)Trail.Pop();
                    double score = double.MinValue;
                    foreach (Action e in s.Actions)
                    {
                        if (!e.Explored)
                            s.Explored = false;
                        if (e.Score > score)
                            score = e.Score;
                    }
                    s.Score = score;
                }
            }
            Knowledge.SaveChanges();
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
            foreach (Action e in Trail)
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
