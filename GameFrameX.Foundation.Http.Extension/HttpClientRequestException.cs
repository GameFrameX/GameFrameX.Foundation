using System.Net;

namespace GameFrameX.Foundation.Http.Extension;

/// <summary>
/// Represents a failed typed HTTP request with normalized context for diagnostics.
/// </summary>
public sealed class HttpClientRequestException : HttpRequestException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HttpClientRequestException"/> class.
    /// </summary>
    public HttpClientRequestException(
        string message,
        HttpStatusCode? statusCode,
        Uri? requestUri,
        string? responseSummary,
        Exception? rawException = null)
        : base(message, rawException, statusCode)
    {
        StatusCode = statusCode;
        RequestUri = requestUri;
        ResponseSummary = responseSummary;
        RawException = rawException;
    }

    /// <summary>
    /// Gets the response status code when a response was received.
    /// </summary>
    public new HttpStatusCode? StatusCode { get; }

    /// <summary>
    /// Gets the request URI associated with the failure.
    /// </summary>
    public Uri? RequestUri { get; }

    /// <summary>
    /// Gets a bounded response body summary when one is available.
    /// </summary>
    public string? ResponseSummary { get; }

    /// <summary>
    /// Gets the original exception that caused this failure when one is available.
    /// </summary>
    public Exception? RawException { get; }
}
