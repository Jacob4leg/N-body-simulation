using N_Body_Simulation.Commands;
using N_Body_Simulation.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;

namespace N_Body_Simulation.ViewModels
{
    
    internal class MainViewModel : INotifyPropertyChanged
    {
        private int _index_for_naming_bodies = 0;
        private Simulation simulation;
        private SimulationObserver simulationObserver;
        public List<BodyInfo> bodyInfos = new List<BodyInfo>();
        private Canvas drawCanvas;
        Thread? runnerThread;
        public List<BodyInfo> BodyInfos
        {
            get { return bodyInfos.Select(x => new BodyInfo() 
                    { Name=x.Name, PositionX=Math.Round(x.PositionX,2), 
                        PositionY = Math.Round(x.PositionY, 2), Mass=x.Mass }).ToList(); }
            set 
            {
                bodyInfos = value;
                OnPropertyChanged();
            }
        }


        public ICommand SetEulerCommand { get; set; }
        public ICommand SetVerletCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand RemoveCommand { get; set; }
        public ICommand StartCommand { get; set; }
        public ICommand PauseCommand { get; set; }
        public ICommand ExitCommand { get; set; }

        private bool isStarted = false;
        private bool isAddWindowOpened = false;
        private bool isEuler = true;
        private bool isVerlet = false;


        public MainViewModel(Canvas canvas)
        {
            this.drawCanvas = canvas;

            simulation = Simulation.GetInstance();
            simulationObserver = new SimulationObserver(this);
            simulation.Attach(simulationObserver);

            SetEulerCommand = new RelayCommand(SetEuler_Click, IsEulerDisabled);
            SetVerletCommand = new RelayCommand(SetVerlet_Click, IsVerletDisabled);
            AddCommand = new RelayCommand(Add_Click, IsSimulationPaused);
            RemoveCommand = new RelayCommand(Remove_Click, IsSimulationPaused);
            StartCommand = new RelayCommand(Start_Click, IsSimulationPaused);
            PauseCommand = new RelayCommand(Pause_Click, IsSimulationRunning);
            ExitCommand = new RelayCommand(Exit_Click, IsSimulationPaused);
            
        }
        private bool IsSimulationRunning(object obj) => isStarted;
        private bool IsSimulationPaused(object obj) => !isStarted && !isAddWindowOpened;

        private bool IsEulerDisabled(object obj) => !isStarted && !isAddWindowOpened && !isEuler;
        private bool IsVerletDisabled(object obj) => !isStarted && !isAddWindowOpened && !isVerlet;

        private void SetEuler_Click(object obj)
        {
            simulation.ChangeStrategy(new EulerMethodStrategy());
            isEuler = true;
            isVerlet = false;
        }

        private void SetVerlet_Click(object obj)
        {
            simulation.ChangeStrategy(new VerletMethodStrategy());
            isEuler = false;
            isVerlet = true;
        }

        private void Add_Click(object obj)
        {
            isAddWindowOpened = true;
            var addWindow = new AddWindow();
            addWindow.ViewModel.BodyAdded += BodyAdded;
            addWindow.ViewModel.TurnedOff += AddWindowTurnedOff;
            addWindow.Show();
            
        }

        private void Remove_Click(object obj)
        {
            var name = obj as string;
            BodyRemoved(name);
        }

        private void Start_Click(object obj)
        {
            isStarted = true;
            DrawGrid();
            runnerThread = new Thread(new ThreadStart(RunSimulation));
            runnerThread.Start();
        }

        private void Pause_Click(object obj)
        {
            isStarted = false;
        }

        private void Exit_Click(object obj)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void BodyAdded(AddViewModel addViewModel)
        {
            string name = "b" + (++_index_for_naming_bodies).ToString();
            simulation.AddNewBody(addViewModel.MassVal, addViewModel.PositionXVal,
                                    addViewModel.PositionYVal, addViewModel.VelocityXVal,
                                    addViewModel.VelocityYVal, name);
            DrawPoint(addViewModel.PositionXVal, addViewModel.PositionYVal, addViewModel.MassVal);
            simulation.Notify();
        }

        private void AddWindowTurnedOff()
        {
            isAddWindowOpened = false;
        }

        private void BodyRemoved(string bodyName)
        {
            simulation.RemoveBody(bodyName);
            simulation.Notify();
        }

        private void DrawGrid()
        {
            var black = new SolidColorBrush();
            black.Color = Colors.Black;
            var gray = new SolidColorBrush();
            gray.Color = Colors.Gray;

            double a = drawCanvas.ActualWidth;
            double b = drawCanvas.ActualHeight;

            for(int i = 0; i < 19; i++)
            {

                double actualHeight = 
                    drawCanvas.ActualHeight * ((double) (20 * i - 80) / 200 + 1.0 / 2 );
                double actualWidth = 
                    drawCanvas.ActualWidth * ((double) (20 * i - 80) / 200 + 1.0 / 2);
                var horizontalLine = new Line()
                {
                    X1 = 0,
                    X2 = a,
                    Y1 = actualHeight,
                    Y2 = actualHeight,
                    Stroke = i == 4? black : gray,
                    StrokeThickness = i == 4? 3 : 1
                };
                var verticalLine = new Line()
                {
                    X1 = actualWidth,
                    X2 = actualWidth,
                    Y1 = 0,
                    Y2 = b,
                    Stroke = i == 4 ? black : gray,
                    StrokeThickness = i == 4 ? 3 : 1
                };

                drawCanvas.Children.Add(horizontalLine);
                drawCanvas.Children.Add(verticalLine);
            }
        }

        private void DrawPoint(double x, double y, double mass)
        {
            var blue = new SolidColorBrush();
            blue.Color = Colors.Blue;

            double a = drawCanvas.ActualWidth;
            double b = drawCanvas.ActualHeight;

            double actualX = drawCanvas.ActualWidth * (x / 200 + 1.0/2);
            double actualY = drawCanvas.ActualHeight * (y / 200 + 1.0/2);

            double diameter = 30 / Math.Sqrt(100) * Math.Sqrt(mass);

            Ellipse ellipse = new Ellipse()
            {
                Width = diameter,
                Height = diameter,
                Fill = blue
            };
            Canvas.SetLeft(ellipse, actualX - diameter/2);
            Canvas.SetBottom(ellipse, actualY - diameter/2);
            drawCanvas.Children.Add(ellipse);

        }

        private void DrawBodies()
        {
            foreach(var body in bodyInfos)
            {
                DrawPoint(body.PositionX, body.PositionY, body.Mass);
            }
        }

        private void RemoveBodiesFromCanvas()
        {
            for(int i = 0; i < drawCanvas.Children.Count; i++)
            {
                var child = drawCanvas.Children[i];
                if(child is Ellipse)
                    drawCanvas.Children.RemoveAt(i);
            }
        }

        public void RunSimulation()
        {
            while (isStarted)
            {
                Thread.Sleep(1);
                simulation.GetNextStepOfTheSimulation();
                simulation.Notify();
                System.Windows.Application.Current.
                    Dispatcher.Invoke((Action)(() => RemoveBodiesFromCanvas()));
                System.Windows.Application.Current.
                    Dispatcher.Invoke((Action)(() => DrawBodies()));
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
