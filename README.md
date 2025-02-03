# AI Research App

AI Research App is a C# application designed to assist with AI-driven research. It leverages OpenAI's API to generate responses based on user queries, helping streamline research workflows.

## Features
- AI-powered research assistance using OpenAI's API
- User-friendly interface
- C#/.NET-based application

## Prerequisites
Before running this application, ensure you have the following installed:
- [.NET SDK](https://dotnet.microsoft.com/en-us/download) (Latest version)
- An [OpenAI API key](https://openai.com/)
- A `.env` file to store your API key

## Setup Guide

### 1. Get an OpenAI API Key
To use this application, you need to sign up for an OpenAI account and generate an API key.

1. Go to [OpenAI](https://openai.com/)
2. Sign up or log in
3. Navigate to the API section
4. Create and copy your API key

### 2. Clone the Repository
Clone this repository to your local machine:

```bash
git clone https://github.com/dj-blume9/ai-research-app.git
cd ai-research-app
```

### 3. Add Your API Key
The application requires an OpenAI API key stored in a `.env` file.  

#### **Creating a `.env` File**
1. In the root directory of the project, create a new file named `.env`
2. Add the following line to the file:

```plaintext
OPENAI_API_KEY=your_api_key_here
```

> ⚠️ **Important:** Never share your API key or commit it to version control.

### 4. Install Dependencies
Run the following command to restore dependencies:

```bash
dotnet restore
```

### 5. Build and Run the Application
Use the following commands to build and run the project:

```bash
dotnet build
dotnet run
```

### 6. Usage
Once running, enter your research queries in the application, and the AI will generate responses to assist with your research.

## Contributing
Contributions are welcome! Please follow the standard GitHub workflow for pull requests.

## License
This project is licensed under the MIT License.

## Support
For questions or issues, please open an issue in the repository or contact [your-email@example.com].

Happy researching! 📚🤖
