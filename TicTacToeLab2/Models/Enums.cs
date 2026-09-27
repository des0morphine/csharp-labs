namespace TicTacToeLab2.Models
{
    public enum CellState
    {
        Empty,
        X,
        O
    }

    public enum GameResult
    {
        InProgress,
        XWins,
        OWins,
        Draw
    }

    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }
}
