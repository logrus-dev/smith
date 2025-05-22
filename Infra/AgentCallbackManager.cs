using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;

namespace Logrus.Smith.Infra;

public class AgentCallbackManager
{
    private record CallbackInfo(Type RequestType, Type ResponseType, Func<object?, Task<object?>> Callback);

    private readonly ConcurrentDictionary<string, CallbackInfo>
        _callbacks = new();
        
        private class CallbackDisposable(Action action): IDisposable
        {
            public void Dispose()
            {
                action();
            }
        }

    public IDisposable RegisterCallback<TRequest, TResponse>(string code, Func<TRequest?, Task<TResponse?>> cb)
    {
        _callbacks[code] = new CallbackInfo(
            typeof(TRequest),
            typeof(TResponse),
            async request => await cb((TRequest?) request));
        return new CallbackDisposable(() => _callbacks.TryRemove(code, out _));
    }

    public async Task<object?> InvokeCallback(string code, HttpRequest request)
    {
        var callbackInfo = _callbacks[code];
        var input = await request.ReadFromJsonAsync(callbackInfo.RequestType);
        var output = await callbackInfo.Callback(input);
        return output;
    }
}
