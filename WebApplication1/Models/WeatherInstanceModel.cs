using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class WetherInstance
    {
        public long fetchTimeStamp { get; set; }
        
        public String cityName { get; set; }
        public String countryCode { get; set; }
        public String weatherLocationID { get; set; }

        public String main { get; set; }
        public String temperatureKelvin { get; set; }
        public String windSpeedKMPH { get; set; }
        public String humidity { get; set; }
        public String sunrise { get; set; }
    }
}