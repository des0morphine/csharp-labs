using System;
using System.ComponentModel;
using System.Windows.Input;
using TicTacToeLab2.Commands;
using TicTacToeLab2.Models;

namespace TicTacToeLab2.ViewModels
{
    public class CellViewModel : INotifyPropertyChanged
    {
        private CellState state = CellState.Empty;
        private bool isEnabled = true;

        public int Row { get; set; }
        public int Col { get; set; }

        public CellViewModel(int row, int col, Action<CellViewModel> onClick)
        {
            Row = row;
            Col = col;
            ClickCommand = new RelayCommand(p => onClick(this));
        }

        public CellState State
        {
            get { return state; }
            set
            {
                if (state != value)
                {
                    state = value;
                    OnPropertyChanged("State");
                    OnPropertyChanged("Text");
                }
            }
        }

        public string Text
        {
            get
            {
                if (state == CellState.X)
                {
                    return "X";
                }
                if (state == CellState.O)
                {
                    return "O";
                }
                return "";
            }
        }

        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                if (isEnabled != value)
                {
                    isEnabled = value;
                    OnPropertyChanged("IsEnabled");
                }
            }
        }

        public ICommand ClickCommand { get; }

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
