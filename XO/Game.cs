
namespace XO
{
    class Game
    {
        public enum State { Blank, X, O, Win, Draw};

        int n = 3;
        State[,] board;
        int moveCount = 0;

        public Game()
        {
            board = new State[n, n];
        }

        public State play(int x, int y, State s)
        {
            if (board[x, y] != State.Blank)
            {
                return 0;
            }
            else
            {
                board[x, y] = s;
                moveCount += 1;
                return CheckBoard(x, y, s);
            }
        }

        public State[,] GetBoard()
        {
            return board;
        }

        private State CheckBoard(int x, int y, State s)
        {
            //check col
            for (int i = 0; i < n; i++)
            {
                if (board[x, i] != s)
                    break;
                if (i == n - 1)
                {
                    return State.Win;
                }
            }

            //check row
            for (int i = 0; i < n; i++)
            {
                if (board[i, y] != s)
                    break;
                if (i == n - 1)
                {
                    return State.Win;
                }
            }

            //check diag
            if (x == y)
            {
                for (int i = 0; i < n; i++)
                {
                    if (board[i, i] != s)
                        break;
                    if (i == n - 1)
                    {
                        return State.Win;
                    }
                }
            }

            for (int i = 0; i < n; i++)
            {
                if (board[i, (n - 1) - i] != s)
                    break;
                if (i == n - 1)
                {
                    return State.Win;
                }
            }

            if (moveCount == n * n)
            {
                return State.Draw;
            }
            return s;
        }
    }
}
