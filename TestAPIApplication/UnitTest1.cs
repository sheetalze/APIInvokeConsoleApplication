using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace TestAPI
{


[TestFixture]
public class APITest
{
    private HttpClient _httpClient;

    [SetUp]
    public void Setup()
    {
        // Set up any test-specific configuration here
        _httpClient = new HttpClient();
    }

    //[TearDown]
    //public void TearDown()
    //{
    //    // Clean up resources after each test
    //    _httpClient.Dispose();
    //}

    [Test]
    public async Task Given_ApiEndpoint_When_CallingApi_Then_ReturnTotalCountAndSortedData()
    {
        // Given
        string apiUrl = "https://petstore.swagger.io/v2/pet/findByStatus?status=available";

        // When
        HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

        // Then
        Assert.IsTrue(response.IsSuccessStatusCode);

        string petJson = await response.Content.ReadAsStringAsync();
        List<Pets> pet = JsonConvert.DeserializeObject<List<Pets>>(petJson);

        // Check total count
        int totalCount = pet.Count;
        Assert.Greater(totalCount, 0);

        // Check sorting
        List<Pets> sortedPets = new List<Pets>(pet);
        sortedPets.Sort((a, b) => string.Compare(a.Name, b.Name));

        for (int i = 1; i < sortedPets.Count; i++)
        {
            string previousName = sortedPets[i - 1].Name;
            string currentName = sortedPets[i].Name;
            Assert.That(previousName, Is.LessThanOrEqualTo(currentName));
        }
    }

    public class Pets
    {
        public string Name { get; set; }
    }
}
}