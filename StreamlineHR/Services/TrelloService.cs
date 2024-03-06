using StreamlineHR.Commons.Model;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StreamlineHR.Services
{
    public class TrelloService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public TrelloService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        /// <summary>
        /// Post card on the list on trello board;
        /// TODO: list still hardcode
        /// Change Trello Key and Token on appsetting to insert to your own trello
        /// </summary>
        /// <param name="name"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        public async Task<ResultStatus<string>> PostCard(string name, string desc)
        {
            var rs = new ResultStatus<string>();
            string idList = "65e5bc0c46fb7e880234ad04";
            string[] idLabels = ["65e5bc03bf92ddda4f54b71a"];
            HttpResponseMessage response = new HttpResponseMessage();
            string baseUrl = "https://api.trello.com/1/cards";
            string key = _configuration["TrelloConfig:Key"];
            string token = _configuration["TrelloConfig:Token"];

            // Construct the request URL with parameters
            string requestUrl = $"{baseUrl}?idList={idList}&key={key}&token={token}";

            try
            {
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve
                };
                var json = JsonSerializer.Serialize(new { name, desc, idList, idLabels }, options);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                response = await _httpClient.PostAsync(requestUrl, content);
                response.EnsureSuccessStatusCode();
                rs.Status = true;
                rs.Message = "Success insert to trello card";
                rs.Data = "";
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error posting card to Trello API: {ex.Message}");
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                rs.Status = false;
                rs.Code = (int) HttpStatusCode.InternalServerError;
                rs.Message = "Failed insert to trello card";
                rs.Data = ex.ToString();
            }

            return rs;
        }
    }
}
