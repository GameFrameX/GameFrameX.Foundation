using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace GameFrameX.Foundation.Http.Extension;

/// <summary>
/// Provides typed JSON request extension methods for <see cref="HttpClient"/>.
/// </summary>
public static class HttpClientJsonExtension
{
    private const int ResponseSummaryLimit = 1024;

    /// <summary>
    /// Sends an HTTP request without a JSON body and deserializes the JSON response.
    /// </summary>
    public static Task<TResponse?> SendJsonAsync<TResponse>(
        this HttpClient httpClient,
        HttpMethod method,
        string url,
        HttpClientRequestOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentNullException.ThrowIfNull(method, nameof(method));
        ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));

        options ??= new HttpClientRequestOptions();
        return SendJsonCoreAsync<TResponse>(
            httpClient,
            method,
            url,
            options,
            () => new HttpRequestMessage(method, url),
            cancellationToken);
    }

    /// <summary>
    /// Sends an HTTP request with a JSON body and deserializes the JSON response.
    /// </summary>
    public static Task<TResponse?> SendJsonAsync<TRequest, TResponse>(
        this HttpClient httpClient,
        HttpMethod method,
        string url,
        TRequest request,
        HttpClientRequestOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient, nameof(httpClient));
        ArgumentNullException.ThrowIfNull(method, nameof(method));
        ArgumentException.ThrowIfNullOrWhiteSpace(url, nameof(url));

        options ??= new HttpClientRequestOptions();
        return SendJsonCoreAsync<TResponse>(
            httpClient,
            method,
            url,
            options,
            () => new HttpRequestMessage(method, url)
            {
                Content = JsonContent.Create(request, options: options.JsonSerializerOptions)
            },
            cancellationToken);
    }

    /// <summary>
    /// Sends a GET request and deserializes the JSON response.
    /// </summary>
    public static Task<TResponse?> GetJsonAsync<TResponse>(
        this HttpClient httpClient,
        string url,
        HttpClientRequestOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        return httpClient.SendJsonAsync<TResponse>(HttpMethod.Get, url, options, cancellationToken);
    }

    /// <summary>
    /// Sends a POST request with a JSON body and deserializes the JSON response.
    /// </summary>
    public static Task<TResponse?> PostJsonAsync<TRequest, TResponse>(
        this HttpClient httpClient,
        string url,
        TRequest request,
        HttpClientRequestOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        return httpClient.SendJsonAsync<TRequest, TResponse>(HttpMethod.Post, url, request, options, cancellationToken);
    }

    /// <summary>
    /// Sends a PUT request with a JSON body and deserializes the JSON response.
    /// </summary>
    public static Task<TResponse?> PutJsonAsync<TRequest, TResponse>(
        this HttpClient httpClient,
        string url,
        TRequest request,
        HttpClientRequestOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        return httpClient.SendJsonAsync<TRequest, TResponse>(HttpMethod.Put, url, request, options, cancellationToken);
    }

    /// <summary>
    /// Sends a PATCH request with a JSON body and deserializes the JSON response.
    /// </summary>
    public static Task<TResponse?> PatchJsonAsync<TRequest, TResponse>(
        this HttpClient httpClient,
        string url,
        TRequest request,
        HttpClientRequestOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        return httpClient.SendJsonAsync<TRequest, TResponse>(HttpMethod.Patch, url, request, options, cancellationToken);
    }

    /// <summary>
    /// Sends a DELETE request and deserializes the JSON response.
    /// </summary>
    public static Task<TResponse?> DeleteJsonAsync<TResponse>(
        this HttpClient httpClient,
        string url,
        HttpClientRequestOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        return httpClient.SendJsonAsync<TResponse>(HttpMethod.Delete, url, options, cancellationToken);
    }

    /// <summary>
    /// Sends an OPTIONS request and deserializes the JSON response.
    /// </summary>
    public static Task<TResponse?> OptionsJsonAsync<TResponse>(
        this HttpClient httpClient,
        string url,
        HttpClientRequestOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        return httpClient.SendJsonAsync<TResponse>(HttpMethod.Options, url, options, cancellationToken);
    }

    private static async Task<TResponse?> SendJsonCoreAsync<TResponse>(
        HttpClient httpClient,
        HttpMethod method,
        string url,
        HttpClientRequestOptions options,
        Func<HttpRequestMessage> requestFactory,
        CancellationToken cancellationToken)
    {
        using var timeoutCts = options.Timeout.HasValue
                                   ? CancellationTokenSource.CreateLinkedTokenSource(cancellationToken)
                                   : null;
        if (timeoutCts is not null)
        {
            timeoutCts.CancelAfter(options.Timeout.GetValueOrDefault());
        }

        var effectiveToken = timeoutCts?.Token ?? cancellationToken;
        var allowRetry = options.Retry.MaxRetries > 0 &&
                         (IsIdempotent(method) || options.Retry.AllowNonIdempotentRetry);
        var attempt = 0;

        while (true)
        {
            attempt++;
            using var request = requestFactory();
            AddHeaders(request, options);
            options.OnRequest?.Invoke(new HttpClientRequestLogEntry(method, request.RequestUri, attempt));

            try
            {
                using var response = await httpClient.SendAsync(request, effectiveToken);
                options.OnResponse?.Invoke(new HttpClientResponseLogEntry(method, request.RequestUri, attempt, response.StatusCode));

                if (allowRetry && attempt <= options.Retry.MaxRetries && ShouldRetryStatus(response.StatusCode, options.Retry))
                {
                    await DelayBeforeRetryAsync(method, request.RequestUri, attempt, response.StatusCode, null, options, effectiveToken);
                    continue;
                }

                var responseText = await response.Content.ReadAsStringAsync(effectiveToken);
                if (!response.IsSuccessStatusCode)
                {
                    throw CreateStatusException(response.StatusCode, request.RequestUri, responseText);
                }

                return DeserializeResponse<TResponse>(responseText, request.RequestUri, options);
            }
            catch (OperationCanceledException) when (effectiveToken.IsCancellationRequested)
            {
                throw;
            }
            catch (HttpClientRequestException)
            {
                throw;
            }
            catch (HttpRequestException exception) when (allowRetry && attempt <= options.Retry.MaxRetries)
            {
                await DelayBeforeRetryAsync(method, request.RequestUri, attempt, null, exception, options, effectiveToken);
            }
            catch (Exception exception) when (exception is HttpRequestException or JsonException)
            {
                throw new HttpClientRequestException(
                    $"HTTP JSON request failed for {method} {request.RequestUri}.",
                    null,
                    request.RequestUri,
                    null,
                    exception);
            }
        }
    }

    private static void AddHeaders(HttpRequestMessage request, HttpClientRequestOptions options)
    {
        foreach (var header in options.Headers)
        {
            try
            {
                var success = request.Headers.TryAddWithoutValidation(header.Key, header.Value);
                if (!success && request.Content is not null)
                {
                    success = request.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }

                options.OnHeader?.Invoke(new HttpClientHeaderResult(
                    header.Key,
                    header.Value,
                    success,
                    success ? null : "Header was rejected by request and content header collections."));
            }
            catch (Exception exception)
            {
                options.OnHeader?.Invoke(new HttpClientHeaderResult(header.Key, header.Value, false, exception.Message));
            }
        }
    }

    private static TResponse? DeserializeResponse<TResponse>(
        string responseText,
        Uri? requestUri,
        HttpClientRequestOptions options)
    {
        if (string.IsNullOrWhiteSpace(responseText))
        {
            return default;
        }

        try
        {
            return JsonSerializer.Deserialize<TResponse>(responseText, options.JsonSerializerOptions);
        }
        catch (JsonException exception)
        {
            throw new HttpClientRequestException(
                $"HTTP JSON response could not be deserialized for {requestUri}.",
                null,
                requestUri,
                Summarize(responseText),
                exception);
        }
    }

    private static HttpClientRequestException CreateStatusException(
        HttpStatusCode statusCode,
        Uri? requestUri,
        string responseText)
    {
        return new HttpClientRequestException(
            $"HTTP request failed with status code {(int)statusCode} ({statusCode}) for {requestUri}.",
            statusCode,
            requestUri,
            Summarize(responseText));
    }

    private static async Task DelayBeforeRetryAsync(
        HttpMethod method,
        Uri? requestUri,
        int attempt,
        HttpStatusCode? statusCode,
        Exception? exception,
        HttpClientRequestOptions options,
        CancellationToken cancellationToken)
    {
        var delay = GetDelay(options.Retry, attempt);
        options.Retry.OnRetry?.Invoke(new HttpClientRetryLogEntry(method, requestUri, attempt, delay, statusCode, exception));
        await Task.Delay(delay, cancellationToken);
    }

    private static TimeSpan GetDelay(HttpClientRetryOptions retryOptions, int attempt)
    {
        if (retryOptions.BaseDelay < TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(retryOptions), "Retry base delay cannot be negative.");
        }

        if (retryOptions.BackoffFactor < 1D)
        {
            throw new ArgumentOutOfRangeException(nameof(retryOptions), "Retry backoff factor must be greater than or equal to 1.");
        }

        var multiplier = Math.Pow(retryOptions.BackoffFactor, attempt - 1);
        return TimeSpan.FromMilliseconds(retryOptions.BaseDelay.TotalMilliseconds * multiplier);
    }

    private static bool ShouldRetryStatus(HttpStatusCode statusCode, HttpClientRetryOptions retryOptions)
    {
        return retryOptions.RetryStatusCodes.Contains(statusCode);
    }

    private static bool IsIdempotent(HttpMethod method)
    {
        return method == HttpMethod.Get ||
               method == HttpMethod.Head ||
               method == HttpMethod.Put ||
               method == HttpMethod.Delete ||
               method == HttpMethod.Options;
    }

    private static string? Summarize(string? responseText)
    {
        if (string.IsNullOrEmpty(responseText))
        {
            return responseText;
        }

        return responseText.Length <= ResponseSummaryLimit
                   ? responseText
                   : responseText[..ResponseSummaryLimit];
    }
}
