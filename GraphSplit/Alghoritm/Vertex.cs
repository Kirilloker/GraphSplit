namespace MultiagentAlgorithm
{
    public class Vertex
   {
      public int ID { get; }
      public int Weight { get; }
      public int Color { get; set; }
      public double LocalCost { get; set; }

      private Dictionary<int, int> _connectedEdges;

      public Dictionary<int, int> ConnectedEdges
      {
         get
         {
            return _connectedEdges ?? (_connectedEdges = new Dictionary<int, int>());
         }
         set { _connectedEdges = value; }
      }

      public Vertex(int id, int weight)
      {
         ID = id;
         Weight = weight;
      }
   }
}