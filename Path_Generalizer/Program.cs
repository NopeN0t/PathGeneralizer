using System.Diagnostics;

namespace Path_Generalizer
{
    internal class Program
    {
        /* The wall of idea
        This program will generalize the path for other program to run
        Source and target path will be defined in the config file
            Source is the path to a file that contains path that will be generalized
            Target is the path to a folder that other programs start to run

        Example : Touhou community reliant automatic patcher
        ***This program config file
            Source = D:/1-Games/Touhou/thcrap/config/games.js
            Target = D:/1-Games/Touhou/thcrap
        ***Actual config
            D:/1-Games/Touhou/thcrap/config/games.js|||D:/1-Games/Touhou/thcrap
        ***Actual config after generalized
            ../../../../thcrap/config/games.js|||../../../../thcrap

        This is data in games.js
            {
            "th06": "D:/1-Games/Touhou/th6/eosd/vpatch.exe",
            "th06_custom": "D:/1-Games/Touhou/th6/eosd/custom.exe"
            }

        1. Get all path form games.js with some sort of regrex
        2. Detect if the path is alaready generalized
        3. Generalize the path based on target path
        4. Replace the path in games.js with the generalized path

        The result should be
            {
            "th06": "../th6/eosd/vpatch.exe",
            "th06_custom": "../th6/eosd/custom.exe"
            }

        Next to implemented desktop.ini icon path generalization
        A function that will hunt for desktop.ini files in the current directory and subdirectories
        and replace the IconResource path with a relative path to the icon file

        */
        public static Generalizer_Core gc = new();
        public static Stopwatch sw = new();
        static void Main()
        {
            gc.Config.Warning += (sender, message) =>
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"[Generalizer] Warning: {message}");
                Console.ResetColor();
            };
            //Console Menu
            while (true)
            {
                string? input;
                Console.WriteLine("Main Menu");
                Console.WriteLine("1. Add");
                Console.WriteLine("2. Remove");
                Console.WriteLine("3. Show Data");
                Console.WriteLine("4. Generalize");
                Console.WriteLine("5. Self Generalize");
                Console.WriteLine("6. Generalize folder icons (desktop.ini)");
                Console.WriteLine("0. Exit");
                input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        Add();
                        break;
                    case "2":
                        Remove();
                        break;
                    case "3":
                        ShowData();
                        break;
                    case "4":
                        try
                        {
                            Console.WriteLine("Generalizing paths...");
                            sw.Start();
                            gc.Generalize();
                            sw.Stop();
                            Console.WriteLine($"Generalized successfully\ntime taken {sw.Elapsed}");
                            sw.Reset();
                        }
                        catch (Exception ex)
                        { Console.WriteLine($"Error: {ex.Message}"); }
                        break;
                    case "5":
                        try
                        {
                            string[] result = gc.Self_Generalize(true);
                            Console.WriteLine("Preview of generalized paths");
                            Console.WriteLine("Program Path : " + Generalizer_Core.Program_Path);
                            foreach (string s in result)
                                Console.WriteLine(s);
                            if (GetString("Commit (y/n) : ", "Invalid input, please type 'y' or 'n'") == "y")
                            {
                                sw.Start();
                                gc.Self_Generalize(false);
                                sw.Stop();
                                Console.WriteLine($"Generalized successfully\ntime taknen {sw.Elapsed}");
                                sw.Reset();
                            }
                        }
                        catch (Exception ex)
                        { Console.WriteLine($"Error: {ex.Message}"); }
                        break;
                    case "6":
                            Console.WriteLine("Input base directory (leave empty for current directory)");
                            string? baseDirectory;

                            while (true)
                            {
                                baseDirectory = Console.ReadLine()?.Trim().Replace("\"", "") ?? Directory.GetCurrentDirectory();
                                if (Directory.Exists(baseDirectory)) break;
                                Console.WriteLine("Invalid directory, please try again.");
                            }

                            Console.WriteLine("Generalizing desktop.ini files...");
                            sw.Start();
                            gc.Generalize_desktopini(baseDirectory);
                            sw.Stop();
                            Console.WriteLine($"Desktop.ini files generalized successfully\ntime taken {sw.Elapsed}");
                            sw.Reset();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid input");
                        break;
                }
            }
        }

        static void Remove()
        {
            ShowData();
            while (true)
            {
                Console.Write("Select index to remove (type -1 to cancel)");
                string? index = GetString("Index : ");
                if (index == "-1") return;

                try
                {
                    gc.Config.Remove(int.Parse(index));
                    return;
                }
                catch (Exception ex)
                { Console.WriteLine($"Error: {ex.Message}"); }
            }
        }
        static void Add()
        {
            while (true)
            {
                Console.WriteLine("Add new path (type -1 to cancel)");
                string Source = GetString("Source : ");
                if (Source == "-1") return;
                string? Target = GetString("Target : ");
                if (Target == "-1") return;

                try
                {
                    gc.Config.Add(Source, Target);
                    return;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error: {e.Message}");
                }
            }
        }
        static string GetString(string? Message = "Input : ", string? ErrorMessage = null)
        {
            while (true)
            {
                Console.Write(Message);

                string? input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine(ErrorMessage ?? "Invalid Input");
                    continue;
                }
                return input.Trim();
            }
        }
        static void ShowData()
        {
            string[] datas = gc.Config.GetAllData();
            for (int i = 0; i < datas.Length; i++)
            {
                string[] data = datas[i].Split("|||");
                Console.WriteLine($"{i}. Key : {data[0]}\n     Value : {data[1]}");
            }
        }
    }
}