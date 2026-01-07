using System;
using System.Diagnostics;
using System.Threading.Tasks;
using GameLauncher.Models;

namespace GameLauncher.Services
{
    public class LauncherService
    {
        public event Action<GameModel, TimeSpan> GameExited;

        public Task LaunchGame(GameModel game)
        {
            if (string.IsNullOrEmpty(game.ExecutablePath) || !System.IO.File.Exists(game.ExecutablePath))
                throw new Exception("Executable path is invalid or file does not exist.");

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = game.ExecutablePath,
                WorkingDirectory = System.IO.Path.GetDirectoryName(game.ExecutablePath),
                UseShellExecute = true
            };

            game.LastPlayed = DateTime.Now;
            Stopwatch stopwatch = new Stopwatch();
            
            try
            {
                Process? process = Process.Start(startInfo);
                if (process == null) return Task.CompletedTask;

                stopwatch.Start();
                
                // Track exit in background
                _ = Task.Run(async () =>
                {
                    await process.WaitForExitAsync();
                    stopwatch.Stop();
                    
                    TimeSpan sessionTime = stopwatch.Elapsed;
                    GameExited?.Invoke(game, sessionTime);
                });

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to start game: {ex.Message}");
            }
        }
    }
}
