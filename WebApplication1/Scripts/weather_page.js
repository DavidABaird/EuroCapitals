function createWeatherLI(weather)
{
    var date = new Date(parseInt(weather.fetchTimeStamp));
    if (!(date.getFullYear() <= 2016))
        date = new Date(Date.now() - (1000*52*34*24));
    var minuteString = date.getMinutes();
    if (minuteString.length < 2)
        minuteString = "0" + minuteString;
    var dateString = "" + date.getHours() + ":" + minuteString + "  " + date.getDate() + "/" + (1 + parseInt(date.getMonth())) + "/" + date.getFullYear();
    var celcius = Math.round((parseInt(weather.temperatureKelvin) - 273.15) * 10) / 10;
    var sunriseDate = new Date(weather.sunrise * 1000);
    minuteString = sunriseDate.getMinutes();
    if (minuteString.length < 2)
        minuteString = "0" + minuteString;
    var sunriseTimeString = "" + sunriseDate.getHours() + ":" + minuteString;

    return "<li class = \"weather-object\">\
                <h3 class = \"cityName\">" + weather.cityName + ", <class = \"countryCode\">" + weather.countryCode + "</></h3>\
                  <p hidden class = \"fetchTimeStamp\">" + weather.fetchTimeStamp + "</p>\
                  <ol class = \"weather-datablock\">\
                    <li class = \"fetchTime\">Recorded " + dateString + "</li>\
                    <li class = \"main\">Weather " + weather.main + "</li>\
                    <li class = \"sunrise\">Sunrise " + sunriseTimeString + "</li>\
                  </ol>\
                  <ol class = \"weather-datablock\">\
                    <li class = \"temperature\">Temp " + celcius + " C</li>\
                    <li class = \"humidity\">Humidity " + weather.humidity + "</li>\
                    <li class = \"windspeed\">Wind " + weather.windSpeedKMPH + " km/h</li>\
                  </ol>\
           </li>"
}
function populateWeatherList(weathers)
{
    for(weather in weathers)
    {
        $(".list").append(createWeatherLI(weathers[weather]));
    }

    var userList = new List('WeatherInstances', {
        "valueNames": [
        "cityName",
        "fetchTime",
        "main",
        "temperature",
        "humidity",
        "windspeed",
        "sunrise",
        "fetchTimeStamp"
        ]
    });
}
$(document).ready(function(){
    populateWeatherList(allWeather);
});