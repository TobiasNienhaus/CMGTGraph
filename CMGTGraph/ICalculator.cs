namespace CMGTGraph
{
    public interface ICalculator<T>
    {
        bool Equals(T a, T b);

        float Distance(T a, T b);

        float Length(T t);

        T Subtract(T a, T b);
        
        T Add(T a, T b);
    }
}