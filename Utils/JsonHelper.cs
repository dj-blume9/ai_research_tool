using System;
using System.Text.RegularExpressions;

namespace ai_research_app.Utils
{
    public static class JsonHelper
    {
        public static string ExtractJsonFromMarkdown(string content)
        {
            // If OpenAI wraps JSON in ```json ... ```
            if (content.StartsWith("```json"))
            {
                int startIndex = content.IndexOf("```") + 7;
                int endIndex = content.LastIndexOf("```");

                if (endIndex > startIndex)
                {
                    return content.Substring(startIndex, endIndex - startIndex).Trim();
                }
            }

            return content.Trim(); // Ensure valid JSON
        }
    }
}