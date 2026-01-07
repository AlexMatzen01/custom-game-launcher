using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using GameLauncher.Models;

namespace GameLauncher.Services
{
    public class LibraryService
    {
        private static readonly string AppDataPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "GameLauncher"
        );
        private static readonly string LibraryFilePath = Path.Combine(AppDataPath, "library.json");
        public static readonly string IconsFolderPath = Path.Combine(AppDataPath, "Icons");

        public LibraryService()
        {
            if (!Directory.Exists(AppDataPath)) Directory.CreateDirectory(AppDataPath);
            if (!Directory.Exists(IconsFolderPath)) Directory.CreateDirectory(IconsFolderPath);
        }

        public List<GameModel> LoadLibrary()
        {
            if (!File.Exists(LibraryFilePath))
                return new List<GameModel>();

            try
            {
                string json = File.ReadAllText(LibraryFilePath);
                return JsonSerializer.Deserialize<List<GameModel>>(json) ?? new List<GameModel>();
            }
            catch
            {
                return new List<GameModel>();
            }
        }

        public void SaveLibrary(List<GameModel> games)
        {
            try
            {
                string json = JsonSerializer.Serialize(games, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(LibraryFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to save library: {ex.Message}");
            }
        }
    }
}
