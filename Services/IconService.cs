using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace GameLauncher.Services
{
    public class IconService
    {
        public string ExtractIcon(string executablePath, string gameId)
        {
            try
            {
                if (!File.Exists(executablePath)) return null;

                using (Icon icon = Icon.ExtractAssociatedIcon(executablePath))
                {
                    if (icon == null) return null;

                    string fileName = $"{gameId}.png";
                    string destinationPath = Path.Combine(LibraryService.IconsFolderPath, fileName);

                    // Convert Icon to Bitmap then save as PNG for better quality/transparency support
                    using (Bitmap bitmap = icon.ToBitmap())
                    {
                        bitmap.Save(destinationPath, ImageFormat.Png);
                    }

                    return destinationPath;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error extracting icon: {ex.Message}");
                return null;
            }
        }
    }
}
