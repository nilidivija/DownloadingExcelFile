using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace DownloadingExcelFile.Data
{
    public class FruitService:IFruitService
    {

        private HttpClient http { get; }
        public FruitService(HttpClient Http)
        {
            Http.BaseAddress = new Uri("https://fruityvice.com/");
            http = Http;
        }

 
        public async Task<List<Fruit>> GetAllFruits()
        {
            var result = new List<Fruit>();
            string apiUrl = "api/fruit/all";

            var response = await http.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                result = JsonConvert.DeserializeObject<List<Fruit>>(await response.Content.ReadAsStringAsync());
                string jsonData = JsonConvert.SerializeObject(result, Formatting.None);
                if (!System.IO.File.Exists("jsondata.txt"))
                {
                   var txt= System.IO.File.Create("jsondata.txt");
                    txt.Close();
                    System.IO.File.WriteAllText("jsondata.txt", jsonData);
                }
                else
                {
                    System.IO.File.WriteAllText("jsondata.txt", string.Empty);
                    System.IO.File.WriteAllText("jsondata.txt", jsonData);
                    

                }

            }

       
            return result;



        }

        public async Task<List<Fruit>> SearchFruits(string query)
        {
           var result = new List<Fruit>();

            if (query == "")
            {
                string apiUrl = "api/fruit/all";

                var response = await http.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<List<Fruit>>(await response.Content.ReadAsStringAsync());

                }
            }
            else
            {
                string apiUrl = "api/fruit/family/" + query;

                var response = await http.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<List<Fruit>>(await response.Content.ReadAsStringAsync());

                }
            }
            string jsonData = JsonConvert.SerializeObject(result, Formatting.None);
            if (System.IO.File.Exists("jsondata.txt"))
            {
                System.IO.File.WriteAllText("jsondata.txt", string.Empty);
                System.IO.File.WriteAllText("jsondata.txt", jsonData);
            }
          
            return result;
        }

        

    

    }
}
