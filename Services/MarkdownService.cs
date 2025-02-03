using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ai_research_app.Models;
using ai_research_app.Utils;

namespace ai_research_app.Services
{
    public class MarkdownService
    {
        private readonly OpenAIService _openAiService;
        private readonly LinkFilterService _linkFilterService;
        private const int TokenLimit = 20000;
        private const int ApproxCharsPerToken = 4;

        public MarkdownService(OpenAIService openAiService)
        {
            _openAiService = openAiService;
            _linkFilterService = new LinkFilterService(openAiService);
        }

        public async Task<string> CreateMarkdown(string url, string outputFilePath)
        {
            string systemPrompt = "You are an AI assistant evaluating third-party vendor websites. "
                                + "Analyze relevant pages and generate a structured Markdown report including: "
                                + "- **API Documentation & Integration Info** "
                                + "- **Pricing & Subscription Plans** "
                                + "- **Security & Compliance Standards** "
                                + "- **Company Overview & Partnerships** "
                                + "Limit your response to important details only.";

            Website website = new Website(url);
            await website.ScrapeAsync();

            List<string> relevantLinks = await _linkFilterService.GetRelevantLinks(url, website.Links);
            website.FilteredLinks = relevantLinks;

            Console.WriteLine("Filtered Vendor-Relevant Links:");
            relevantLinks.ForEach(Console.WriteLine);

            int currentTokenCount = systemPrompt.Length / ApproxCharsPerToken;
            int maxAllowedChars = TokenLimit * ApproxCharsPerToken;

            StringBuilder userPrompt = new StringBuilder();
            userPrompt.Append($"Company: {website.Title}\n\nLanding Page:\n{website.GetContents()}");

            currentTokenCount += userPrompt.Length / ApproxCharsPerToken;

            foreach (var link in relevantLinks)
            {
                Website subPage = new Website(link);
                await subPage.ScrapeAsync();

                string subPageContent = $"\n\n{link}\n{subPage.GetContents()}";
                int subPageTokens = subPageContent.Length / ApproxCharsPerToken;
                
                if (currentTokenCount + subPageTokens > TokenLimit)
                {
                    Console.WriteLine($"Skipping {link} to stay within token limit.");
                    break;
                }

                userPrompt.Append(subPageContent);
                currentTokenCount += subPageTokens;
            }
            
            string finalUserPrompt = userPrompt.ToString();
            if (finalUserPrompt.Length > maxAllowedChars)
            {
                finalUserPrompt = finalUserPrompt.Substring(0, maxAllowedChars);
            }

            string markdown = await _openAiService.GetResponse(systemPrompt, finalUserPrompt);
            File.WriteAllText(outputFilePath, markdown);

            return markdown;
        }
    }
}
