using System;
using TicTacToeLab2.Models;
using Xunit;

namespace TicTacToeLab2.Tests
{
    public class GameTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void TestRowWin(int row)
        {
            TicTacToeGame game = new TicTacToeGame();

            game.MakeMove(row, 0, CellState.X);
            game.MakeMove(row, 1, CellState.X);
            game.MakeMove(row, 2, CellState.X);

            GameResult result = game.CheckWinner();

            Assert.Equal(GameResult.XWins, result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void TestColWin(int col)
        {
            TicTacToeGame game = new TicTacToeGame();

            game.MakeMove(0, col, CellState.O);
            game.MakeMove(1, col, CellState.O);
            game.MakeMove(2, col, CellState.O);

            GameResult result = game.CheckWinner();

            Assert.Equal(GameResult.OWins, result);
        }

        [Fact]
        public void TestMainDiagonalWin()
        {
            TicTacToeGame game = new TicTacToeGame();

            game.MakeMove(0, 0, CellState.X);
            game.MakeMove(1, 1, CellState.X);
            game.MakeMove(2, 2, CellState.X);

            GameResult result = game.CheckWinner();

            Assert.Equal(GameResult.XWins, result);
        }

        [Fact]
        public void TestAntiDiagonalWin()
        {
            TicTacToeGame game = new TicTacToeGame();

            game.MakeMove(0, 2, CellState.O);
            game.MakeMove(1, 1, CellState.O);
            game.MakeMove(2, 0, CellState.O);

            GameResult result = game.CheckWinner();

            Assert.Equal(GameResult.OWins, result);
        }

        [Fact]
        public void TestDraw()
        {
            TicTacToeGame game = new TicTacToeGame();

            game.MakeMove(0, 0, CellState.X);
            game.MakeMove(0, 1, CellState.O);
            game.MakeMove(0, 2, CellState.X);

            game.MakeMove(1, 0, CellState.X);
            game.MakeMove(1, 1, CellState.X);
            game.MakeMove(1, 2, CellState.O);

            game.MakeMove(2, 0, CellState.O);
            game.MakeMove(2, 1, CellState.X);
            game.MakeMove(2, 2, CellState.O);

            GameResult result = game.CheckWinner();

            Assert.Equal(GameResult.Draw, result);
            Assert.True(game.IsDraw());
        }

        [Fact]
        public void TestOccupiedCell()
        {
            TicTacToeGame game = new TicTacToeGame();

            bool first = game.MakeMove(1, 1, CellState.X);
            bool second = game.MakeMove(1, 1, CellState.O);

            Assert.True(first);
            Assert.False(second);
            Assert.Equal(CellState.X, game.GetCell(1, 1));
        }

        [Fact]
        public void TestOutOfBounds()
        {
            TicTacToeGame game = new TicTacToeGame();

            Assert.Throws<ArgumentOutOfRangeException>(() => game.MakeMove(-1, 0, CellState.X));
            Assert.Throws<ArgumentOutOfRangeException>(() => game.MakeMove(3, 0, CellState.X));
        }

        [Fact]
        public void TestHardModeWinOverBlock()
        {
            TicTacToeGame game = new TicTacToeGame();

            game.MakeMove(0, 0, CellState.O);
            game.MakeMove(0, 1, CellState.O);

            game.MakeMove(1, 0, CellState.X);
            game.MakeMove(1, 1, CellState.X);

            var move = game.ChooseComputerMove(Difficulty.Hard, CellState.O);

            Assert.NotNull(move);
            Assert.Equal((0, 2), move.Value);
        }

        [Fact]
        public void TestMediumModeBlock()
        {
            TicTacToeGame game = new TicTacToeGame();

            game.MakeMove(2, 0, CellState.X);
            game.MakeMove(2, 1, CellState.X);

            var move = game.ChooseComputerMove(Difficulty.Medium, CellState.O);

            Assert.NotNull(move);
            Assert.Equal((2, 2), move.Value);
        }

        [Fact]
        public void TestEasyMode()
        {
            TicTacToeGame game = new TicTacToeGame();

            game.MakeMove(0, 0, CellState.X);

            var move = game.ChooseComputerMove(Difficulty.Easy, CellState.O);

            Assert.NotNull(move);
            Assert.NotEqual((0, 0), move.Value);
            Assert.Equal(CellState.Empty, game.GetCell(move.Value.Row, move.Value.Col));
        }
    }
}
