using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        /*
         *queries DB for weather instances
         * converts said instances to a json string and renders view
         */
        public ActionResult Index()
        {
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                String weatherQuerey = "SELECT * FROM WeatherInstance ORDER BY fetchTimeStamp DESC";
                using (SqlCommand cmd = new SqlCommand(weatherQuerey, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        ViewBag.allWeather = sqlReaderJsonSerializer(reader);

                    }
                }

                connection.Close();
            }
            catch(SqlException sqlE)
            {
                Debug.WriteLine("Error retrieving weather data from database");
            }
            return View();
        }
        /*
         * function to create a json string from a SQL DataReader
         * */
        String sqlReaderJsonSerializer(IDataReader reader)
        {
            if (reader == null)
            {
                return "null";
            }

            StringBuilder serializedJson = new StringBuilder();

            int curInstance = 0;

            serializedJson.Append("{\"weather\":[");

            while (reader.Read())
            {
                serializedJson.Append("{");

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    serializedJson.Append("\"" + reader.GetName(i) + "\": ");
                    serializedJson.Append("\"" + reader[i] + "\"");
                    serializedJson.Append(",");
                }

                if (reader.FieldCount > 0)
                    serializedJson.Length -= 1;

                serializedJson.Append("},");

                curInstance++;
            }
            
            if (curInstance > 0)
                serializedJson.Length -= 1;

            serializedJson.Append("]}");
            return serializedJson.ToString();
        }
    }
}