using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace N_Body_Simulation.Models
{
    public class Body
    {
        private Point position;
        private Point velocity;
        private double mass;
        private string name;


        public Point Position { get { return position; } set { position = value; } }
        public Point Velocity { get { return velocity; } set { velocity = value; } }
        public double Mass { get { return mass; } set { mass = value; } }
        public string Name { get { return name; } set { name = value; } }

        
        public void UpdatePosition(Point positionStep)
        {
            this.position.X += positionStep.X;
            this.position.Y += positionStep.Y;
        }

        public void UpdateVelocity(Point acceleration)
        {
            velocity.X += acceleration.X;
            velocity.Y += acceleration.Y;
        }

        public Body ShallowCopy()
        {
            return (Body)this.MemberwiseClone();
        }

        public Body DeepCopy()
        {
            Body clone = (Body)MemberwiseClone();
            clone.position = new Point(position.X, position.Y);
            clone.velocity = new Point(velocity.X, velocity.Y);
            return clone;
        }
    }
}
