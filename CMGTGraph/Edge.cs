
namespace CMGTGraph
{
    public class Edge<T>
    {
        // TODO make generic

        private T a;
        public T VertexA => a;

        private T b;
        public T VertexB => b;

        public Edge(T a, T b)
        {
            this.a = a;
            this.b = b;
        }
    }
}