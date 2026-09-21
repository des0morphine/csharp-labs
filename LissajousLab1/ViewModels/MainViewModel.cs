using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using LissajousLab1.Commands;
using LissajousLab1.Models;

namespace LissajousLab1.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private LissajousCalculator calc = new LissajousCalculator();

        private double ax = 100;
        private double ay = 100;
        private double fx = 1;
        private double fy = 2;
        private double phaseX = 90;
        private double phaseY = 0;
        private double timeStep = 0.002;
        private double totalTime = 2.0;

        private double canvasWidth = 500;
        private double canvasHeight = 500;

        private PointCollection curvePoints = new PointCollection();
        private string errorMessage = "";

        public ICommand PlotCommand { get; }
        public ICommand SetPresetCommand { get; }

        public MainViewModel()
        {
            PlotCommand = new RelayCommand(p => UpdatePlot());
            SetPresetCommand = new RelayCommand(p =>
            {
                string? str = p as string;
                if (str != null)
                {
                    ApplyPreset(str);
                }
            });

            UpdatePlot();
        }

        public double Ax
        {
            get { return ax; }
            set
            {
                ax = value;
                OnPropertyChanged("Ax");
                UpdatePlot();
            }
        }

        public double Ay
        {
            get { return ay; }
            set
            {
                ay = value;
                OnPropertyChanged("Ay");
                UpdatePlot();
            }
        }

        public double Fx
        {
            get { return fx; }
            set
            {
                fx = value;
                OnPropertyChanged("Fx");
                UpdatePlot();
            }
        }

        public double Fy
        {
            get { return fy; }
            set
            {
                fy = value;
                OnPropertyChanged("Fy");
                UpdatePlot();
            }
        }

        public double PhaseX
        {
            get { return phaseX; }
            set
            {
                phaseX = value;
                OnPropertyChanged("PhaseX");
                UpdatePlot();
            }
        }

        public double PhaseY
        {
            get { return phaseY; }
            set
            {
                phaseY = value;
                OnPropertyChanged("PhaseY");
                UpdatePlot();
            }
        }

        public double TimeStep
        {
            get { return timeStep; }
            set
            {
                timeStep = value;
                OnPropertyChanged("TimeStep");
                OnPropertyChanged("PointsCount");
                UpdatePlot();
            }
        }

        public double TotalTime
        {
            get { return totalTime; }
            set
            {
                totalTime = value;
                OnPropertyChanged("TotalTime");
                OnPropertyChanged("PointsCount");
                UpdatePlot();
            }
        }

        public int PointsCount
        {
            get
            {
                if (timeStep > 0 && totalTime > 0)
                {
                    return (int)(totalTime / timeStep);
                }
                return 0;
            }
        }

        public double CanvasWidth
        {
            get { return canvasWidth; }
            set
            {
                canvasWidth = value;
                OnPropertyChanged("CanvasWidth");
                OnPropertyChanged("CenterX");
                UpdatePlot();
            }
        }

        public double CanvasHeight
        {
            get { return canvasHeight; }
            set
            {
                canvasHeight = value;
                OnPropertyChanged("CanvasHeight");
                OnPropertyChanged("CenterY");
                UpdatePlot();
            }
        }

        public double CenterX
        {
            get { return canvasWidth / 2.0; }
        }

        public double CenterY
        {
            get { return canvasHeight / 2.0; }
        }

        public PointCollection CurvePoints
        {
            get { return curvePoints; }
            set
            {
                curvePoints = value;
                OnPropertyChanged("CurvePoints");
            }
        }

        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
                OnPropertyChanged("ErrorMessage");
            }
        }

        public void ApplyPreset(string name)
        {
            if (name == "1:1_circle")
            {
                ax = 100;
                ay = 100;
                fx = 1;
                fy = 1;
                phaseX = 0;
                phaseY = 90;
            }
            else if (name == "1:1_line")
            {
                ax = 100;
                ay = 100;
                fx = 1;
                fy = 1;
                phaseX = 0;
                phaseY = 0;
            }
            else if (name == "1:2")
            {
                ax = 100;
                ay = 100;
                fx = 1;
                fy = 2;
                phaseX = 90;
                phaseY = 0;
            }
            else if (name == "2:3")
            {
                ax = 100;
                ay = 100;
                fx = 2;
                fy = 3;
                phaseX = 90;
                phaseY = 0;
            }
            else if (name == "3:4")
            {
                ax = 100;
                ay = 100;
                fx = 3;
                fy = 4;
                phaseX = 90;
                phaseY = 0;
            }

            OnPropertyChanged("Ax");
            OnPropertyChanged("Ay");
            OnPropertyChanged("Fx");
            OnPropertyChanged("Fy");
            OnPropertyChanged("PhaseX");
            OnPropertyChanged("PhaseY");
            UpdatePlot();
        }

        public void UpdatePlot()
        {
            if (canvasWidth <= 0 || canvasHeight <= 0)
            {
                return;
            }

            try
            {
                List<Point2D> spisok = calc.Calculate(ax, ay, fx, fy, phaseX, phaseY, totalTime, timeStep);
                ErrorMessage = "";

                double otstup = 30;
                double w = canvasWidth - 2 * otstup;
                double h = canvasHeight - 2 * otstup;
                if (w < 10) w = 10;
                if (h < 10) h = 10;

                double maxA = ax;
                if (ay > maxA)
                {
                    maxA = ay;
                }
                if (maxA <= 0)
                {
                    maxA = 1;
                }

                double kx = w / (2 * maxA);
                double ky = h / (2 * maxA);
                double mashtab = kx;
                if (ky < mashtab)
                {
                    mashtab = ky;
                }

                double cx = CenterX;
                double cy = CenterY;

                PointCollection pts = new PointCollection();
                for (int i = 0; i < spisok.Count; i++)
                {
                    double px = cx + spisok[i].X * mashtab;
                    double py = cy - spisok[i].Y * mashtab;
                    pts.Add(new Point(px, py));
                }

                CurvePoints = pts;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                CurvePoints = new PointCollection();
            }
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
