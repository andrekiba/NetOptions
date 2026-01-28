# Service Lifetime Reminder

- **Transient**: Created anew each time they're requested.
- **Scoped**: Created anew for a single scope as needed.
- **Singleton**: Created only once for the life of the app.

## Options Interfaces

- `IOptions<TOptions>`:

  - Singleton service lifetime.
  - Only read once, at app startup.

- `IOptionsSnapshot<TOptions>`:

  - Scoped service lifetime.
  - Values are recomputed for each new scope.
  - Designed for transient and scoped dependencies.

- `IOptionsMonitor<TOptions>`:

  - Singleton service lifetime.
  - Enables change detection.
  - Supports dynamic reloading of values.

# Monitoring Limitations
1. Limited to File-System based configuration providers (so for example no Environment Variables):
    - Microsoft.Extensions.Configuration.Json
    - Microsoft.Extensions.Configuration.Xml
    - Microsoft.Extensions.Configuration.Ini
    - Microsoft.Extensions.Configuration.UserSecrets
2. Some environments, such as File Shares or Docker Containers are unreliable for change detection.
    - Set `DOTNET_USE_POLLING_FILE_WATCHER` to `true` to use polling instead of file system events (every 4 seconds not configurable).