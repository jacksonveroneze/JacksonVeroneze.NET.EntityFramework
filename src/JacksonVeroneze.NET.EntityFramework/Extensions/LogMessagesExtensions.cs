namespace JacksonVeroneze.NET.EntityFramework.Extensions;

public static partial class LogMessagesExtensions
{
    [LoggerMessage(
        EventId = 1000,
        Level = LogLevel.Information,
        Message = "{className} - {methodName} - Any: '{any}'")]
    public static partial void LogAny(this ILogger logger,
        string className, string methodName,
        bool any);

    [LoggerMessage(
        EventId = 2000,
        Level = LogLevel.Information,
        Message = "{className} - {methodName} - Count: '{count}'")]
    public static partial void LogCount(this ILogger logger,
        string className, string methodName,
        long count);

    [LoggerMessage(
        EventId = 3000,
        Level = LogLevel.Information,
        Message = "{className} - {methodName} - Created")]
    public static partial void LogCreate(this ILogger logger,
        string className, string methodName);

    [LoggerMessage(
        EventId = 4000,
        Level = LogLevel.Information,
        Message = "{className} - {methodName} - Count: '{count}'")]
    public static partial void LogGetAll(this ILogger logger,
        string className, string methodName,
        long count);

    [LoggerMessage(
        EventId = 5000,
        Level = LogLevel.Information,
        Message = "{className} - {methodName} - Id: '{id}' - Found: '{found}'")]
    public static partial void LogGetById(this ILogger logger,
        string className, string methodName,
        object id, bool found);

    [LoggerMessage(
        EventId = 6000,
        Level = LogLevel.Information,
        Message = "{className} - {methodName} - Pagination: '{pagination}'")]
    public static partial void LogGetPaged(this ILogger logger,
        string className, string methodName,
        object pagination);

    [LoggerMessage(
        EventId = 7000,
        Level = LogLevel.Information,
        Message = "{className} - {methodName} - Found: '{found}'")]
    public static partial void LogGetSingleOrDefault(this ILogger logger,
        string className, string methodName,
        bool found);

    [LoggerMessage(
        EventId = 8000,
        Level = LogLevel.Information,
        Message = "{className} - {methodName} - Id: '{id}' - Removed")]
    public static partial void LogRemove(this ILogger logger,
        string className, string methodName,
        object id);


    [LoggerMessage(
        EventId = 9000,
        Level = LogLevel.Information,
        Message = "{className} - {methodName} - Id: '{id}' - Updated")]
    public static partial void LogUpdate(this ILogger logger,
        string className, string methodName,
        object id);
}