using Newtonsoft.Json;
using System;
using System.IO;

namespace GraphSplit
{
    public static class GraphSettings
    {
        private static int vertexRadius = 20;
        private static int vertexBorder = 4;
        private static int edgeLineSize = 3;

        private static string login = "";
        private static string password = "";

        static string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Settings", "settings.json");

        public static event EventHandler SettingsChange;

        public static int VertexRadius => vertexRadius;
        public static int VertexBorder => vertexBorder;
        public static int EdgeLineSize => edgeLineSize;
        public static string Login => login;
        public static string Password => password;

        public static void LoadSettings()
        {
            try
            {
                string json = File.ReadAllText(filePath);
                var settings = JsonConvert.DeserializeObject<dynamic>(json);

                ChangeVertexRadius((int)settings.VertexRadius);
                ChangeVertexBorder((int)settings.VertexStroke);
                ChangeEdgeLineSize((int)settings.EdgeLineSize);
                login = (string)settings.Login;
                password = (string)settings.Password;
            }
            catch 
            {
                return;
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
                    EdgeLineSize = edgeLineSize,
                    Login = login,
                    Password = password
                };

                var jsonSettings = JsonConvert.SerializeObject(settings, Formatting.Indented);
                // Создаем папку, если ее нет
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                File.WriteAllText(filePath, jsonSettings);
            }
            catch 
            {
                return;
            }
        }

        public static void ChangeLogin(string _login)
        {
            login = _login;
        }

        public static void ChangePassword(string _password)
        {
            password = _password;
        }

        public static void ChangeVertexRadius(int newValue)
        {
            vertexRadius = Math.Clamp(newValue, 5, 60);
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
