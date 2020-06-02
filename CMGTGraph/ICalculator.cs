namespace CMGTGraph
{
    public interface ICalculator<T>
    {
        public bool Equals(T a, T b);

        public float Distance(T a, T b);

        public float Length(T t);

        public T Subtract(T a, T b);
        
        public T Add(T a, T b);
    }
}