namespace CMGTGraph.Calculators
{
    public interface ICalculator<in T>
    {
        /// <summary>
        /// Calculate the square distance between the provided elements.
        /// </summary>
        float SqrDistance(T a, T b);

        float Distance(T a, T b);
    }
}