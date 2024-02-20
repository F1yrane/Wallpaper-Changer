using System;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using OpenCvSharp;


namespace WallpaperChanger
{
    public class WallpaperChanger // changes wallpaper depending on the time
    {
        // consts for changing the wallpaper on desktop
        private const int SPI_SETDESKWALLPAPER = 0x0014;
        private const int SPIF_UPDATEINIFILE = 0x01;
        private const int SPIF_SENDCHANGE = 0x02;

        public async Task ChangeWallpaper()
        {
            DateTime now = DateTime.Now;

            string weatherCondition = await WeatherChecker.RunWeatherCheck(); // recives info about the current weather

            string partOfDay = (now.Hour >= 6 && now.Hour < 18) ? "bright" : "dark"; // from 6 a.m to 6 p.m - bright, else dark

            string unsplashUrl = $"https://source.unsplash.com/random?{partOfDay},{weatherCondition}";

            // path to save image
            string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            string filePath = Path.Combine(downloadPath, $"wallpaper_{partOfDay}.jpg");

            try
            {
                using (var client = new HttpClient())
                {
                    // download the image
                    var response = await client.GetAsync(unsplashUrl);
                    response.EnsureSuccessStatusCode();
                    using var imageStream = await response.Content.ReadAsStreamAsync();
                    using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
                    await imageStream.CopyToAsync(fileStream);
                }

                if (ImageToneAnalyzer.Analyze(filePath) == partOfDay)
                {
                    SetWallpaper(filePath); // set wallpaper if it matches the desired tone
                    Console.WriteLine($"Wallpaper successfully changed");
                }
                else
                {
                    await ChangeWallpaper(); // retry if the tone doesn't match
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error when setting wallpaper: {ex.Message}");
            }
        }

        private void SetWallpaper(string filePath)
        {
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, filePath, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE); // Set the wallpaper using Windows API
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
    }

    public class ImageToneAnalyzer // defines the tone of image
    {
        public static string Analyze(string imagePath)
        {
            var image = Cv2.ImRead(imagePath, ImreadModes.Grayscale);
            if (image.Empty())
            {
                throw new FileNotFoundException($"Failed to load image from path: {imagePath}");
            }

            var avgIntensity = Cv2.Mean(image);
            return avgIntensity.Val0 > 127 ? "bright" : "dark"; // determine whether the image is bright or dark based on average intensity
        }
    }
}


