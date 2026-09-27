using System;
using System.Collections.Generic;

namespace TicTacToeLab2.Models
{
    public class TicTacToeGame
    {
        public const int BoardSize = 3;

        private CellState[,] board = new CellState[BoardSize, BoardSize];
        private Random rnd = new Random();

        public CellState GetCell(int r, int c)
        {
            Validate(r, c);
            return board[r, c];
        }

        public bool MakeMove(int r, int c, CellState player)
        {
            Validate(r, c);
            if (board[r, c] != CellState.Empty)
            {
                return false;
            }

            board[r, c] = player;
            return true;
        }

        public List<(int Row, int Col)> GetAvailableMoves()
        {
            List<(int Row, int Col)> list = new List<(int Row, int Col)>();
            for (int r = 0; r < BoardSize; r++)
            {
                for (int c = 0; c < BoardSize; c++)
                {
                    if (board[r, c] == CellState.Empty)
                    {
                        list.Add((r, c));
                    }
                }
            }
            return list;
        }

        public GameResult CheckWinner()
        {
            for (int i = 0; i < BoardSize; i++)
            {
                if (board[i, 0] != CellState.Empty && board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2])
                {
                    return board[i, 0] == CellState.X ? GameResult.XWins : GameResult.OWins;
                }

                if (board[0, i] != CellState.Empty && board[0, i] == board[1, i] && board[1, i] == board[2, i])
                {
                    return board[0, i] == CellState.X ? GameResult.XWins : GameResult.OWins;
                }
            }

            if (board[0, 0] != CellState.Empty && board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])
            {
                return board[0, 0] == CellState.X ? GameResult.XWins : GameResult.OWins;
            }

            if (board[0, 2] != CellState.Empty && board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])
            {
                return board[0, 2] == CellState.X ? GameResult.XWins : GameResult.OWins;
            }

            for (int r = 0; r < BoardSize; r++)
            {
                for (int c = 0; c < BoardSize; c++)
                {
                    if (board[r, c] == CellState.Empty)
                    {
                        return GameResult.InProgress;
                    }
                }
            }

            return GameResult.Draw;
        }

        public bool IsDraw()
        {
            return CheckWinner() == GameResult.Draw;
        }

        public (int Row, int Col)? ChooseComputerMove(Difficulty diff, CellState comp = CellState.O)
        {
            List<(int Row, int Col)> moves = GetAvailableMoves();
            if (moves.Count == 0)
            {
                return null;
            }

            CellState human = (comp == CellState.O) ? CellState.X : CellState.O;

            if (diff == Difficulty.Easy)
            {
                return moves[rnd.Next(moves.Count)];
            }

            if (diff == Difficulty.Medium)
            {
                var block = FindWinningMove(human, moves);
                if (block != null)
                {
                    return block;
                }
                return moves[rnd.Next(moves.Count)];
            }

            if (diff == Difficulty.Hard)
            {
                var win = FindWinningMove(comp, moves);
                if (win != null)
                {
                    return win;
                }

                var block = FindWinningMove(human, moves);
                if (block != null)
                {
                    return block;
                }

                return moves[rnd.Next(moves.Count)];
            }

            return moves[rnd.Next(moves.Count)];
        }

        private (int Row, int Col)? FindWinningMove(CellState p, List<(int Row, int Col)> moves)
        {
            GameResult target = (p == CellState.X) ? GameResult.XWins : GameResult.OWins;

            for (int i = 0; i < moves.Count; i++)
            {
                int r = moves[i].Row;
                int c = moves[i].Col;

                board[r, c] = p;
                bool win = (CheckWinner() == target);
                board[r, c] = CellState.Empty;

                if (win)
                {
                    return (r, c);
                }
            }

            return null;
        }

        public void Reset()
        {
            for (int r = 0; r < BoardSize; r++)
            {
                for (int c = 0; c < BoardSize; c++)
                {
                    board[r, c] = CellState.Empty;
                }
            }
        }

        private void Validate(int r, int c)
        {
            if (r < 0 || r >= BoardSize || c < 0 || c >= BoardSize)
            {
                throw new ArgumentOutOfRangeException("Невірні координати");
            }
        }
    }
}
