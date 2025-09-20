using System.ComponentModel;
using Microsoft.SemanticKernel;

public class LightsPlugin
{
    // 為你的 Plugin 函式加上註解和描述，讓 AI 知道這個函式是做什麼的
    [KernelFunction, Description("打開燈。")]
    public string TurnOn()
    {
        Console.WriteLine("打開電燈！");
        return "電燈已打開。";
    }

    [KernelFunction, Description("關閉燈。")]
    public string TurnOff()
    {
        Console.WriteLine("關閉電燈！");
        return "電燈已關閉。";
    }
}