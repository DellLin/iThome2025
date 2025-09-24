// 我們的天氣工具
using System.ComponentModel;
using Microsoft.SemanticKernel;

public class WeatherPlugin
{
    [KernelFunction, Description("取得指定城市的天氣")]
    public string GetWeather(
        [Description("城市名稱")] string city)
    {
        // 這裡為了範例簡單，我們回傳假資料
        return $"天氣晴朗，溫度 25 度，很適合出門走走！";
    }
}