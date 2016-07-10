using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Diagnostics;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace WebApplication1
{
    public class GetWeather
    {
        //calls weather API and stores relevent data retrieved in the
        public static void RetrieveWeatherData()
        {
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            long retrievalTimestamp = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            string apiKey = "0675025a78a80e8b2b663313e8a25247";
            //normally I would read this from a file, but considering this is a 
            //simple exercise I'll save us both time and hard code the array
            string[] cityIDs = {"2759794", "3041563", "264371", "792680", "2950159", "3060972", "2800866",
                "683506", "3054643","618426","2618425", "4192205", "658225", "703448", "6458923", "3196359",
                "2643743", "2960316", "3117735", "625144", "2993458", "524901", "146268", "3421319", "3143244",
                "3067696", "3413829", "456172", "3169070", "3191281", "785842", "727011", "2673730", "588409",
                "3183875", "3042030", "2562305", "6691831", "593116", "756135", "6618988"};

            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                using (System.Net.WebClient wc = new System.Net.WebClient())
                {
                    //loop over each location ID
                    for (int i = 0; i < cityIDs.Length; i++)
                    {
                        //build and execute api call for current city
                        String queryString = "http://api.openweathermap.org/data/2.5/weather?id=" + cityIDs[i] + "&appid=" + apiKey;
                        String jsonString = wc.DownloadString(queryString);
                        dynamic jsonDict = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonString);

                        //create WeatherInstance row for current city
                        String commandString = "INSERT INTO WeatherInstance (fetchTimeStamp,cityName,countryCode, weatherLocationID, temperatureKelvin, windSpeedKMPH, humidity, main, sunrise) VALUES (@val1, @val2, @val3, @val4, @val5, @val6, @val7, @val8, @val9)";
                        using (SqlCommand comm = new SqlCommand(commandString, connection))
                        {
                            comm.CommandText = commandString;
                            comm.Parameters.AddWithValue("@val1", retrievalTimestamp);
                            comm.Parameters.AddWithValue("@val2", jsonDict.name.ToObject<string>());
                            comm.Parameters.AddWithValue("@val3", jsonDict.sys.country.ToObject<string>());
                            comm.Parameters.AddWithValue("@val4", cityIDs[i]);
                            comm.Parameters.AddWithValue("@val5", jsonDict.main.temp.ToObject<string>());
                            comm.Parameters.AddWithValue("@val6", jsonDict.wind.speed.ToObject<string>());
                            comm.Parameters.AddWithValue("@val7", jsonDict.main.humidity.ToObject<string>());
                            comm.Parameters.AddWithValue("@val8", jsonDict.weather[0].main.ToObject<string>());
                            comm.Parameters.AddWithValue("@val9", jsonDict.sys.sunrise.ToObject<string>());
                            int result = comm.ExecuteNonQuery();
                        }

                    }//end api loop
                    connection.Close();
                }
            }
            catch(SqlException sqlE)
            {
                Debug.WriteLine("Fetching new weather data and adding to database\n" + sqlE);
            }
        }
    }
}
