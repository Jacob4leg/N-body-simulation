using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace N_Body_Simulation.Models
{
    
    internal class ODESolver // calculates the next step of solving c(t)'' = F(c(t))
    {
        private MethodStrategy method;

        public ODESolver()
        {
            method = new EulerMethodStrategy();
        }

        public void SetStrategy(MethodStrategy method)
        {
            this.method = method;
        }

        public (List<Point>, List<Point>) NextStep(List<Body> bodies)
        {
            return method.NextStep(bodies);
        }
    }
}
