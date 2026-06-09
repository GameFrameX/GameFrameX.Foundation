using System.Net;
using System.Text.Json;
using GameFrameX.Foundation.Json;

namespace GameFrameX.Foundation.Http.Extension;

/// <summary>
/// Configures typed HTTP JSON requests, including timeout, headers, JSON options, retry, and logging hooks.
/// </summary>
public sealed class HttpClientRequestOptions
{
    /// <summary>
    /// Gets or sets the per-request timeout. When null, the <see cref="HttpClient"/> timeout behavior is used.
    /// </summary>
    public TimeSpan? Timeout { get; set; }

    /// <summary>
    /// Gets the headers that should be added to the outgoing request.
    /// </summary>
    public IDictionary<string, string> Headers { get; } = new Dictionary<string, string>();

    /// <summary>
    /// Gets or sets the JSON serializer options used for request and response bodies.
    /// </summary>
    public JsonSerializerOptions JsonSerializerOptions { get; set; } = JsonHelper.DefaultOptions;

    /// <summary>
    /// Gets or sets the retry policy for the request.
    /// </summary>
    public HttpClientRetryOptions Retry { get; set; } = new();

    /// <summary>
    /// Gets or sets a hook that receives the result of each header add operation.
    /// </summary>
    public Action<HttpClientHeaderResult>? OnHeader { get; set; }

    /// <summary>
    /// Gets or sets a hook invoked before each send attempt.
    /// </summary>
    public Action<HttpClientRequestLogEntry>? OnRequest { get; set; }

    /// <summary>
    /// Gets or sets a hook invoked after each received response.
    /// </summary>
    public Action<HttpClientResponseLogEntry>? OnResponse { get; set; }
}

/// <summary>
/// Configures retry behavior for typed HTTP JSON requests.
/// </summary>
public sealed class HttpClientRetryOptions
{
    /// <summary>
    /// Gets or sets the number of retries after the first attempt.
    /// </summary>
    public int MaxRetries { get; set; }

    /// <summary>
    /// Gets or sets the first retry delay.
    /// </summary>
    public TimeSpan BaseDelay { get; set; } = TimeSpan.FromMilliseconds(100);

    /// <summary>
    /// Gets or sets the exponential backoff multiplier.
    /// </summary>
    public double BackoffFactor { get; set; } = 2D;

    /// <summary>
    /// Gets or sets whether POST and PATCH requests can be retried.
    /// </summary>
    public bool AllowNonIdempotentRetry { get; set; }

    /// <summary>
    /// Gets the HTTP status codes that should trigger a retry.
    /// </summary>
    public ISet<HttpStatusCode> RetryStatusCodes { get; } = new HashSet<HttpStatusCode>
    {
        HttpStatusCode.RequestTimeout,
        HttpStatusCode.TooManyRequests,
        HttpStatusCode.InternalServerError,
        HttpStatusCode.BadGateway,
        HttpStatusCode.ServiceUnavailable,
        HttpStatusCode.GatewayTimeout,
    };

    /// <summary>
    /// Gets or sets a hook invoked immediately before a retry delay.
    /// </summary>
    public Action<HttpClientRetryLogEntry>? OnRetry { get; set; }
}

/// <summary>
/// Represents the result of adding a single header to a request.
/// </summary>
/// <param name="Name">The header name.</param>
/// <param name="Value">The header value.</param>
/// <param name="Success">Whether the header was accepted.</param>
/// <param name="FailureReason">The failure reason when the header was rejected.</param>
public sealed record HttpClientHeaderResult(string Name, string Value, bool Success, string? FailureReason);

/// <summary>
/// Represents an outgoing request attempt.
/// </summary>
/// <param name="Method">The HTTP method.</param>
/// <param name="RequestUri">The request URI.</param>
/// <param name="Attempt">The one-based attempt number.</param>
public sealed record HttpClientRequestLogEntry(HttpMethod Method, Uri? RequestUri, int Attempt);

/// <summary>
/// Represents a received response attempt.
/// </summary>
/// <param name="Method">The HTTP method.</param>
/// <param name="RequestUri">The request URI.</param>
/// <param name="Attempt">The one-based attempt number.</param>
/// <param name="StatusCode">The response status code.</param>
public sealed record HttpClientResponseLogEntry(HttpMethod Method, Uri? RequestUri, int Attempt, HttpStatusCode StatusCode);

/// <summary>
/// Represents a scheduled retry attempt.
/// </summary>
/// <param name="Method">The HTTP method.</param>
/// <param name="RequestUri">The request URI.</param>
/// <param name="Attempt">The failed one-based attempt number.</param>
/// <param name="Delay">The delay before the next attempt.</param>
/// <param name="StatusCode">The status code that triggered retry, when available.</param>
/// <param name="Exception">The exception that triggered retry, when available.</param>
public sealed record HttpClientRetryLogEntry(HttpMethod Method, Uri? RequestUri, int Attempt, TimeSpan Delay, HttpStatusCode? StatusCode, Exception? Exception);
