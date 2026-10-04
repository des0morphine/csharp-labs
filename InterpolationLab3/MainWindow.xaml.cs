using System.Windows;
using InterpolationLab3.ViewModels;

namespace InterpolationLab3
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void PlotCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            MainViewModel? vm = DataContext as MainViewModel;
            if (vm != null)
            {
                vm.CanvasWidth = e.NewSize.Width;
                vm.CanvasHeight = e.NewSize.Height;
            }
        }
    }
}