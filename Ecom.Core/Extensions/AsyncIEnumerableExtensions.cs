namespace Ecom.Core.Extensions;

public static class AsyncIEnumerableExtensions
{
    /// <summary>
    /// Projects each element of an async-enumerable sequence into a new form by applying
    /// an asynchronous selector function to each member of the source sequence and awaiting
    /// the result.
    /// </summary>
    /// <typeparam name="TSource"> The type of the elements in the source sequence</typeparam>
    /// <typeparam name="TResult">
    /// The type of the elements in the result sequence, obtained by running the selector
    /// function for each element in the source sequence and awaiting the result.
    /// </typeparam>
    /// <param name="source">A sequence of elements to invoke a transform function on</param>
    /// <param name="predicate">An asynchronous transform function to apply to each source element</param>
    /// <returns>
    /// An async-enumerable sequence whose elements are the result of invoking the transform
    /// function on each element of the source sequence and awaiting the result
    /// </returns>
    public static IAsyncEnumerable<TResult> SelectAwait<TSource, TResult>(this IEnumerable<TSource> source,
          Func<TSource, ValueTask<TResult>> predicate)
    {
        return source.ToAsyncEnumerable().SelectAwait(predicate);
    }

    /// <summary>
    /// Returns the first element of an async-enumerable sequence that satisfies the
    /// condition in the predicate, or a default value if no element satisfies the condition
    /// in the predicate
    /// </summary>
    /// <typeparam name="TSource">The type of element in the sequence</typeparam>
    /// <param name="source">Source sequence</param>
    /// <param name="predicate">An asynchronous predicate to invoke and await on each element of the sequence</param>
    /// <returns>
    /// A Task containing the first element in the sequence that satisfies the predicate,
    /// or a default value if no element satisfies the predicate
    /// </returns>
    /// <returns>A task that represents the asynchronous operation</returns>
    public static Task<TSource> FirstOrDefaultAwaitAsync<TSource>(this IEnumerable<TSource> source,
        Func<TSource, ValueTask<bool>> predicate)
    {
        return source.ToAsyncEnumerable().FirstOrDefaultAwaitAsync(predicate).AsTask();
    }

    /// <summary>
    /// Filters the elements of an async-enumerable sequence based on an asynchronous
    /// predicate
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source">An async-enumerable sequence whose elements to filter</param>
    /// <param name="predicate">An asynchronous predicate to test each source element for a condition</param>
    /// <returns>
    /// An async-enumerable sequence that contains elements from the input sequence that
    /// satisfy the condition
    /// </returns>
    public static IAsyncEnumerable<TSource> WhereAwait<TSource>(this IEnumerable<TSource> source,
        Func<TSource, ValueTask<bool>> predicate)
    {
        return source.ToAsyncEnumerable().WhereAwait(predicate);
    }

    /// <summary>
    /// Creates a list from an async-enumerable sequence
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence</typeparam>
    /// <param name="source">The source async-enumerable sequence to get a list of elements for</param>
    /// <returns>
    /// An async-enumerable sequence containing a single element with a list containing
    /// all the elements of the source sequence
    /// </returns>
    /// <returns>A task that represents the asynchronous operation</returns>
    public static Task<List<TSource>> ToListAsync<TSource>(this IEnumerable<TSource> source)
    {
        return source.ToAsyncEnumerable().ToListAsync().AsTask();
    }
}
