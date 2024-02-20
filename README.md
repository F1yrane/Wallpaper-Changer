# Wallpaper Changer

This project is an application for automatically changing desktop wallpapers based on the time of day and current weather conditions. It utilizes open APIs to retrieve weather information and images.

## Why?

Having a suitable desktop background can often improve mood and boost productivity. The Wallpaper Changer provides variety and ensures that desktop wallpapers match the current time and weather conditions, creating a pleasant and cozy work environment.

## How to Use?

1. **Setting up API Keys:**
   - To ensure the proper functioning of the application, you need to obtain API keys for IPGeolocation and OpenWeatherMap. These keys are used to retrieve user location information and current weather conditions, respectively. Add the API keys to the corresponding variables in the code.

2. **Running the Application:**
   - After setting up the API keys and installing the application, run it. The application will automatically start with each system boot and change desktop wallpapers every three hours.

3. **Adjusting Wallpaper Change Interval:**
   - By default, wallpapers are changed every three hours. If you need to adjust this interval, edit the value of the `period` variable in the `Main` method of the `Program` class.

4. **Installing Dependencies:**
   - Before running the application, make sure to install the necessary dependencies. You can install them using NuGet Package Manager or the .NET CLI. The required packages are:
     - `OpenCvSharp`: Used for image processing and tone analysis. Make sure to install the appropriate runtime for your platform (e.g., `OpenCvSharp4.runtime.win` for Windows).
     - `Newtonsoft.Json`: Used for JSON parsing.


5. **Note:**
   - The application automatically adds itself to system startup to ensure continuous desktop wallpaper changes.
