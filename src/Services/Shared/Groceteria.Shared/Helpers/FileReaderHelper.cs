using Newtonsoft.Json;

namespace Groceteria.Shared.Helpers
{
    public class FileReaderHelper<T> where T: class
    {
        public static List<T> SeederFileReader(string filename)
        {
            var data = File.ReadAllText($"./DataAccess/Seeders/{filename}.json");
            var jsonData = JsonConvert.DeserializeObject<List<T>>(data);
            return jsonData;
        }

        public static List<T> SeederFileReader(string filename, string path)
        {
            var data = File.ReadAllText($"{path}/{filename}.json");
            var jsonData = JsonConvert.DeserializeObject<List<T>>(data);
            return jsonData;
        }
    }
}
