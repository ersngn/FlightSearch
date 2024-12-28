using FluentValidation.Results;

namespace FlightSearch.Core.Response;

public class ApiResponse<T> 
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public int StatusCode { get; set; }
    public T? Data { get; set; }
    public List<string>? Errors { get; set; } = new();
    

    public ApiResponse(T data, string message, int statusCode)
    {
        Success = true;
        Data = data;
        StatusCode = statusCode;
        Message = message;
    }

    public ApiResponse(string message, int statusCode, List<string>? errors = null)
    {
        Success = false;
        Data = default;
        StatusCode = statusCode;
        Message = message;
        Errors = errors ?? new List<string>();
    }
    public ApiResponse(string message, int statusCode)
    {
        Success = false;
        Data = default;
        StatusCode = statusCode;
        Message = message;
        Errors = new List<string>();
    }
    
    public static ApiResponse<T> SuccessApiResponse(T data, string message = "Success", int statusCode = 200)
    {
        return new ApiResponse<T>(data, message, statusCode)
        {
            Success = true
        };
    }

    // Success without data
    public static ApiResponse<T> SuccessApiResponse(string message = "Success", int statusCode = 200)
    {
        return new ApiResponse<T>(default, message, statusCode)
        {
            Success = true
        };
    }

    // General error
    public static ApiResponse<T> Error(string message, int statusCode = 500, List<string>? errors = null)
    {
        return new ApiResponse<T>(message, statusCode, errors);
    }

    // Validation error
    public static ApiResponse<T> ValidationError(string message, ValidationResult validationResult)
    {
        var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
        return new ApiResponse<T>(message, 400, errorMessages)
        {
            Success = false
        };
    }
}