using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;

namespace GameLauncher.Models
{
    public partial class GameModel : ObservableObject
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private string _executablePath;

        [ObservableProperty]
        private string _iconPath;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(LastPlayedDisplay))]
        private DateTime? _lastPlayed;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PlayTimeDisplay))]
        private TimeSpan _totalPlayTime = TimeSpan.Zero;

        public List<string> Tags { get; set; } = new List<string>();
        public int Rating { get; set; } // 0-5
        
        // Helper for display
        public string PlayTimeDisplay => $"{(int)TotalPlayTime.TotalHours}h {TotalPlayTime.Minutes}m";
        public string LastPlayedDisplay => LastPlayed.HasValue ? LastPlayed.Value.ToString("yyyy-MM-dd HH:mm") : "Never";
    }
}
