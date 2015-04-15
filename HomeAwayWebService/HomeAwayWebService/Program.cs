//API Test section - David Lawrence
//Dependencies - C# Console Project with Visual Studio using .Net 4.5 and additional external library, Newtonsoft.Json (http://www.newtonsoft.com )
//Can install newtonsoft with NuGet: Install-Package Newtonsoft.Json

using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;


class Program
{

    //define Classes to support JSON fields documented here: https://api.data.gov/docs/nrel/transportation/alt-fuel-stations-v1/nearest/#precision-record-fields

      public class CurrencyRates {
          public string Disclaimer { get; set; }
          public string License { get; set; }
          public int TimeStamp { get; set; }
          public string Base { get; set; }
          public Dictionary<string, decimal> Rates { get; set; }
    }
    
        public class PrecisionClass
        {
            public string name { get; set; }
            public int value { get; set; }
            public IList<string> types { get; set; }
        }

      public class Fuel
      {
          public int Total_results { get; set; }
          public int Offset  { get; set; }
          public string Station_locator_url { get; set; }
          public decimal latitude { get; set; }
          public decimal longitude { get; set; }
          public PrecisionClass Precision { get; set; }
          public IList<Fuel_Stations_Class> Fuel_Stations { get; set; }
      }

      public class FederalAgencyClass
      {
          public int id { get; set; }
          public string name { get; set; }
      }

      public class Fuel_Stations_Class
      {
          public string Fuel_type_code { get; set; }
          public string Station_name { get; set; }
          public string Street_address { get; set; }
          public string intersection_directions { get; set; }
          public string city { get; set; }
          public string state { get; set; }
          public string zip { get; set; }
          public string plus4 { get; set; }
          public string station_phone { get; set; }
          public string status_code { get; set; }
          public int? expected_date { get; set; }
          public string groups_with_access_code { get; set; }
          public string access_days_time { get; set; }
          public string cards_accepted { get; set; }
          public string owner_type_code { get; set; }
          public IList<FederalAgencyClass> federal_agency { get; set; }
          public string bd_blends { get; set; }
          public bool? e85_blender_pump { get; set; }
          public bool? lpg_primary { get; set; }
          public string ng_fill_type_code { get; set; }
          public string ng_psi { get; set; }
          public string ng_vehicle_class { get; set; }
          public int? ev_level1_evse_num { get; set; }
          public int? ev_level2_evse_num { get; set; }
          public int? ev_dc_fast_num { get; set; }
          public string ev_other_evse { get; set; }
          public string ev_network { get; set; }
          public string ev_network_web { get; set; }
          public string hy_status_link { get; set; }
          public string geocode_status { get; set; }
          public decimal latitude { get; set; }
          public decimal longitude { get; set; }
          public int? open_date { get; set; }
          public string date_last_confirmed { get; set; }
          public string updated_at { get; set; }
          public int id { get; set; }
          public decimal distance { get; set; }
      }

      


    // Helper function to un-serialize JSON format to classes defined above 
    // url:   url to download
    // returns new collection T
    private static T _download_serialized_json_data<T>(string url) where T : new() {
    using (var w = new WebClient()) {
        var json_data = string.Empty;
        // attempt to download JSON data as a string
        try {
          json_data = w.DownloadString(url);
        }
        catch (Exception) {}
        // if string with JSON data is not empty, deserialize it to class and return its instance 
        return !string.IsNullOrEmpty(json_data) ? JsonConvert.DeserializeObject<T>(json_data) : new T();
      }
    }

    //LogTestResult - helper function to log result of test - currently a simple stub
    // Extend this helper as neeeded to support additional types, and logging needs of tests, etc

    static void LogTestResult(string testResult)
    {   
            Console.WriteLine (testResult);        
        
    }

       //Test1 - Query for nearest stations to Austin, TX that are part of the “ChargePoint Network” and return the Station Id of the HYATT AUSTIN station.
    //returns string - used to pass value of station ID to next test case.
    static void Test1()
    {
        string strExpectedValue = "HYATT AUSTIN";        
        var url = "http://api.data.gov/nrel/alt-fuel-stations/v1/nearest.json?api_key=6ZxRGjBYs9ceQBnQ62hh8as2HKLV2vsEhgvNP5oA&location= Austin, TX&fuel_type=ELEC&ev_network=ChargePoint Network";
        var fuel = _download_serialized_json_data<Fuel>(url); 

        //iterate through values and look for station-name, HYATT Austin
        for (int i=0;i<fuel.Fuel_Stations.Count;i++)
        {
            //Console.WriteLine(fuel.Fuel_Stations[i].Station_name);
            if (fuel.Fuel_Stations[i].Station_name.ToLower() == strExpectedValue.ToLower())
            {
                //found expected - log result
                LogTestResult("Test 1 Passed:  Expected value " + strExpectedValue + " Actual value " + fuel.Fuel_Stations[i].Station_name);
                return;
            }
        }
        LogTestResult("Not found");
        
       
    } //Test1

    //Test2 -Use the Station ID from previous test to query the API and return the Street Address of that station.
    static void Test2()
    {
        
        //As we are expecting single record back, test is simply getting an HTTP response and parsing single record for the expected address
        string url = "http://api.data.gov/nrel/alt-fuel-stations/v1/62029.json?api_key=6ZxRGjBYs9ceQBnQ62hh8as2HKLV2vsEhgvNP5oA";
        string expectedValue = "208 Barton Springs Rd";   //know expected value since record is well known data.   Could read this dynamically from test above if needed

         WebClient client = new WebClient();

        // Download json response 
        string fuelStationRecord = client.DownloadString(url);
        if (fuelStationRecord.Contains(expectedValue) == true)
        {
            LogTestResult("Test 2: Passed");
        }
        else
        {
            LogTestResult("Failed");
        }

    } //Test2

    static void Main()
    {
        //Let's kick off our two tests: 
        Test1();
        Test2();
        Console.ReadLine(); //pausing here since we're sending our result data to console
    
    }

   
}
