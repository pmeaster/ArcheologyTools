namespace FractalSource.Energy
{
    public class Quantum
    {
        public static Quantum One { get; } = new();

        private Quantum() { }

        public int Value { get; } = 1;
    }
}