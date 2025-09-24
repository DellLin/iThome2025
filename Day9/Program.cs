using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;

// 設定設定檔
var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

// 建立 Kernel
var builder = Kernel.CreateBuilder()
    .AddOpenAIChatCompletion(
        modelId: "gpt-4o-mini",
        apiKey: config["OpenAI:ApiKey"]!);
builder.Plugins.AddFromType<WeatherPlugin>();
var kernel = builder.Build();

// 設定出餐指令為 Auto 模式
// var executionSettings = new OpenAIPromptExecutionSettings
// {
//     FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
// };

// // 從 Kernel 中取得我們要強制使用的函式
// var getWeatherFunction = kernel.Plugins.GetFunction("WeatherPlugin", "GetWeather");
// // 設定出餐指令為 Required 模式，並指定必須使用 getWeatherFunction
// var executionSettings = new OpenAIPromptExecutionSettings
// {
//     FunctionChoiceBehavior = FunctionChoiceBehavior.Required(
//         functions: [getWeatherFunction]
//     )
// };

var executionSettings = new OpenAIPromptExecutionSettings
{
    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
};

// 客人點菜
var result = await kernel.InvokePromptAsync(
    "請問波士頓現在天氣如何？",
    new(executionSettings));

// AI 廚師會自動呼叫 GetWeather 函式
Console.WriteLine(result);
// 輸出: 天氣晴朗，溫度 25 度，很適合出門走走！