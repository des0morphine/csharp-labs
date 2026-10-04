using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using InterpolationLab3.Commands;
using InterpolationLab3.Models;

namespace InterpolationLab3.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private Interpolation[] methods = new Interpolation[]
        {
            new LinearInterpolation(),
            new HermiteInterpolation()
        };

        private double xStart = 0.0;
        private double xEnd = 6.28;
        private double h1 = 1.0;
        private double h2 = 0.05;

        private bool showReference = true;
        private bool showPoints = true;
        private bool showLinear = true;
        private bool showHermite = true;
        private bool showErrorMode = false;

        private string linearMaxError = "0";
        private string hermiteMaxError = "0";
        private int nodesCount = 0;
        private int interpolationPointsCount = 0;
        private string statusMessage = "";

        private double canvasWidth = 600;
        private double canvasHeight = 400;

        private PointCollection referencePoints = new PointCollection();
        private PointCollection linearPoints = new PointCollection();
        private PointCollection hermitePoints = new PointCollection();
        private PointCollection linearErrorPoints = new PointCollection();
        private PointCollection hermiteErrorPoints = new PointCollection();
        private ObservableCollection<Point> screenNodes = new ObservableCollection<Point>();

        private double axisYPosition = 200;
        private double axisXPosition = 40;

        public ICommand CalculateCommand { get; }

        public MainViewModel()
        {
            CalculateCommand = new RelayCommand(p => Calculate());
            Calculate();
        }

        public double XStart
        {
            get { return xStart; }
            set
            {
                if (xStart != value)
                {
                    xStart = value;
                    OnPropertyChanged("XStart");
                }
            }
        }

        public double XEnd
        {
            get { return xEnd; }
            set
            {
                if (xEnd != value)
                {
                    xEnd = value;
                    OnPropertyChanged("XEnd");
                }
            }
        }

        public double H1
        {
            get { return h1; }
            set
            {
                if (h1 != value)
                {
                    h1 = value;
                    OnPropertyChanged("H1");
                }
            }
        }

        public double H2
        {
            get { return h2; }
            set
            {
                if (h2 != value)
                {
                    h2 = value;
                    OnPropertyChanged("H2");
                }
            }
        }

        public bool ShowReference
        {
            get { return showReference; }
            set
            {
                if (showReference != value)
                {
                    showReference = value;
                    OnPropertyChanged("ShowReference");
                    Calculate();
                }
            }
        }

        public bool ShowPoints
        {
            get { return showPoints; }
            set
            {
                if (showPoints != value)
                {
                    showPoints = value;
                    OnPropertyChanged("ShowPoints");
                    Calculate();
                }
            }
        }

        public bool ShowLinear
        {
            get { return showLinear; }
            set
            {
                if (showLinear != value)
                {
                    showLinear = value;
                    OnPropertyChanged("ShowLinear");
                    Calculate();
                }
            }
        }

        public bool ShowHermite
        {
            get { return showHermite; }
            set
            {
                if (showHermite != value)
                {
                    showHermite = value;
                    OnPropertyChanged("ShowHermite");
                    Calculate();
                }
            }
        }

        public bool ShowErrorMode
        {
            get { return showErrorMode; }
            set
            {
                if (showErrorMode != value)
                {
                    showErrorMode = value;
                    OnPropertyChanged("ShowErrorMode");
                    Calculate();
                }
            }
        }

        public string LinearMaxError
        {
            get { return linearMaxError; }
            set
            {
                if (linearMaxError != value)
                {
                    linearMaxError = value;
                    OnPropertyChanged("LinearMaxError");
                }
            }
        }

        public string HermiteMaxError
        {
            get { return hermiteMaxError; }
            set
            {
                if (hermiteMaxError != value)
                {
                    hermiteMaxError = value;
                    OnPropertyChanged("HermiteMaxError");
                }
            }
        }

        public int NodesCount
        {
            get { return nodesCount; }
            set
            {
                if (nodesCount != value)
                {
                    nodesCount = value;
                    OnPropertyChanged("NodesCount");
                }
            }
        }

        public int InterpolationPointsCount
        {
            get { return interpolationPointsCount; }
            set
            {
                if (interpolationPointsCount != value)
                {
                    interpolationPointsCount = value;
                    OnPropertyChanged("InterpolationPointsCount");
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

        public double CanvasWidth
        {
            get { return canvasWidth; }
            set
            {
                if (canvasWidth != value)
                {
                    canvasWidth = value;
                    OnPropertyChanged("CanvasWidth");
                    Calculate();
                }
            }
        }

        public double CanvasHeight
        {
            get { return canvasHeight; }
            set
            {
                if (canvasHeight != value)
                {
                    canvasHeight = value;
                    OnPropertyChanged("CanvasHeight");
                    Calculate();
                }
            }
        }

        public PointCollection ReferencePoints
        {
            get { return referencePoints; }
            set
            {
                referencePoints = value;
                OnPropertyChanged("ReferencePoints");
            }
        }

        public PointCollection LinearPoints
        {
            get { return linearPoints; }
            set
            {
                linearPoints = value;
                OnPropertyChanged("LinearPoints");
            }
        }

        public PointCollection HermitePoints
        {
            get { return hermitePoints; }
            set
            {
                hermitePoints = value;
                OnPropertyChanged("HermitePoints");
            }
        }

        public PointCollection LinearErrorPoints
        {
            get { return linearErrorPoints; }
            set
            {
                linearErrorPoints = value;
                OnPropertyChanged("LinearErrorPoints");
            }
        }

        public PointCollection HermiteErrorPoints
        {
            get { return hermiteErrorPoints; }
            set
            {
                hermiteErrorPoints = value;
                OnPropertyChanged("HermiteErrorPoints");
            }
        }

        public ObservableCollection<Point> ScreenNodes
        {
            get { return screenNodes; }
            set
            {
                screenNodes = value;
                OnPropertyChanged("ScreenNodes");
            }
        }

        public double AxisYPosition
        {
            get { return axisYPosition; }
            set
            {
                axisYPosition = value;
                OnPropertyChanged("AxisYPosition");
            }
        }

        public double AxisXPosition
        {
            get { return axisXPosition; }
            set
            {
                axisXPosition = value;
                OnPropertyChanged("AxisXPosition");
            }
        }

        public void Calculate()
        {
            if (canvasWidth <= 50 || canvasHeight <= 50)
            {
                return;
            }

            if (h1 <= 0 || h2 <= 0)
            {
                StatusMessage = "Помилка: кроки h1 і h2 мають бути більшими за 0";
                ClearCurves();
                return;
            }

            if (h2 >= h1)
            {
                StatusMessage = "Помилка: крок h2 має бути меншим за h1";
                ClearCurves();
                return;
            }

            if (xEnd <= xStart)
            {
                StatusMessage = "Помилка: кінець діапазону має бути більшим за початок";
                ClearCurves();
                return;
            }

            StatusMessage = "";

            List<PointD> nodes = new List<PointD>();
            for (double x = xStart; x <= xEnd + 1e-9; x += h1)
            {
                nodes.Add(new PointD(x, Math.Sin(x)));
            }

            if (nodes[nodes.Count - 1].X < xEnd - 1e-9)
            {
                nodes.Add(new PointD(xEnd, Math.Sin(xEnd)));
            }

            NodesCount = nodes.Count;

            List<PointD> refList = new List<PointD>();
            List<PointD> linList = new List<PointD>();
            List<PointD> herList = new List<PointD>();
            List<PointD> linErrList = new List<PointD>();
            List<PointD> herErrList = new List<PointD>();

            double maxLinErr = 0;
            double maxHerErr = 0;

            for (double x = xStart; x <= xEnd + 1e-9; x += h2)
            {
                double yRef = Math.Sin(x);
                double yLin = methods[0].Calculate(nodes, x);
                double yHer = methods[1].Calculate(nodes, x);

                double errLin = Math.Abs(yLin - yRef);
                double errHer = Math.Abs(yHer - yRef);

                if (errLin > maxLinErr) maxLinErr = errLin;
                if (errHer > maxHerErr) maxHerErr = errHer;

                refList.Add(new PointD(x, yRef));
                linList.Add(new PointD(x, yLin));
                herList.Add(new PointD(x, yHer));
                linErrList.Add(new PointD(x, errLin));
                herErrList.Add(new PointD(x, errHer));
            }

            InterpolationPointsCount = refList.Count;
            LinearMaxError = maxLinErr.ToString("F5");
            HermiteMaxError = maxHerErr.ToString("F5");

            double padding = 40.0;
            double plotWidth = canvasWidth - 2 * padding;
            double plotHeight = canvasHeight - 2 * padding;

            double xMin = xStart;
            double xMax = xEnd;

            double yMin;
            double yMax;

            if (showErrorMode)
            {
                yMin = 0.0;
                double maxErr = Math.Max(maxLinErr, maxHerErr);
                if (maxErr < 1e-6) maxErr = 0.1;
                yMax = maxErr * 1.15;
            }
            else
            {
                yMin = -1.25;
                yMax = 1.25;
            }

            double toScreenX(double x)
            {
                return padding + (x - xMin) / (xMax - xMin) * plotWidth;
            }

            double toScreenY(double y)
            {
                return canvasHeight - padding - (y - yMin) / (yMax - yMin) * plotHeight;
            }

            AxisYPosition = toScreenY(0);
            AxisXPosition = toScreenX(0);

            PointCollection newRef = new PointCollection();
            PointCollection newLin = new PointCollection();
            PointCollection newHer = new PointCollection();
            PointCollection newLinErr = new PointCollection();
            PointCollection newHerErr = new PointCollection();
            ObservableCollection<Point> newNodes = new ObservableCollection<Point>();

            if (!showErrorMode)
            {
                if (showPoints)
                {
                    for (int i = 0; i < nodes.Count; i++)
                    {
                        newNodes.Add(new Point(toScreenX(nodes[i].X), toScreenY(nodes[i].Y)));
                    }
                }

                for (int i = 0; i < refList.Count; i++)
                {
                    if (showReference) newRef.Add(new Point(toScreenX(refList[i].X), toScreenY(refList[i].Y)));
                    if (showLinear) newLin.Add(new Point(toScreenX(linList[i].X), toScreenY(linList[i].Y)));
                    if (showHermite) newHer.Add(new Point(toScreenX(herList[i].X), toScreenY(herList[i].Y)));
                }
            }
            else
            {
                for (int i = 0; i < refList.Count; i++)
                {
                    if (showLinear) newLinErr.Add(new Point(toScreenX(linErrList[i].X), toScreenY(linErrList[i].Y)));
                    if (showHermite) newHerErr.Add(new Point(toScreenX(herErrList[i].X), toScreenY(herErrList[i].Y)));
                }
            }

            ReferencePoints = newRef;
            LinearPoints = newLin;
            HermitePoints = newHer;
            LinearErrorPoints = newLinErr;
            HermiteErrorPoints = newHerErr;
            ScreenNodes = newNodes;
        }

        private void ClearCurves()
        {
            ReferencePoints = new PointCollection();
            LinearPoints = new PointCollection();
            HermitePoints = new PointCollection();
            LinearErrorPoints = new PointCollection();
            HermiteErrorPoints = new PointCollection();
            ScreenNodes = new ObservableCollection<Point>();
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
