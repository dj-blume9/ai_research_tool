using System;
using System.Threading.Tasks;
using System.CommandLine;
using System.CommandLine.Invocation;
using ai_research_app.Services;
using ai_research_app.Utils;

class Program
{
    static async Task<int> Main(string[] args)
    {
        EnvironmentLoader.Load(".env");

        string? apiKey = EnvironmentLoader.Get("OPENAI_API_KEY");
        if (string.IsNullOrEmpty(apiKey))
        {
            Console.WriteLine("Error: OPENAI_API_KEY is missing or empty.");
            return 0;
        }
        
        // Initialize Services
        var openAiService = new OpenAIService(apiKey);
        var markdownService = new MarkdownService(openAiService);
        
        var rootCommand = new RootCommand("My C# CLI tool");
        var searchArgument = new Argument<string>("search", "Website to be searched");
        
        var searchCommand = new Command("search", "Search Website")
        {
            searchArgument
        };
        
        searchCommand.SetHandler(async (string website) =>
        {
            Console.WriteLine("Fetching website information...");
            string outputFilePath = $"integration_evaluation.md";
            // Generate Markdown file
            string markdownContent = await markdownService.CreateMarkdown(website, outputFilePath);
            Console.WriteLine("\nMarkdown Content Preview:\n");
            Console.WriteLine(markdownContent.Substring(0, Math.Min(markdownContent.Length, 5000)));
            Console.WriteLine("\nMarkdown file created successfully!");
        }, searchArgument);
        
        rootCommand.AddCommand(searchCommand);
        
        return await rootCommand.InvokeAsync(args);
    }
}