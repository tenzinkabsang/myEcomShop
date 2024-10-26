namespace Ecom.Services;

/// <summary>
/// Consumer interface
/// </summary>
/// <typeparam name="T">Type</typeparam>
public interface IConsumer<T>
{
    Task HandleEventAsync(T eventMessage);
}
