using System.Data;

namespace Path_Generalizer
{
    class Generalizer_Config : IDisposable
    {
        public Dictionary<string, string> Data = [];
        public bool DataExists = false;
        public static readonly string Config_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.txt");
        public event EventHandler<string>? Warning;

        public Generalizer_Config()
        {
            AppDomain.CurrentDomain.ProcessExit += (s, e) => Dispose();
            
            if (File.Exists(Config_path))
                try
                {
                    Load();
                    DataExists = true;
                }
                catch (Exception ex)
                {
                    Warning?.Invoke(this, ex.Message);
                    Data.Clear();
                }
        }
        public void Dispose()
        {
            Save();
            Data.Clear();
        }

        //File operations
        public void Save()
        {
            using StreamWriter sw = new(Config_path);
            foreach (var kvp in Data)
                sw.WriteLine($"{kvp.Key}|||{kvp.Value}");
        }
        public void Load()
        {
            using StreamReader sr = new(Config_path);
            while (!sr.EndOfStream)
            {
                string? line = sr.ReadLine();
                if (string.IsNullOrEmpty(line))
                    continue;
                string[] data = line.Split("|||");
                if (data.Length == 2)
                    Data[data[0]] = data[1];
            }
        }

        //Get set operations
        public void Add(string Source, string Target)
        {
            Source = Source.Trim().Replace("\"", "");
            Target = Target.Trim().Replace("\"", "");
            if (string.IsNullOrEmpty(Source) || string.IsNullOrEmpty(Target))
                throw new ArgumentException("Source and Target cannot be null or empty");
            if (!Path.Exists(Source) || !Path.Exists(Target))
                throw new ArgumentException("Source and Target must be valid paths");
            if (!File.Exists(Source))
                throw new ArgumentException("Source must be a file");
            Data[Source] = Target;
        }
        public void Remove(string Source)
        {
            Source = Source.Trim().Replace("\"", "");
            if (string.IsNullOrEmpty(Source))
                throw new ArgumentException("Source cannot be null or empty");
            Data.Remove(Source);
        }
        public void Remove(int index)
        {
            if (index < 0 || index >= Data.Count)
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range");
            var key = Data.Keys.ElementAt(index);
            Data.Remove(key);
        }
        public string[] GetAllData()
        {
            return [.. Data.Select(kvp => $"{kvp.Key}|||{kvp.Value}")];
        }
        public string GetData(int index)
        {
            if (index < 0 || index >= Data.Count)
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range");
            var kvp = Data.ElementAt(index);
            return $"{kvp.Key}|||{kvp.Value}";
        }
    }
}
