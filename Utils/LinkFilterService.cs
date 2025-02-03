using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ai_research_app.Services;

namespace ai_research_app.Utils
{
    public class LinkFilterService
    {
        private readonly OpenAIService _openAiService;

        public LinkFilterService(OpenAIService openAiService)
        {
            _openAiService = openAiService;
        }

        public async Task<List<string>> GetRelevantLinks(string baseUrl, List<string> links)
        {
            if (links.Count == 0) return new List<string>();

            string systemPrompt = "You are an AI assistant that helps evaluate third-party vendor websites. "
                                  + "Your goal is to return ONLY the most relevant web pages in JSON format. "
                                  + "ONLY include links that provide insights into: "
                                  + "- **API Documentation & Integration** (API docs, SDKs, developer guides) "
                                  + "- **Pricing & Subscription Plans** (Cost, enterprise plans, free trials) "
                                  + "- **Security & Compliance** (HIPAA, GDPR, SOC 2, security policies) "
                                  + "- **Company Overview** (About Us, partnerships, leadership) "
                                  + "DO NOT include: Terms of Service, Privacy Policy, Contact Forms, Blog Articles, Careers. "
                                  + "Respond STRICTLY in JSON format as follows: "
                                  + "{ \"relevant_links\": [\"full_url_1\", \"full_url_2\", \"full_url_3\"] }";


            string userPrompt = "The following links were found on a vendor's website. "
                                + "Filter and return ONLY those relevant for vendor evaluation. "
                                + "Return STRICTLY in JSON format as follows: "
                                + "{ \"relevant_links\": [\"full_url_1\", \"full_url_2\", \"full_url_3\"] }\n\n"
                                + $"Base URL: {baseUrl}\n\nLinks:\n"
                                + string.Join("\n", links);

            string responseJson = await _openAiService.GetJsonResponse(systemPrompt, userPrompt);
            
            responseJson = JsonHelper.ExtractJsonFromMarkdown(responseJson);

            try
            {
                var jsonResponse = JsonSerializer.Deserialize<FilteredLinkResponse>(responseJson);
                return jsonResponse?.Links ?? new List<string>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error parsing filtered links: " + ex.Message);
                Console.WriteLine("Raw OpenAI Response: " + responseJson);
                return new List<string>();
            }
        }
    }

    public class FilteredLinkResponse
    {
        [JsonPropertyName("relevant_links")]
        public List<string> Links { get; set; } = new();
    }
}
