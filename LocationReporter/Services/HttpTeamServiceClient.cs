using LocationReporter.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace LocationReporter.Services
{
    public class HttpTeamServiceClient : ITeamServiceClient
    {
        private readonly ILogger _logger;
        private HttpClient _httpClient;

        public HttpTeamServiceClient(
            IOptions<TeamServiceOptions> serviceOptions,
            ILogger<HttpTeamServiceClient> logger)
        {
            var url = serviceOptions.Value.Url;

            _logger = logger;
            _logger.LogInformation("Team Service HTTP client using URL {0}", url);
            _httpClient = new HttpClient { BaseAddress = new Uri(url) };
        }

        public Guid GetTeamForMember(Guid memberId)
        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = _httpClient.GetAsync(String.Format("/members/{0}/team", memberId)).Result;

            TeamIDResponse teamIdResponse;

            if (response.IsSuccessStatusCode)
            {
                string json = response.Content.ReadAsStringAsync().Result;
                teamIdResponse = JsonConvert.DeserializeObject<TeamIDResponse>(json);

                return teamIdResponse.TeamID;
            }

            return Guid.Empty;
        }
    }
}
