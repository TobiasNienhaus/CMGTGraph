namespace CMGTGraph.Calculators
{
    /// <summary>
    /// An interface for a calculator that can be used to calculate the distance between
    /// two objects of type <see cref="T"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICalculator<in T>
    {
        // Due to C#s nature, this is kind of OOP forced onto it
        // It has no right to be an object
        
        /// <summary>
        /// Calculate the square distance between the two provided elements.
        /// </summary>
        float SqrDistance(T a, T b);

        /// <summary>
        /// Calculate the distance between the two provided points
        /// </summary>
        float Distance(T a, T b);
    }
}