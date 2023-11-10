using N_Body_Simulation.Models;
using N_Body_Simulation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace N_Body_Simulation
{
    /// <summary>
    /// Logika interakcji dla klasy AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        private AddViewModel viewModel;

        public AddWindow()
        {
            viewModel = new AddViewModel();
            InitializeComponent();
            DataContext = ViewModel;
            
        }

        internal AddViewModel ViewModel { get => viewModel; set => viewModel = value; }
    }
}
