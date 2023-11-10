using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace N_Body_Simulation.Models
{
    public class BodyInfo
    {
        public string Name { get; set; }
        public double PositionX { get; set; }
        public double PositionY { get; set; }
        public double Mass { get; set; }
    }
    public interface ISubject
    {
        void Attach(IObserver observer);
        void Detach(IObserver observer);
        void Notify();
    }

    public class Simulation : ISubject
    {
        private List<IObserver> observers = new List<IObserver>();
        private MethodIterator methodIterator;
        private IEnumerator<List<Point>> enumerator;
        private Body prototype = new Body()
        { Mass = 0, Name = "prototype", Position = new Point(0, 0), Velocity = new Point(0, 0) };

        
        private Simulation()
        {
            methodIterator = new MethodIterator(new List<Body>());
            enumerator = methodIterator.GetEnumerator();
        }
        
        private static Simulation? _instance;
        private static readonly object _instanceLock = new object();
        public static Simulation GetInstance()
        {
            if(_instance == null)
            {
                lock (_instanceLock)
                {
                    if(_instance == null)
                        _instance = new Simulation();
                }
            }
                
            return _instance;
        }

        public void ChangeStrategy(MethodStrategy method)
        {
            methodIterator.ChangeStrategy(method);
        }

        public void AddNewBody(double mass, double positionX, double positionY,
                                double velocityX, double velocityY, string name)
        {
            Body newBody = prototype.DeepCopy();
            newBody.Mass = mass;
            newBody.Position = new Point(positionX, positionY);
            newBody.Velocity = new Point(velocityX, velocityY);
            newBody.Name = name;
            
            methodIterator.AddBody(newBody);
        }

        public void RemoveBody(string name)
        {
            methodIterator.RemoveBody(name);
        }

        public List<string> getBodyNames()
        {
            return methodIterator.getBodyNames();
        }

        public List<Point> GetNextStepOfTheSimulation()
        {
            enumerator.MoveNext();
            return enumerator.Current;
        }

        public List<BodyInfo> GetState()
        {
            return methodIterator.Bodies.
                Select(x => new BodyInfo()
                { Name = x.Name, PositionX = x.Position.X, PositionY = x.Position.Y, Mass = x.Mass }).
                ToList();
        }

        public void Attach(IObserver observer)
        {
            observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (IObserver observer in observers)
                observer.Update(this);
        }

    }
}
