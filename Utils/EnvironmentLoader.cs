using System;
using System.IO;
using System.Collections.Generic;

namespace ai_research_app.Utils
{
    public static class EnvironmentLoader
    {
        public static void Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("Warning: .env file not found.");
                return;
            }

            foreach (var line in File.ReadLines(filePath))
            {
                var parts = line.Split('=', 2, StringSplitOptions.TrimEntries);
                if (parts.Length == 2)
                {
                    Environment.SetEnvironmentVariable(parts[0], parts[1]);
                }
            }
        }

        public static string? Get(string key)
        {
            return Environment.GetEnvironmentVariable(key);
        }
    }
}