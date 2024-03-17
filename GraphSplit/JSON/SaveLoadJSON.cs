using GraphSplit.GraphElements;
using System.Text.Json;

namespace GraphSplit.JSON
{
    public static class SaveLoadJSON
    {
        public static void SaveToJSON(string filename, List<Vertex> vertices)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var verticesToSave = vertices.Select(v => new
            {
                Location = new { v.Location.X, v.Location.Y },
                v.Index,
                AdjacentEdgesRender = v.AdjacentEdgesRender.Select(e => new {
                    StartVertexIndex = vertices.IndexOf(e.Vertex1),
                    EndVertexIndex = vertices.IndexOf(e.Vertex2)
                }).ToList()
            }).ToList();

            string jsonString = JsonSerializer.Serialize(verticesToSave, options);
            File.WriteAllText(filename, jsonString);
        }

        public static List<Vertex> LoadFromJSON(string filename)
        {
            List<Vertex> vertices  = new ();
            string jsonString = File.ReadAllText(filename);
            var verticesData = JsonSerializer.Deserialize<List<VertexData>>(jsonString);

            foreach (var vertexData in verticesData)
            {
                var vertex = new Vertex(new Point(vertexData.Location.X, vertexData.Location.Y), vertexData.Index);
                vertices.Add(vertex);
            }

            foreach (var vertexData in verticesData)
            {
                var vertex = vertices[vertexData.Index];

                foreach (var edgeData in vertexData.AdjacentEdgesRender)
                {
                    var startVertex = vertices[edgeData.StartVertexIndex];
                    var endVertex = vertices[edgeData.EndVertexIndex];
                    var edge = new Edge(startVertex, endVertex);
                }
            }

            return vertices;
        }
    }
}
