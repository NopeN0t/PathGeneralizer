using System;
using System.Text.RegularExpressions;

namespace Path_Generalizer
{
    class Generalizer_Core
    {
        public static readonly string Program_Path = AppDomain.CurrentDomain.BaseDirectory;
        public Generalizer_Config Config = new();
        public static Regex PathLike = new("\"(?<key>[^\"]+)\"\\s*:\\s*\"(?<value>[^\"]+)\"");
        public event EventHandler<string>? Warning;

        public Generalizer_Core()
        {
            Config.Warning += (s, e) => Warning?.Invoke(this, e);
        }

        //Generalize usual config file with path-like values
        public void Generalize()
        {
            Parallel.ForEach(Config.Data, kvp =>
            {
                string[] lines = File.ReadAllLines(kvp.Key);
               
                for (int i = 0; i < lines.Length; i++)
                {
                    var match = PathLike.Match(lines[i]);

                    if (!match.Success) continue; // Skip lines that do not match the pattern

                    string key = match.Groups["key"].Value;
                    string path = match.Groups["value"].Value;

                    if (string.IsNullOrEmpty(path)) continue; // Skip empty values

                    bool looksLikePath =
                        Path.IsPathRooted(path) ||
                        path.StartsWith("..\\") ||
                        path.StartsWith("../");

                    if (!looksLikePath) continue; // Skip non-path values

                    path = path.Replace("\\", "/"); // Normalize path separators

                    if (Path.IsPathRooted(path))
                    {
                        lines[i] = PathLike.Replace(lines[i], $"\"{key}\": \"{Path.GetRelativePath(kvp.Value, path).Replace("\\", "/")}\"");
                    }
                }
                File.WriteAllLines(kvp.Key, lines);
            });
        }
        
        //Generalize desktop.ini files within a given directory
        public void Generalize_desktopini(string baseDirectory)
        {

            Parallel.ForEach(Directory.EnumerateFiles(baseDirectory, "desktop.ini", SearchOption.AllDirectories), iniPath =>
            {
                string iniFolder = Path.GetDirectoryName(iniPath) ?? throw new InvalidOperationException("Invalid ini path : " + iniPath);
                string[] lines = File.ReadAllLines(iniPath);
                for (int i = 0; i < lines.Length; i++)
                {
                    if (!lines[i].StartsWith("IconResource=", StringComparison.OrdinalIgnoreCase))
                        continue;
                    string value = lines[i].Substring("IconResource=".Length);
                    string[] parts = value.Split(',', 2);

                    string iconPath = parts[0];
                    string iconIndex = parts.Length > 1 ? "," + parts[1] : "";

                    if (!Path.IsPathRooted(iconPath)) continue; // Skip if not rooted

                    // Get correct relative path from folder containing the .ini
                    string relative = Path.GetRelativePath(iniFolder, iconPath)
                                        .Replace('\\', '/');

                    lines[i] = $"IconResource={relative}{iconIndex}";
                }
                var attributes = File.GetAttributes(iniPath);
                try
                {

                    File.SetAttributes(iniPath, attributes & ~FileAttributes.ReadOnly & ~FileAttributes.System & ~FileAttributes.Hidden);
                    File.WriteAllLines(iniPath, lines);
                }
                catch (Exception ex)
                {
                    Warning?.Invoke(this, $"Failed to edit {iniPath}: {ex.Message}");
                }
                finally
                {
                    File.SetAttributes(iniPath, attributes);
                }
            });
        }

        public string[] Self_Generalize(bool preview = false)
        {
            if (!Config.DataExists)
                throw new InvalidOperationException("Data does not exist. Please load data first.");

            List<string> results = [];
            var newData = new Dictionary<string, string>();
            
            foreach (var kvp in Config.Data)
            {
                // Create a new dictionary with generalized (relative) keys and values
                string relKey = Path.GetRelativePath(Program_Path, kvp.Key);
                string relValue = Path.GetRelativePath(Program_Path, kvp.Value);
                
                newData[relKey] = relValue;
                results.Add(relKey);
                results.Add(relValue);
            }
            
            if (!preview)
                Config.Data = newData;
            
            return [.. results];
        }
    }
}