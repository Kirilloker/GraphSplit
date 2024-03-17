using Newtonsoft.Json;

namespace GraphSplit
{
    public static class GraphSettings
    {
        private static int vertexRadius = 20;
        private static int vertexBorder = 4;
        private static int edgeLineSize = 3;

        const string filePath = "settings.json";

        public static event EventHandler SettingsChange;

        public static int VertexRadius
        {
            get { return vertexRadius; }
        }

        public static int VertexBorder
        {
            get { return vertexBorder; }
        }

        public static int EdgeLineSize
        {
            get { return edgeLineSize; }
        }

        public static void LoadSettings()
        {
            try
            {
                string json = File.ReadAllText(filePath);
                var settings = JsonConvert.DeserializeObject<dynamic>(json);

                ChangeVertexRadius((int)settings.VertexRadius);
                ChangeVertexBorder((int)settings.VertexStroke);
                ChangeEdgeLineSize((int)settings.EdgeLineSize);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading settings: {ex.Message}");
            }
        }

        public static void SaveSettings()
        {
            try
            {
                var settings = new
                {
                    VertexRadius = vertexRadius,
                    VertexStroke = vertexBorder,
                    EdgeLineSize = edgeLineSize
                };

                var jsonSettings = JsonConvert.SerializeObject(settings, Formatting.Indented);
                File.WriteAllText(filePath, jsonSettings);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving settings: {ex.Message}");
            }
        }

        public static void ChangeVertexRadius(int newValue)
        {
            vertexRadius = Math.Clamp(newValue, 10, 60);
            SettingsChange?.Invoke(null, EventArgs.Empty);
        }

        public static void ChangeVertexBorder(int newValue)
        {
            vertexBorder = Math.Clamp(newValue, 3, 10);
            SettingsChange?.Invoke(null, EventArgs.Empty);
        }

        public static void ChangeEdgeLineSize(int newValue)
        {
            edgeLineSize = Math.Clamp(newValue, 2, 10);
            SettingsChange?.Invoke(null, EventArgs.Empty);
        }
    }

}
