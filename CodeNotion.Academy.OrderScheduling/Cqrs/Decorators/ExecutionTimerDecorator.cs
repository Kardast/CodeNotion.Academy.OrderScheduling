using System.Diagnostics;
using Azure.Core;
using MediatR;

namespace CodeNotion.Academy.OrderScheduling.Cqrs.Decorators;

public class ExecutionTimerDecorator<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly Timer _timer;

    public ExecutionTimerDecorator(Timer timer)
    {
        _timer = timer;
    }

    public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _timer.StartTime();
        var result = next.Invoke();
        _timer.EndTime();
        return result;
    }
}

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