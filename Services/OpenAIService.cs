using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ai_research_app.Services
{
    public class OpenAIService
    {
        private readonly string _apiKey;
        private readonly HttpClient _client;
        private const string OpenAiEndpoint = "https://api.openai.com/v1/chat/completions";
        private const string Model = "gpt-4o-mini";

        public OpenAIService(string apiKey)
        {
            _apiKey = apiKey;
            _client = new HttpClient();
        }

        public async Task<string> GetJsonResponse(string systemPrompt, string userPrompt)
        {
            var requestBody = new
            {
                model = Model,
                messages = new[]
                {
                    new { role = "system", content = systemPrompt },
                    new { role = "user", content = userPrompt }
                },
                response_format = new {type = "json_object"}
            };

            var requestContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            using var request = new HttpRequestMessage(HttpMethod.Post, OpenAiEndpoint)
            {
                Content = requestContent
            };

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);
            request.Headers.Add("User-Agent", "MyCustomApp/1.0");
            
            using HttpResponseMessage response = await _client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"OpenAI API request failed: {response.StatusCode}\n{responseContent}");
            }

            try
            {
                Console.WriteLine("Raw OpenAI Response: " + responseContent);

                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var jsonResponse = JsonSerializer.Deserialize<OpenAIResponse>(responseContent, jsonOptions);

                if (jsonResponse?.Choices == null || jsonResponse.Choices.Count == 0)
                {
                    throw new Exception("No valid response content found in OpenAI response.");
                }

                return jsonResponse.Choices[0].Message.Content;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error processing OpenAI response: " + ex.Message);
                Console.WriteLine("Raw OpenAI Response: " + responseContent);
                throw;
            }
        }
        public async Task<string> GetResponse(string systemPrompt, string userPrompt)
        {
            var requestBody = new
            {
                model = Model,
                messages = new[]
                {
                    new { role = "system", content = systemPrompt },
                    new { role = "user", content = userPrompt }
                }
            };

            var requestContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            using var request = new HttpRequestMessage(HttpMethod.Post, OpenAiEndpoint)
            {
                Content = requestContent
            };

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);
            request.Headers.Add("User-Agent", "MyCustomApp/1.0");
            
            using HttpResponseMessage response = await _client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"OpenAI API request failed: {response.StatusCode}\n{responseContent}");
            }

            try
            {
                Console.WriteLine("Raw OpenAI Response: " + responseContent);

                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var jsonResponse = JsonSerializer.Deserialize<OpenAIResponse>(responseContent, jsonOptions);

                if (jsonResponse?.Choices == null || jsonResponse.Choices.Count == 0)
                {
                    throw new Exception("No valid response content found in OpenAI response.");
                }

                return jsonResponse.Choices[0].Message.Content;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error processing OpenAI response: " + ex.Message);
                Console.WriteLine("Raw OpenAI Response: " + responseContent);
                throw;
            }
        }
    }

    public class OpenAIResponse
    {
        [JsonPropertyName("choices")]
        public List<OpenAIChoice> Choices { get; set; } = new();
    }

    public class OpenAIChoice
    {
        [JsonPropertyName("message")]
        public OpenAIMessage Message { get; set; } = new();
    }

    public class OpenAIMessage
    {
        [JsonPropertyName("role")]
        public string Role { get; set; } = "";

        [JsonPropertyName("content")]
        public string Content { get; set; } = "";
    }
}
