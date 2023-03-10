using System.Diagnostics;

namespace CodeNotion.Academy.OrderScheduling;

public class Timer
{
    private readonly Stopwatch _stopwatch = new();
    
    public void StartTime()
    {
        _stopwatch.Start();
    }

    public void EndTime()
    {
        _stopwatch.Stop();
        Console.WriteLine($"Time taken: {_stopwatch.ElapsedMilliseconds}ms");
    }
}