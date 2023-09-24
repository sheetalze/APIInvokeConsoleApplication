
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace APIInvoke
{
    class Program
    {
        static async Task Main()
        {
            /*
             Main objective of this code-
            1- Call API method using console application
            2- Get the data from API GET method in JSOn format
            3-Deserilaize the JSON
            4-Map the JSOn fields with locally created model class with all the fields we need
            5-Map the fields and print 
            6-1 unit test case included

             */
            // here is the API endpoint URL for the GET method
            string apiUrl = "https://petstore.swagger.io/v2/pet/findByStatus?status=available";

            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    // Make an HTTP GET request to the API
                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                    // Check if the request was successful (status code 200)
                    if (response.IsSuccessStatusCode)
                    {
                        // first need to read and parse the API response as JSON from API GET method
                        string petJson = await response.Content.ReadAsStringAsync();

                        // need to deserialize the JSON into a list of pets from API JSOn response
                        List<Pets> pets = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Pets>>(petJson);
                        //to use JSOn deserialize option need to install newtownsoft from nuget package manager

                        int totalCount = pets.Count;
                        Console.WriteLine($"Total Count of all Pets: {totalCount}");

                        Console.WriteLine($"Below is teh full list of all Pets");
                        // Sort the products by a specific property, e.g., ProductName
                        pets.Sort((a, b) => string.Compare(a.Name, b.Name));


                        // loop all data cming via get method and show the details for each after mapping it with class model pet
                        foreach (Pets pet in pets)
                        {
                            //Console.WriteLine($"Pet ID: {pet.Id}");
                            Console.WriteLine($"Pet Name: {pet.Name}");
                            //Console.WriteLine($"Pet Status: {pet.Status}");
                            Console.WriteLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine($"There is an error with API call and error/ status code is: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }

    // This class is having fields and which will map to all fields from API GEt method for Pets,
    // we need onyl few fields not all,checked this from API get method model structure for GETPEt method
    public class Pets
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
    }

}



