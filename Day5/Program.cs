using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;

// 初始化共用配置
var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

var kernel = Kernel.CreateBuilder()
            .AddOpenAIChatClient(
                modelId: "gpt-5-mini",
                apiKey: config["OpenAI:ApiKey"] ?? throw new Exception("請先設定 user-secrets: OpenAI:ApiKey"))
            .Build();

var prompt = kernel.CreateFunctionFromPrompt("請將以下文字翻譯成繁體中文：\n\n{{$input}}");

var result = await kernel.InvokeAsync(prompt, new() { ["input"] = "Hello world, this is a sunny day." });
Console.WriteLine(result);

var jokeFunction = kernel.CreateFunctionFromPromptYaml(File.ReadAllText("joke.prompt.yaml"));

result = await kernel.InvokeAsync(jokeFunction, arguments: new() { { "input", "IT工程師" } });
Console.WriteLine(result);

