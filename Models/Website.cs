using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace ai_research_app.Models
{
    public class Website
    {
        public string Url { get; }
        public string Title { get; private set; } = "No title found";
        public string Text { get; private set; } = "";
        public List<string> Links { get; private set; } = new List<string>();
        public List<string> FilteredLinks { get; set; } = new List<string>();

        private static readonly HttpClient Client = new HttpClient();

        public Website(string url) => Url = url;

        public async Task ScrapeAsync()
        {
            using var response = await Client.GetAsync(Url);
            string html = await response.Content.ReadAsStringAsync();

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            Title = doc.DocumentNode.SelectSingleNode("//title")?.InnerText.Trim() ?? "No title found";

            var bodyNode = doc.DocumentNode.SelectSingleNode("//body");
            Text = bodyNode?.InnerText.Trim() ?? "";

            Links = doc.DocumentNode.SelectNodes("//a[@href]")?
                .Select(node => node.GetAttributeValue("href", ""))
                .Where(link => !string.IsNullOrEmpty(link))
                .ToList() ?? new List<string>();
        }

        public string GetContents() => $"Title: {Title}\nContent: {Text}\n";
    }
}