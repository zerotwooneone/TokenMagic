using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace WebApplication2.Jwt
{
    public class AppConfigService
    {
        public static readonly string AssemblyName = typeof(ApplicationBuilderExtensions).Assembly.GetName().Name;
        public static readonly string DirectoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            AssemblyName,
            "Jwt");
        public static readonly string FilePath = Path.Combine(DirectoryPath, "app.json");
        public AppConfig Get()
        {
            var fileInfo = new FileInfo(FilePath);

            if (fileInfo.Exists)
            {
                const int maxCharLength=10000;
                string json;
                try
                {
                    using (var stream = fileInfo.OpenRead())
                    using (var reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        char[] buffer = new char[maxCharLength];
                        int n = reader.ReadBlock(buffer, 0, buffer.Length);

                        char[] result = new char[n];

                        Array.Copy(buffer, result, n);
                        json = new string(result);
                    }
                    var appConfig = JsonConvert.DeserializeObject<AppConfig>(json);
                    return appConfig;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            return null;
        }

        public void Set(AppConfig appConfig)
        {
            var j = JsonConvert.SerializeObject(appConfig, Formatting.Indented);

            Directory.CreateDirectory(DirectoryPath); //creates the directory if it does not exist, does not complain if it already exists

            var fileInfo = new FileInfo(FilePath);

            using (var fileStream = fileInfo.OpenWrite())
            using (var writer = new StreamWriter(fileStream))
            {
                writer.Write(j);
                writer.Flush();
            }
        }
    }
}