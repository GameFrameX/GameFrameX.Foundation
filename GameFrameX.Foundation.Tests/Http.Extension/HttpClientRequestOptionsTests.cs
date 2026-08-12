using System.Net;
using System.Text.Json;
using GameFrameX.Foundation.Http.Extension;
using Xunit;

namespace GameFrameX.Foundation.Tests.Http.Extension
{
    /// <summary>
    /// HttpClientRequestOptions / HttpClientRetryOptions / HttpClientRequestException /
    /// 各 LogEntry record 的直接单元测试，不依赖 HttpClient 网络栈。
    /// </summary>
    public sealed class HttpClientRequestOptionsTests
    {
        // ============================================================
        // HttpClientRequestOptions
        // ============================================================

        [Fact]
        public void Defaults_AreConsistentWithDocumentation()
        {
            var options = new HttpClientRequestOptions();

            Assert.Null(options.Timeout);
            Assert.NotNull(options.Headers);
            Assert.Empty(options.Headers);
            Assert.NotNull(options.JsonSerializerOptions);
            Assert.NotNull(options.Retry);
            Assert.Null(options.OnHeader);
            Assert.Null(options.OnRequest);
            Assert.Null(options.OnResponse);
        }

        [Fact]
        public void Timeout_CanBeAssignedAndRetrieved()
        {
            var options = new HttpClientRequestOptions();
            var timeout = TimeSpan.FromSeconds(30);

            options.Timeout = timeout;

            Assert.Equal(timeout, options.Timeout);
        }

        [Fact]
        public void Headers_DictionaryIsMutable()
        {
            var options = new HttpClientRequestOptions();

            options.Headers.Add("Authorization", "Bearer abc");
            options.Headers.Add("X-Trace-Id", "12345");

            Assert.Equal(2, options.Headers.Count);
            Assert.Equal("Bearer abc", options.Headers["Authorization"]);
            Assert.Equal("12345", options.Headers["X-Trace-Id"]);
        }

        [Fact]
        public void JsonSerializerOptions_CanBeReplaced()
        {
            var options = new HttpClientRequestOptions();
            var custom = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            options.JsonSerializerOptions = custom;

            Assert.Same(custom, options.JsonSerializerOptions);
        }

        [Fact]
        public void Hooks_CanBeAssigned()
        {
            Action<HttpClientHeaderResult>? onHeader = _ => { };
            Action<HttpClientRequestLogEntry>? onRequest = _ => { };
            Action<HttpClientResponseLogEntry>? onResponse = _ => { };

            var options = new HttpClientRequestOptions
            {
                OnHeader = onHeader,
                OnRequest = onRequest,
                OnResponse = onResponse,
            };

            Assert.NotNull(options.OnHeader);
            Assert.NotNull(options.OnRequest);
            Assert.NotNull(options.OnResponse);
        }

        // ============================================================
        // HttpClientRetryOptions
        // ============================================================

        [Fact]
        public void RetryDefaults_AreConsistentWithDocumentation()
        {
            var retry = new HttpClientRetryOptions();

            Assert.Equal(0, retry.MaxRetries);
            Assert.Equal(TimeSpan.FromMilliseconds(100), retry.BaseDelay);
            Assert.Equal(2D, retry.BackoffFactor);
            Assert.False(retry.AllowNonIdempotentRetry);
            Assert.Null(retry.OnRetry);
        }

        [Fact]
        public void RetryStatusCodes_ContainsExpectedTransients()
        {
            var retry = new HttpClientRetryOptions();

            Assert.Contains(HttpStatusCode.RequestTimeout, retry.RetryStatusCodes);
            Assert.Contains(HttpStatusCode.TooManyRequests, retry.RetryStatusCodes);
            Assert.Contains(HttpStatusCode.InternalServerError, retry.RetryStatusCodes);
            Assert.Contains(HttpStatusCode.BadGateway, retry.RetryStatusCodes);
            Assert.Contains(HttpStatusCode.ServiceUnavailable, retry.RetryStatusCodes);
            Assert.Contains(HttpStatusCode.GatewayTimeout, retry.RetryStatusCodes);
        }

        [Fact]
        public void RetryStatusCodes_ExcludesNonTransientCodes()
        {
            var retry = new HttpClientRetryOptions();

            Assert.DoesNotContain(HttpStatusCode.OK, retry.RetryStatusCodes);
            Assert.DoesNotContain(HttpStatusCode.NotFound, retry.RetryStatusCodes);
            Assert.DoesNotContain(HttpStatusCode.BadRequest, retry.RetryStatusCodes);
            Assert.DoesNotContain(HttpStatusCode.Unauthorized, retry.RetryStatusCodes);
        }

        [Fact]
        public void RetryStatusCodes_IsMutable()
        {
            var retry = new HttpClientRetryOptions();

            retry.RetryStatusCodes.Add(HttpStatusCode.Conflict);
            retry.RetryStatusCodes.Remove(HttpStatusCode.InternalServerError);

            Assert.Contains(HttpStatusCode.Conflict, retry.RetryStatusCodes);
            Assert.DoesNotContain(HttpStatusCode.InternalServerError, retry.RetryStatusCodes);
        }

