using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using TicTacToeLab2.Commands;
using TicTacToeLab2.Models;

namespace TicTacToeLab2.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private TicTacToeGame game = new TicTacToeGame();
        private Difficulty selectedDifficulty = Difficulty.Hard;
        private string statusMessage = "Ваш хід (X)";
        private bool isGameOver = false;

        private int playerScore = 0;
        private int computerScore = 0;
        private int drawScore = 0;

        public ObservableCollection<CellViewModel> Cells { get; }
        public ICommand NewGameCommand { get; }
        public ICommand ResetScoreCommand { get; }

        public MainViewModel()
        {
            Cells = new ObservableCollection<CellViewModel>();
            for (int r = 0; r < TicTacToeGame.BoardSize; r++)
            {
                for (int c = 0; c < TicTacToeGame.BoardSize; c++)
                {
                    Cells.Add(new CellViewModel(r, c, OnCellClicked));
                }
            }

            NewGameCommand = new RelayCommand(p => NewGame());
            ResetScoreCommand = new RelayCommand(p => ResetScore());
        }

        public Difficulty SelectedDifficulty
        {
            get { return selectedDifficulty; }
            set
            {
                if (selectedDifficulty != value)
                {
                    selectedDifficulty = value;
                    OnPropertyChanged("SelectedDifficulty");
                }
            }
        }

        public string StatusMessage
        {
            get { return statusMessage; }
            set
            {
                if (statusMessage != value)
                {
                    statusMessage = value;
                    OnPropertyChanged("StatusMessage");
                }
            }
        }

        public int PlayerScore
        {
            get { return playerScore; }
            set
            {
                if (playerScore != value)
                {
                    playerScore = value;
                    OnPropertyChanged("PlayerScore");
                }
            }
        }

        public int ComputerScore
        {
            get { return computerScore; }
            set
            {
                if (computerScore != value)
                {
                    computerScore = value;
                    OnPropertyChanged("ComputerScore");
                }
            }
        }

        public int DrawScore
        {
            get { return drawScore; }
            set
            {
                if (drawScore != value)
                {
                    drawScore = value;
                    OnPropertyChanged("DrawScore");
                }
            }
        }

        private void OnCellClicked(CellViewModel cell)
        {
            if (cell.State != CellState.Empty || isGameOver)
            {
                return;
            }

            if (!game.MakeMove(cell.Row, cell.Col, CellState.X))
            {
                return;
            }

            cell.State = CellState.X;
            cell.IsEnabled = false;

            GameResult result = game.CheckWinner();
            if (result != GameResult.InProgress)
            {
                HandleGameOver(result);
                return;
            }

            var compMove = game.ChooseComputerMove(selectedDifficulty, CellState.O);
            if (compMove.HasValue)
            {
                int cr = compMove.Value.Row;
                int cc = compMove.Value.Col;
                game.MakeMove(cr, cc, CellState.O);

                int index = cr * TicTacToeGame.BoardSize + cc;
                Cells[index].State = CellState.O;
                Cells[index].IsEnabled = false;
            }

            result = game.CheckWinner();
            if (result != GameResult.InProgress)
            {
                HandleGameOver(result);
            }
            else
            {
                StatusMessage = "Ваш хід (X)";
            }
        }

        private void HandleGameOver(GameResult result)
        {
            isGameOver = true;

            for (int i = 0; i < Cells.Count; i++)
            {
                Cells[i].IsEnabled = false;
            }

            if (result == GameResult.XWins)
            {
                PlayerScore++;
                StatusMessage = "Ви перемогли (X)!";
            }
            else if (result == GameResult.OWins)
            {
                ComputerScore++;
                StatusMessage = "Переміг комп'ютер (O)!";
            }
            else if (result == GameResult.Draw)
            {
                DrawScore++;
                StatusMessage = "Нічия!";
            }
        }

        public void NewGame()
        {
            game.Reset();
            isGameOver = false;

            for (int i = 0; i < Cells.Count; i++)
            {
                Cells[i].State = CellState.Empty;
                Cells[i].IsEnabled = true;
            }

            StatusMessage = "Нова партія! Ваш хід (X)";
        }

        public void ResetScore()
        {
            PlayerScore = 0;
            ComputerScore = 0;
            DrawScore = 0;
            NewGame();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
