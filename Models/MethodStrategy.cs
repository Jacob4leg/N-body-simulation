using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace N_Body_Simulation.Models
{
    public abstract class MethodStrategy
    {
        private readonly double gravity_constant = 1;
        protected readonly double time_step = 1e-3;
        
        private double Norm(Point p1, Point p2)
        {
            return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }
        protected List<Point> F(List<Point> positions, List<double> masses) // right side of the equation
        {
            List<Point> res = new List<Point>();

            for (int i = 0; i < positions.Count; i++)
            {
                Point acc = new Point(0, 0);
                for (int j = 0; j < positions.Count; j++)
                {
                    if (i == j)
                        continue;
                    double norm_cube = Norm(positions[i], positions[j]);
                    acc.X += masses[j] * (positions[j].X - positions[i].X) / norm_cube;
                    acc.Y += masses[j] * (positions[j].Y - positions[i].Y) / norm_cube;
                }
                acc.X *= gravity_constant;
                acc.Y *= gravity_constant;
                res.Add(acc);
            }
            return res;
        }
        public abstract (List<Point>, List<Point>) NextStep(List<Body> bodies);
    }

    public class EulerMethodStrategy : MethodStrategy
    {
        public override (List<Point>, List<Point>) NextStep(List<Body> bodies)
        {
            List<Point> positions = bodies.Select(x => x.Position).ToList();
            List<Point> velocities = bodies.Select(x => x.Velocity).ToList();
            List<double> masses = bodies.Select(x => x.Mass).ToList();

            var velocity_differential = F(positions, masses).Select
                                (x => new Point(x.X * time_step, x.Y * time_step)).ToList();

            var position_differential = velocities.Select
                                (x => new Point(x.X * time_step, x.Y * time_step)).ToList();

            return (position_differential, velocity_differential);
        }
    }

    public class VerletMethodStrategy : MethodStrategy
    {
        public override (List<Point>, List<Point>) NextStep(List<Body> bodies)
        {
            List<Point> positions = bodies.Select(x => x.Position).ToList();
            List<Point> velocities = bodies.Select(x => x.Velocity).ToList();
            List<double> masses = bodies.Select(x => x.Mass).ToList();

            var velocity_differential = F(positions, masses).Select
                                (x => new Point(x.X * time_step, x.Y * time_step)).ToList();

            var position_differential = new List<Point>();
            for(int i = 0; i < bodies.Count; i++)
            {
                var position = new Point
                    (velocities[i].X * time_step + velocity_differential[i].X * time_step / 2,
                    velocities[i].Y * time_step + velocity_differential[i].Y * time_step / 2);
                position_differential.Add(position);
            }

            return (position_differential, velocity_differential);
        }
    }
}
