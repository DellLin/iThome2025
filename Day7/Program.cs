using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

// 設定設定檔
var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

// 建立 Kernel 和 ChatCompletion 服務
var kernel = Kernel.CreateBuilder()
    .AddOpenAIChatCompletion(
        modelId: "gpt-4o-mini",
        apiKey: config["OpenAI:ApiKey"]!)
    .Build();

var chatService = kernel.GetRequiredService<IChatCompletionService>();

// 建立一個新的對話紀錄
ChatHistory chatHistory = [];

Console.WriteLine("=== 餐廳點餐助理 - ChatHistory 範例 ===\n");

// 1. 加入系統訊息，為 AI 設定角色
chatHistory.AddSystemMessage("你是一位樂於助人的餐廳點餐助理。請記住客戶的偏好和點餐內容，並提供個人化的服務。");

// 2. 加入使用者的問題
Console.WriteLine("🔹 第一輪對話：詢問菜單");
chatHistory.AddUserMessage("菜單上有什麼推薦的嗎？");

var response1 = await chatService.GetChatMessageContentAsync(chatHistory);
chatHistory.Add(response1);

Console.WriteLine($"客戶：菜單上有什麼推薦的嗎？");
Console.WriteLine($"助理：{response1.Content}");
Console.WriteLine();

// 3. 客戶表達偏好
Console.WriteLine("🔹 第二輪對話：表達偏好");
chatHistory.AddUserMessage("我比較喜歡辣的食物，而且我對海鮮過敏。");

var response2 = await chatService.GetChatMessageContentAsync(chatHistory);
chatHistory.Add(response2);

Console.WriteLine($"客戶：我比較喜歡辣的食物，而且我對海鮮過敏。");
Console.WriteLine($"助理：{response2.Content}");
Console.WriteLine();

// 4. 使用者繼續對話，測試記憶功能
Console.WriteLine("🔹 第三輪對話：點餐（測試是否記住偏好）");
chatHistory.AddUserMessage("我想要第一個選項，謝謝。");

var response3 = await chatService.GetChatMessageContentAsync(chatHistory);
chatHistory.Add(response3);

Console.WriteLine($"客戶：我想要第一個選項，謝謝。");
Console.WriteLine($"助理：{response3.Content}");
Console.WriteLine();

// 5. 詢問飲料，測試是否記住客戶偏好
Console.WriteLine("🔹 第四輪對話：飲料推薦（測試記憶功能）");
chatHistory.AddUserMessage("有什麼飲料推薦嗎？");

var response4 = await chatService.GetChatMessageContentAsync(chatHistory);
chatHistory.Add(response4);

Console.WriteLine($"客戶：有什麼飲料推薦嗎？");
Console.WriteLine($"助理：{response4.Content}");
Console.WriteLine();

// 6. 最後確認訂單
Console.WriteLine("🔹 第五輪對話：確認訂單");
chatHistory.AddUserMessage("請幫我確認一下我的訂單內容。");

var response5 = await chatService.GetChatMessageContentAsync(chatHistory);
chatHistory.Add(response5);

Console.WriteLine($"客戶：請幫我確認一下我的訂單內容。");
Console.WriteLine($"助理：{response5.Content}");
Console.WriteLine();

// 顯示完整的對話歷史分析
Console.WriteLine("=== 對話歷史分析 ===");
Console.WriteLine($"總共有 {chatHistory.Count} 則訊息");
Console.WriteLine();

var systemMessages = chatHistory.Where(m => m.Role == AuthorRole.System).Count();
var userMessages = chatHistory.Where(m => m.Role == AuthorRole.User).Count();
var assistantMessages = chatHistory.Where(m => m.Role == AuthorRole.Assistant).Count();

Console.WriteLine($"系統訊息：{systemMessages} 則");
Console.WriteLine($"使用者訊息：{userMessages} 則");
Console.WriteLine($"助理回覆：{assistantMessages} 則");
Console.WriteLine();

Console.WriteLine("=== 完整對話記錄 ===");
for (int i = 0; i < chatHistory.Count; i++)
{
    var message = chatHistory[i];
    Console.WriteLine($"{i + 1}. [{message.Role}] {message.Content}");
    Console.WriteLine();
}