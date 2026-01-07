# GameLauncher

GameLauncher is a modern, polished desktop application for managing and launching your games. Inspired by platforms like Steam and GOG Galaxy, it allows you to manually add games, assign custom icons, and track metadata such as playtime and last played date.

## Features

- **Game Management**:
  - Add games manually by selecting `.exe` files.
  - Assign custom names and custom icons (default: executable icon).
  - Store metadata: name, path, icon, last played, playtime, tags, ratings.
  - Save the library locally in JSON format.

- **Game Launching**:
  - Launch games directly from the app.
  - Track last played and total playtime.

- **Modern GUI**:
  - Slick, modern interface with responsive layouts, smooth animations, and transitions.
  - Grid and list view of games with large, high-quality icons.
  - Hover effects, tooltips, and context menus (launch, rename, remove, change icon, view path, edit tags).
  - Dark/light themes with smooth toggle transitions.

- **Extra Features**:
  - Search, filter, and sort games by name, tags, or last played.
  - Import/export game library.
  - Automatic icon extraction from executables.

## Requirements

- **Operating System**: Windows 10 or later
- **.NET Runtime**: .NET 9.0 (self-contained builds include the runtime)

## Installation

1. Download the latest release from the `publish` folder or build the project yourself (see below).
2. Run the `GameLauncher.exe` file to start the application.

## Build Instructions

To build the project, ensure you have the .NET SDK installed.

1. Clone the repository:
   ```bash
   git clone <repository-url>
   cd game-launcher
   ```

2. Build the project:
   ```powershell
   .\build.ps1
   ```

3. The standalone executable will be located in the `publish` folder.

## Clean Build Artifacts

To clean the project and remove all build artifacts, run:
```powershell
.\clean.ps1
```

## Contributing

Contributions are welcome! Feel free to submit issues or pull requests to improve the project.

## License

This project is licensed under the MIT License. See the `LICENSE` file for details.