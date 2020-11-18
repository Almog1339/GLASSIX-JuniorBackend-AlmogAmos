using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GLASSIX_JuniorBackend_AlmogAmos
{
    class DadJoke
    {
        public string id { get; set; }
        public string joke { get; set; }
        public int status { get; set; }

        public static async Task<DadJoke> GetJokeAsync()
        {
            /*
             * An async function, that fetch us a random dad joke from the icanhazdadjoke API.
             * Once we got a JSON back we deserialize the result to an DadJoke class object.
            */
            DadJoke joke = new DadJoke();
            _ = await Task.Run(async () =>
            {
                string dadJokeApi = "https://icanhazdadjoke.com/";
                HttpClient _httpClient = new HttpClient();
                _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                string httpMessage = await _httpClient.GetStringAsync(dadJokeApi);
                joke = JsonConvert.DeserializeObject<DadJoke>(httpMessage);
                return joke;
            });
            return joke;
        }

    }
}
