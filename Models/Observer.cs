using N_Body_Simulation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace N_Body_Simulation.Models
{
    public interface IObserver
    {
        public void Update(ISubject subject);
    }

    internal class SimulationObserver : IObserver
    {
        public MainViewModel viewModel;
        public SimulationObserver(MainViewModel viewModel)
        {
            this.viewModel = viewModel;
        }
        public void Update(ISubject subject)
        {
            List<BodyInfo> state = (subject as Simulation).GetState();
            viewModel.BodyInfos = state;
        }
    }
}
