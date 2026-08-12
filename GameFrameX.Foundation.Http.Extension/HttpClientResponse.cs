using System.Net;
using System.Text.Json;

namespace GameFrameX.Foundation.Http.Extension;

/// <summary>
/// Represents the raw HTTP response of a typed request, exposing the status code, body, and headers
/// without forcing deserialization. Non-success status codes do not throw; callers decide via
/// <see cref="EnsureSuccessStatusCode"/> or by inspecting <see cref="IsSuccessStatusCode"/>.
/// </summary>
/// <remarks>
/// Shares the same timeout / headers / retry / logging-hook infrastructure as the typed JSON methods
/// (<see cref="HttpClientJsonExtension"/>), but returns the response as-is so callers can inspect error
/// bodies, branch on status codes, or deserialize lazily via <see cref="As{T}"/>.
/// </remarks>
public sealed class HttpClientResponse
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    internal HttpClientResponse(
        HttpMethod method,
        Uri? requestUri,
        HttpStatusCode statusCode,
        bool isSuccessStatusCode,
        int attempt,
        string body,
        IReadOnlyDictionary<string, IEnumerable<string>> headers,
        JsonSerializerOptions jsonSerializerOptions)
    {
        Method = method;
        RequestUri = requestUri;
        StatusCode = statusCode;
        IsSuccessStatusCode = isSuccessStatusCode;
        Attempt = attempt;
        Body = body;
        Headers = headers;
        _jsonSerializerOptions = jsonSerializerOptions;
    }

    /// <summary>The HTTP method of the request.</summary>
    public HttpMethod Method { get; }

    /// <summary>The request URI.</summary>
    public Uri? RequestUri { get; }

    /// <summary>The response status code.</summary>
    public HttpStatusCode StatusCode { get; }

    /// <summary>Whether the response status code indicates success (2xx).</summary>
    public bool IsSuccessStatusCode { get; }

    /// <summary>The one-based attempt number that produced this response (1 when no retry occurred).</summary>
    public int Attempt { get; }

    /// <summary>The raw response body, or an empty string when the response had no body.</summary>
    public string Body { get; }

    /// <summary>The response headers merged with content headers; safe to read after the response is disposed.</summary>
    public IReadOnlyDictionary<string, IEnumerable<string>> Headers { get; }

    /// <summary>
    /// Deserializes the response body as JSON into <typeparamref name="T"/> using the options associated
    /// with the request, or <paramref name="options"/> when provided. Returns default when the body is empty.
    /// Throws <see cref="JsonException"/> when the body cannot be parsed.
    /// </summary>
    public T? As<T>(JsonSerializerOptions? options = null)
    {
        if (string.IsNullOrWhiteSpace(Body))
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(Body, options ?? _jsonSerializerOptions);
    }

    /// <summary>
    /// Returns this response when <see cref="IsSuccessStatusCode"/> is true; otherwise throws an
    /// <see cref="HttpClientRequestException"/> carrying the status code, request URI, and response body.
    /// </summary>
    public HttpClientResponse EnsureSuccessStatusCode()
    {
        if (IsSuccessStatusCode)
        {
            return this;
        }

        throw new HttpClientRequestException(
            $"HTTP request failed with status code {(int)StatusCode} ({StatusCode}) for {RequestUri}.",
            StatusCode,
            RequestUri,
            Body);
    }
}

/// <summary>
/// Internal snapshot of a single HTTP attempt, used to drive both JSON deserialization and raw response paths
/// from a single shared retry loop.
/// </summary>
internal sealed record RawResponse(
    HttpMethod Method,
    Uri? RequestUri,
    HttpStatusCode StatusCode,
    bool IsSuccessStatusCode,
    int Attempt,
    string Body,
    IReadOnlyDictionary<string, IEnumerable<string>> Headers)
{
    public HttpClientResponse ToResponse(JsonSerializerOptions jsonSerializerOptions) =>
        new HttpClientResponse(
            Method,
            RequestUri,
            StatusCode,
            IsSuccessStatusCode,
            Attempt,
            Body,
            Headers,
            jsonSerializerOptions);
}
