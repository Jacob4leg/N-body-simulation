using N_Body_Simulation.Commands;
using N_Body_Simulation.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace N_Body_Simulation.ViewModels
{
    internal class AddViewModel : INotifyPropertyChanged
    {
        private double _massVal;
        private double _positionXVal;
        private double _positionYVal;
        private double _velocityXVal;
        private double _velocityYVal;
        public double MassVal
        {
            get { return _massVal; }
            set
            {
                _massVal = value;
                OnPropertyChanged();
            }
        }
        public double PositionXVal
        {
            get { return _positionXVal; }
            set
            {
                _positionXVal = value;
                OnPropertyChanged();
            }
        }
        public double PositionYVal
        {
            get { return _positionYVal; }
            set
            {
                _positionYVal = value;
                OnPropertyChanged();
            }
        }
        public double VelocityXVal
        {
            get { return _velocityXVal; }
            set
            {
                _velocityXVal = value;
                OnPropertyChanged();
            }
        }
        public double VelocityYVal
        {
            get { return _velocityYVal; }
            set
            {
                _velocityYVal = value;
                OnPropertyChanged();
            }
        }

        
        public ICommand AddCommand { get; set; }
        public ICommand AbortCommand { get; set; }

        public AddViewModel()
        {
            AddCommand = new RelayCommand(AddClick);
            AbortCommand = new RelayCommand(AbortClick);
        }

        public delegate void Added(AddViewModel addViewModel);
        public delegate void Off();
        public event Added BodyAdded;
        public event Off TurnedOff;
        
        protected virtual void OnBodyAdded(AddViewModel addViewModel)
        {
            BodyAdded?.Invoke(addViewModel);
        }
        protected virtual void OnTurnedOff()
        {
            TurnedOff?.Invoke();
        }

        private void AddClick(object obj)
        {
            OnBodyAdded(this);
            OnTurnedOff();
            Window win = (Window)obj;
            win.Close();
        }

        private void AbortClick(object obj)
        {
            OnTurnedOff();
            Window win = (Window)obj;
            win.Close();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
