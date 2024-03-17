namespace GraphSplit.JSON
{
    public class VertexData
    {
        public struct LocationStruct
        {
            public int X { get; set; }
            public int Y { get; set; }
        }

        public LocationStruct Location { get; set; }
        public int Index { get; set; }
        public List<EdgeData> AdjacentEdgesRender { get; set; }
    }

    public class EdgeData
    {
        public int StartVertexIndex { get; set; }
        public int EndVertexIndex { get; set; }
    }
}
