using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace N_Body_Simulation.Models
{
    internal class MethodIterator : IEnumerable<List<Point>>
    {
        List<Body> bodies;
        ODESolver solver;

        public List<Body> Bodies { get { return bodies; } }

        public MethodIterator(List<Body> bodies)
        {
            this.bodies = bodies;
            this.solver = new ODESolver();
        }

        public void ChangeStrategy(MethodStrategy method)
        {
            solver.SetStrategy(method);
        }

        public void AddBody(Body b)
        {
            bodies.Add(b);
        }

        public void RemoveBody(string name)
        {
            foreach (Body b in bodies)
            {
                if(b.Name == name)
                {
                    bodies.Remove(b);
                    break;
                }
            }
        }


        public List<string> getBodyNames()
        {
            return bodies.Select(b => b.Name).ToList();
        }

        public IEnumerator<List<Point>> GetEnumerator()
        {
            while (true) 
            {
                var solverRes = solver.NextStep(bodies);
                var positionStep = solverRes.Item1;
                var velocityStep = solverRes.Item2;

                for (int i = 0; i < bodies.Count; i++)
                {
                    bodies[i].UpdatePosition(positionStep[i]);
                    bodies[i].UpdateVelocity(velocityStep[i]);
                }
                List<Point> resPosition = bodies.Select(x => x.Position).ToList();

                yield return resPosition;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
