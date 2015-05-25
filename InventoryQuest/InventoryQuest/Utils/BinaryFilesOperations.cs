using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace InventoryQuest.Utils
{
    public class BinaryFilesOperations
    {
        public static T Load<T>(string file)
        {
            try
            {
                using (var fs = new FileStream(file, FileMode.Open))
                {
                    var w = new BinaryFormatter();
                    return (T) w.Deserialize(fs);
                }
            }
            catch
            {
                return default(T);
            }
        }

        public static void Save<T>(T Instance, string file)
        {
            using (var fs = new FileStream(file, FileMode.Create))
            {
                var w = new BinaryFormatter();
                w.Serialize(fs, Instance);
            }
        }
    }
}