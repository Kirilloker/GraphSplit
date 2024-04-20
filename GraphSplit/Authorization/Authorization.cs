using System.Security.Cryptography;
using System.Text;

namespace GraphSplit.Authorization
{
    public static class Authorization
    {
        private static bool isAuthorization;
        static string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Authorization", "AuthorizationData.txt");
        private static Dictionary<string, string> AuthorizationData = new();

        public static bool Login(string login, string password, bool isHash = false)
        {
            if (login == null || password == null) return false;

            UpdateAuthorizationData();

            if (AuthorizationData.ContainsKey(login)) 
            {
                if (isHash == false)
                    password = ComputeSha256Hash(password);
            
                if (AuthorizationData[login] == password) 
                {
                    isAuthorization = true;
                    return true; 
                }
            }
            
            return false;
        }

        public static bool Registration(string login, string password, bool isHash = false) 
        {
            if (login == null || password == null) return false;

            UpdateAuthorizationData();

            if (AuthorizationData.ContainsKey(login)) return false;

            if (isHash == false)
                password = ComputeSha256Hash(password);

            AuthorizationData.Add(login, password);
            SaveAuthorizationData();
            isAuthorization = true;

            return true;
        }


        private static void UpdateAuthorizationData() 
        {
            AuthorizationData.Clear();

            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length == 2)
                    {
                        AuthorizationData.Add(parts[0].Trim(), parts[1].Trim());
                    }
                }
            }
        }

        private static void SaveAuthorizationData() 
        {
            using (StreamWriter file = new StreamWriter(filePath))
            {
                foreach (var pair in AuthorizationData)
                {
                    file.WriteLine($"{pair.Key},{pair.Value}");
                }
            }
        }

        public static void LogOut() { isAuthorization = false; }

        public static bool IsAuthorization { get { return isAuthorization; } }


        public static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
