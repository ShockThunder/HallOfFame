using HallOfFame.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;

namespace HallOfFame.IntegrationTest
{
    public class PersonTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public PersonTests(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

      
        [Fact]
        public void TestGetPersons()
        {
            var request = "/api/v1/Persons";

            var response = _client.GetAsync(request).Result;

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public void TestGetPerson()
        {
            var request = "/api/v1/Person/1";

            var response = _client.GetAsync(request).Result;

            response.EnsureSuccessStatusCode();
        }


        // контекст БД при попытке использовать update в тесте выдает ошибку. Исправить пока не удалось.
        // На данный момент возвращает корректную ошибку 500.
        // Возможно, если настроить подключение к реальной БД вместо inmemory, то ошибка уйдет.
        [Fact]
        public void TestPostPerson()
        {

            var testPerson = new Person()
            {
                id = 1,
                name = "Test",
                displayName = "TestDisplay",
                skills = new List<Skill>()
                {
                    new Skill { id = 1, name = "asserting", level = 3},
                }
            };

            string json = JsonConvert.SerializeObject(testPerson);

            var request = "/api/v1/Person/1";

            var response = _client.PostAsync(request, new StringContent(json, Encoding.UTF8, "application/json")).Result;

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public void TestPutPerson()
        {
            var testPerson = new Person()
            {
                name = "Test",
                displayName = "TestDisplay",
                skills = new List<Skill>()
                {
                    new Skill { id = 1, name = "asserting", level = 3},
                }
            };

            string json = JsonConvert.SerializeObject(testPerson);

            var request = "/api/v1/Person";

            var response = _client.PutAsync(request, new StringContent(json, Encoding.UTF8, "application/json")).Result;

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public void TestDeletePerson()
        {
            var request = "/api/v1/Person/1";

            var response = _client.DeleteAsync(request).Result;

            response.EnsureSuccessStatusCode();
        }
    }
}
