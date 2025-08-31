
using Microsoft.SemanticKernel;
using Microsoft.Extensions.Configuration;

// 1. 廚房的總指揮：建立 Kernel Builder
// 這就像是廚師在設定他的工作檯，準備好所有工具和服務。

// 讀取 user-secrets 設定
var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

var builder = Kernel.CreateBuilder();

// 2. 邀請 AI 服務進駐廚房
// 這裡我們以 OpenAI 為例，但你可以替換成你使用的任何服務。
// API 金鑰從 user-secrets 取得
var apiKey = config["OpenAI:ApiKey"] ?? throw new Exception("請先設定 user-secrets: OpenAI:ApiKey");
builder.AddOpenAIChatCompletion("gpt-5-mini", apiKey);

// 3. 廚師準備就緒：建構 Kernel 實例
// 所有的服務都準備好了，現在我們正式把 Kernel 實體化，這位廚師可以開始工作了！
var kernel = builder.Build();

// 4. 準備食譜 (提示詞)
// 這是我們給廚師的指令，告訴他我們想要什麼樣的料理。
var prompt = "你好，我是一位 AI 工程師，請用幽默的方式跟我打招呼。";

// 5. 廚師開始料理 (呼叫 AI)
// 執行這段程式碼，廚師會根據你的提示詞，去呼叫 AI 服務並得到結果。
Console.WriteLine("AI 思考中...");
var result = await kernel.InvokePromptAsync(prompt);

// 6. 菜上桌囉！
// 把廚師完成的料理呈現出來，也就是 AI 的回應。
Console.WriteLine($"AI 的回應：{result}");
