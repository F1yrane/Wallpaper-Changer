using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

class WeatherChecker
{
    public static async Task<string> RunWeatherCheck()
    {
        try
        {
            var (IP, Latitude, Longitude) = await GetLocation();
            string? city = await GetCityName(IP);
            string? weather = await GetWeather(city);
            return weather;
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }

    static async Task<(string IP, double Latitude, double Longitude)> GetLocation()
    {
        string ipGeolocationApiKey = "419692603b324adfb8f95416532489ee"; // Вставьте ваш API ключ от IPGeolocation

        using (var httpClient = new HttpClient())
        {
            HttpResponseMessage response = await httpClient.GetAsync($"https://api.ipgeolocation.io/ipgeo?apiKey={ipGeolocationApiKey}");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            dynamic locationData = JObject.Parse(responseBody);
            string ip = locationData.ip;
            double latitude = (double)locationData.latitude;
            double longitude = (double)locationData.longitude;
            return (ip, latitude, longitude);
        }
    }

    static async Task<string> GetCityName(string ip)
    {
        string ipGeolocationApiKey = "419692603b324adfb8f95416532489ee"; // Вставьте ваш API ключ от IPGeolocation

        using (var httpClient = new HttpClient())
        {
            HttpResponseMessage response = await httpClient.GetAsync($"https://api.ipgeolocation.io/ipgeo?apiKey={ipGeolocationApiKey}");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            dynamic locationData = JObject.Parse(responseBody);
            string city = locationData.city;
            return city;
        }
    }

    static async Task<string> GetWeather(string city)
    {
        string openWeatherMapApiKey = "634c1170cf1de7812a5e3d6c47a62743"; // Вставьте ваш API ключ от OpenWeatherMap

        using (var httpClient = new HttpClient())
        {
            HttpResponseMessage response = await httpClient.GetAsync($"http://api.openweathermap.org/data/2.5/weather?q={city}&appid={openWeatherMapApiKey}&units=metric");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            dynamic weatherData = JObject.Parse(responseBody);

            string weatherDescription = weatherData.weather[0].main;
            return weatherDescription;
        }
    }
}
