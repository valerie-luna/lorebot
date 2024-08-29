namespace Weather
{
    public delegate WeatherUpdate WeatherCalculator(in WeatherValues values, 
        WeatherEnum weather, CloudLevel cloud, Random rand);
}