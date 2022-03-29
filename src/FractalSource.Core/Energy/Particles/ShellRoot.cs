using System.Collections.Generic;

namespace FractalSource.Energy.Particles
{
    public class ShellRoot
    {
        public ShellRoot(IEnumerable<Quantum> energy, ShellRoot parent)
        {
            Energy = energy;
            Parent = parent;
            ShellNumber = (Parent?.ShellNumber ?? 0) + 1;
            SubShellCount = ShellNumber;
            Capacity = ShellNumber + 1;
            HigherShellNumber = Capacity;
            LowerShellNumber = ShellNumber - 1;
        }

        public int Capacity { get; }

        public IEnumerable<Quantum> Energy { get; }

        public int ShellNumber { get; }

        public ShellRoot Parent { get; }

        public int HigherShellNumber { get; }

        public int LowerShellNumber { get; }

        public int SubShellCount { get; }

        public void Collapse()
        {
            return;
        }

        public void Expand()
        {
            return;
        }
    }


    public class ShellQuantum : ShellQuantumPair
    {
        public ShellQuantum()
        {
            Value = Quantum.One;
        }

        public Quantum Value { get; }

        public Information Information { get; }
    }

    public class ShellQuantumPair
    {
        public ShellQuantum Inner { get; } // min orbit

        public ShellQuantum Outer { get; } // max orbit
    }

    public class Observer
    {

    }
}