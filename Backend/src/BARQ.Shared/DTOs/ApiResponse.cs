namespace BARQ.Shared.DTOs;

/// <summary>
/// Generic API response wrapper
/// </summary>
/// <typeparam name="T">Type of the response data</typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// Indicates if the request was successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Response message
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Response data
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Error details if the request failed
    /// </summary>
    public object? Errors { get; set; }

    /// <summary>
    /// Response metadata
    /// </summary>
    public ResponseMetadata? Metadata { get; set; }

    /// <summary>
    /// Create a successful response
    /// </summary>
    /// <param name="data">Response data</param>
    /// <param name="message">Success message</param>
    /// <returns>Successful API response</returns>
    public static ApiResponse<T> SuccessResponse(T data, string? message = null)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
    }

    /// <summary>
    /// Create an error response
    /// </summary>
    /// <param name="message">Error message</param>
    /// <param name="errors">Error details</param>
    /// <returns>Error API response</returns>
    public static ApiResponse<T> ErrorResponse(string message, object? errors = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Errors = errors
        };
    }
}

/// <summary>
/// API response without data
/// </summary>
public class ApiResponse : ApiResponse<object>
{
    /// <summary>
    /// Create a successful response without data
    /// </summary>
    /// <param name="message">Success message</param>
    /// <returns>Successful API response</returns>
    public static ApiResponse SuccessResponse(string? message = null)
    {
        return new ApiResponse
        {
            Success = true,
            Message = message
        };
    }

    /// <summary>
    /// Create an error response without data
    /// </summary>
    /// <param name="message">Error message</param>
    /// <param name="errors">Error details</param>
    /// <returns>Error API response</returns>
    public static new ApiResponse ErrorResponse(string message, object? errors = null)
    {
        return new ApiResponse
        {
            Success = false,
            Message = message,
            Errors = errors
        };
    }
}

/// <summary>
/// Response metadata for pagination and additional information
/// </summary>
public class ResponseMetadata
{
    /// <summary>
    /// Current page number
    /// </summary>
    public int? Page { get; set; }

    /// <summary>
    /// Page size
    /// </summary>
    public int? PageSize { get; set; }

    /// <summary>
    /// Total number of items
    /// </summary>
    public int? TotalItems { get; set; }

    /// <summary>
    /// Total number of pages
    /// </summary>
    public int? TotalPages { get; set; }

    /// <summary>
    /// Indicates if there is a next page
    /// </summary>
    public bool? HasNextPage { get; set; }

    /// <summary>
    /// Indicates if there is a previous page
    /// </summary>
    public bool? HasPreviousPage { get; set; }

    /// <summary>
    /// Additional metadata as key-value pairs
    /// </summary>
    public Dictionary<string, object>? AdditionalData { get; set; }
}

