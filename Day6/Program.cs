using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

// 1. 建立 Kernel Builder
var builder = Kernel.CreateBuilder()
            .AddOpenAIChatClient(
                modelId: "gpt-5-mini",
                apiKey: config["OpenAI:ApiKey"] ?? throw new Exception("請先設定 user-secrets: OpenAI:ApiKey"));

// 2. 建立我們的 Plugin 實例
var lightsPlugin = new LightsPlugin();

// 3. 將 Plugin 加入 Kernel
builder.Plugins.AddFromType<LightsPlugin>();

// 4. 建構 Kernel
var kernel = builder.Build();

Console.WriteLine("🏠 燈光控制範例啟動！");
Console.WriteLine("你可以用自然語言來控制燈光，例如：");
Console.WriteLine("- 請幫我打開燈");
Console.WriteLine("- 關燈");
Console.WriteLine("- 輸入 'quit' 離開\n");

while (true)
{
    Console.Write("💬 請輸入指令: ");
    var userInput = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(userInput))
        continue;

    if (userInput.ToLower() is "quit" or "exit" or "離開")
    {
        Console.WriteLine("👋 再見！");
        break;
    }

    try
    {
        Console.WriteLine();
        OpenAIPromptExecutionSettings settings = new() { FunctionChoiceBehavior = FunctionChoiceBehavior.Auto() };

        // 建立 Kernel Arguments
        var arguments = new KernelArguments(executionSettings: settings)
        {
            ["userInput"] = userInput,
        };

        // 使用更符合 Semantic Kernel 規格的 Prompt
        var prompt = @"
你是一個智慧燈光控制助手。請根據用戶的指令呼叫適當的函式。

用戶指令：{{$userInput}}

請分析用戶的意圖：
- 如果用戶想要打開燈光（如：打開燈、開燈、亮燈、點燈等），請呼叫 TurnOn 函式
- 如果用戶想要關閉燈光（如：關燈、關閉燈、熄燈、關掉燈等），請呼叫 TurnOff 函式

請直接呼叫對應的函式。";

        var result = await kernel.InvokePromptAsync(prompt, arguments);

        Console.WriteLine($"🤖 AI 回應: {result}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ 發生錯誤: {ex.Message}");
    }

    Console.WriteLine();
}

