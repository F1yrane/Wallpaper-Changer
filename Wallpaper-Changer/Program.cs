using System;
using System.IO;
using System.Threading;

namespace WallpaperChanger
{
    class Program
    {
        static void Main()
        {
            AddToStartup();

            // timer that runs the app every 3 hours
            TimerCallback callback = new(TimerCallback);
            Timer timer = new(callback, null, dueTime: 0, period: 3 * 60 * 60 * 1000); // 3 hours in milliseconds

            Console.ReadLine();
        }

        static void TimerCallback(Object? stateInfo)
        {
            WallpaperChanger changer = new();
            changer.ChangeWallpaper().Wait();
        }

        static void AddToStartup() // adds an executable file to the startup
        {
            try
            {
                string startupFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Startup);

                string executablePath = System.Reflection.Assembly.GetExecutingAssembly().Location;

                // creates a shortcut in the startup folder
                string shortcutPath = Path.Combine(startupFolderPath, "WallpaperChanger.lnk");
                if (!File.Exists(shortcutPath))
                {
                    using StreamWriter writer = new(shortcutPath);
                    writer.WriteLine("[InternetShortcut]");
                    writer.WriteLine($"URL=file:///{executablePath.Replace('\\', '/')}");
                    writer.Flush();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error when adding to startup: {ex.Message}");
            }
        }
    }
}