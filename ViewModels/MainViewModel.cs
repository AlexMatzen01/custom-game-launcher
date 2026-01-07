using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameLauncher.Models;
using GameLauncher.Services;
using Microsoft.Win32;

namespace GameLauncher.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly LibraryService _libraryService;
        private readonly IconService _iconService;
        private readonly LauncherService _launcherService;

        [ObservableProperty]
        private ObservableCollection<GameModel> _games;

        [ObservableProperty]
        private string _searchText;

        [ObservableProperty]
        private GameModel _selectedGame;

        public MainViewModel()
        {
            _libraryService = new LibraryService();
            _iconService = new IconService();
            _launcherService = new LauncherService();
            _launcherService.GameExited += OnGameExited;

            LoadGames();
        }

        private void LoadGames()
        {
            var loadedGames = _libraryService.LoadLibrary();
            Games = new ObservableCollection<GameModel>(loadedGames);
        }

        private void OnGameExited(GameModel game, TimeSpan sessionTime)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                game.TotalPlayTime += sessionTime;
                _libraryService.SaveLibrary(Games.ToList());
                // Force UI update if needed (though TotalPlayTime should probably be observable)
                // For simplicity, we can refresh the list or make GameModel an ObservableObject
            });
        }

        [RelayCommand]
        private async Task AddGame()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Executables (*.exe)|*.exe",
                Title = "Select Game Executable"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                string fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);

                var newGame = new GameModel
                {
                    Name = fileName,
                    ExecutablePath = filePath
                };

                string iconPath = _iconService.ExtractIcon(filePath, newGame.Id.ToString());
                newGame.IconPath = iconPath;

                Games.Add(newGame);
                _libraryService.SaveLibrary(Games.ToList());
            }
        }

        [RelayCommand]
        private async Task LaunchGame(GameModel game)
        {
            if (game == null) return;
            try
            {
                await _launcherService.LaunchGame(game);
                _libraryService.SaveLibrary(Games.ToList());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void RemoveGame(GameModel game)
        {
            if (game == null) return;
            if (MessageBox.Show($"Are you sure you want to remove {game.Name}?", "Confirm", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Games.Remove(game);
                _libraryService.SaveLibrary(Games.ToList());
            }
        }

        [RelayCommand]
        private void ToggleTheme()
        {
            var app = (App)Application.Current;
            bool isDark = (Color)app.Resources["BgColor"] == Color.FromRgb(0x12, 0x12, 0x12);
            
            if (isDark)
            {
                // Switch to Light
                app.Resources["BgColor"] = Color.FromRgb(0xF0, 0xF0, 0xF0);
                app.Resources["SidebarBgColor"] = Color.FromRgb(0xE0, 0xE0, 0xE0);
                app.Resources["CardBgColor"] = Color.FromRgb(0xFF, 0xFF, 0xFF);
                app.Resources["TextColor"] = Color.FromRgb(0x00, 0x00, 0x00);
                app.Resources["SecondaryTextColor"] = Color.FromRgb(0x55, 0x55, 0x55);
            }
            else
            {
                // Switch to Dark
                app.Resources["BgColor"] = Color.FromRgb(0x12, 0x12, 0x12);
                app.Resources["SidebarBgColor"] = Color.FromRgb(0x1E, 0x1E, 0x1E);
                app.Resources["CardBgColor"] = Color.FromRgb(0x25, 0x25, 0x25);
                app.Resources["TextColor"] = Color.FromRgb(0xFF, 0xFF, 0xFF);
                app.Resources["SecondaryTextColor"] = Color.FromRgb(0xAA, 0xAA, 0xAA);
            }

            // Update brushes
            app.Resources["BgBrush"] = new SolidColorBrush((Color)app.Resources["BgColor"]);
            app.Resources["SidebarBgBrush"] = new SolidColorBrush((Color)app.Resources["SidebarBgColor"]);
            app.Resources["CardBgBrush"] = new SolidColorBrush((Color)app.Resources["CardBgColor"]);
            app.Resources["TextBrush"] = new SolidColorBrush((Color)app.Resources["TextColor"]);
            app.Resources["SecondaryTextBrush"] = new SolidColorBrush((Color)app.Resources["SecondaryTextColor"]);
        }

        // Filtering logic (can be expanded)
        public IEnumerable<GameModel> FilteredGames => 
            string.IsNullOrWhiteSpace(SearchText) 
                ? Games 
                : Games.Where(g => g.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
    }
}