        [Fact]
        public void Retry_PropertiesCanBeAssigned()
        {
            var retry = new HttpClientRetryOptions();
            Action<HttpClientRetryLogEntry>? onRetry = _ => { };

            retry.MaxRetries = 5;
            retry.BaseDelay = TimeSpan.FromMilliseconds(250);
            retry.BackoffFactor = 3D;
            retry.AllowNonIdempotentRetry = true;
            retry.OnRetry = onRetry;

            Assert.Equal(5, retry.MaxRetries);
            Assert.Equal(TimeSpan.FromMilliseconds(250), retry.BaseDelay);
            Assert.Equal(3D, retry.BackoffFactor);
            Assert.True(retry.AllowNonIdempotentRetry);
            Assert.NotNull(retry.OnRetry);
        }

        // ============================================================
        // HttpClientHeaderResult
        // ============================================================

        [Fact]
        public void HeaderResult_Success_PreservesValues()
        {
            var result = new HttpClientHeaderResult("X-Custom", "val", true, null);

            Assert.Equal("X-Custom", result.Name);
            Assert.Equal("val", result.Value);
            Assert.True(result.Success);
            Assert.Null(result.FailureReason);
        }

        [Fact]
        public void HeaderResult_Failure_PreservesReason()
        {
            var result = new HttpClientHeaderResult("Bad-Header", "x", false, "rejected");

            Assert.False(result.Success);
            Assert.Equal("rejected", result.FailureReason);
        }

        // ============================================================
        // HttpClientRequestLogEntry
        // ============================================================

        [Fact]
        public void RequestLogEntry_PreservesValues()
        {
            var entry = new HttpClientRequestLogEntry(HttpMethod.Post, new Uri("http://x.com"), 2);

            Assert.Equal(HttpMethod.Post, entry.Method);
            Assert.Equal(new Uri("http://x.com"), entry.RequestUri);
            Assert.Equal(2, entry.Attempt);
        }

        [Fact]
        public void RequestLogEntry_NullUri_IsAllowed()
        {
            var entry = new HttpClientRequestLogEntry(HttpMethod.Get, null, 1);

            Assert.Null(entry.RequestUri);
            Assert.Equal(1, entry.Attempt);
        }

        // ============================================================
        // HttpClientResponseLogEntry
        // ============================================================

        [Fact]
        public void ResponseLogEntry_PreservesValues()
        {
            var entry = new HttpClientResponseLogEntry(
                HttpMethod.Put, new Uri("http://y.com"), 3, HttpStatusCode.Created);

            Assert.Equal(HttpMethod.Put, entry.Method);
            Assert.Equal(new Uri("http://y.com"), entry.RequestUri);
            Assert.Equal(3, entry.Attempt);
            Assert.Equal(HttpStatusCode.Created, entry.StatusCode);
        }

        // ============================================================
        // HttpClientRetryLogEntry
        // ============================================================

        [Fact]
        public void RetryLogEntry_WithStatusCode_PreservesValues()
        {
            var delay = TimeSpan.FromMilliseconds(200);
            var entry = new HttpClientRetryLogEntry(
                HttpMethod.Get, new Uri("http://z.com"), 1, delay, HttpStatusCode.ServiceUnavailable, null);

            Assert.Equal(HttpMethod.Get, entry.Method);
            Assert.Equal(new Uri("http://z.com"), entry.RequestUri);
            Assert.Equal(1, entry.Attempt);
            Assert.Equal(delay, entry.Delay);
            Assert.Equal(HttpStatusCode.ServiceUnavailable, entry.StatusCode);
            Assert.Null(entry.Exception);
        }

        [Fact]
        public void RetryLogEntry_WithException_PreservesValues()
        {
            var exception = new HttpRequestException("network down");
            var entry = new HttpClientRetryLogEntry(
                HttpMethod.Post, null, 2, TimeSpan.FromMilliseconds(400), null, exception);

            Assert.Null(entry.StatusCode);
            Assert.Same(exception, entry.Exception);
        }

        // ============================================================
        // HttpClientRequestException
        // ============================================================

        [Fact]
        public void RequestException_PreservesAllContext()
        {
            var inner = new InvalidOperationException("inner");
            var exception = new HttpClientRequestException(
                "boom",
                HttpStatusCode.InternalServerError,
                new Uri("http://fail.com"),
                "response body snippet",
                inner);

            Assert.Equal("boom", exception.Message);
            Assert.Equal(HttpStatusCode.InternalServerError, exception.StatusCode);
            Assert.Equal(new Uri("http://fail.com"), exception.RequestUri);
            Assert.Equal("response body snippet", exception.ResponseSummary);
            Assert.Same(inner, exception.RawException);
        }

        [Fact]
        public void RequestException_InheritsFromHttpRequestException()
        {
            var exception = new HttpClientRequestException("msg", null, null, null);

            Assert.IsAssignableFrom<HttpRequestException>(exception);
        }

        [Fact]
        public void RequestException_NullableFields_AcceptNull()
        {
            var exception = new HttpClientRequestException("msg", null, null, null, null);

            Assert.Null(exception.StatusCode);
            Assert.Null(exception.RequestUri);
            Assert.Null(exception.ResponseSummary);
            Assert.Null(exception.RawException);
        }

        [Fact]
        public void RequestException_StatusCode_ShadowsBaseWithStrongType()
        {
            var exception = new HttpClientRequestException(
                "msg", HttpStatusCode.BadGateway, null, null);

            Assert.Equal(HttpStatusCode.BadGateway, exception.StatusCode);
            Assert.Equal(HttpStatusCode.BadGateway, ((HttpRequestException)exception).StatusCode);
        }
    }
}
