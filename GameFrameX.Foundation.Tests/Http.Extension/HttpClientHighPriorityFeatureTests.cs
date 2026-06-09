using System.Net;
using System.Text;
using GameFrameX.Foundation.Http.Extension;
using Xunit;

namespace GameFrameX.Foundation.Tests.Http.Extension;

public sealed class HttpClientHighPriorityFeatureTests
{
    private sealed record RequestDto(string Name);

    private sealed record ResponseDto(string Name, int Value);

    private sealed class QueueHttpMessageHandler : HttpMessageHandler
    {
        private readonly Queue<Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>>> _responses = new();

        public List<HttpRequestMessage> Requests { get; } = new();

        public List<string> RequestBodies { get; } = new();

        public int SendCount { get; private set; }

        public void Enqueue(HttpStatusCode statusCode, string content)
        {
            _responses.Enqueue((_, _) => Task.FromResult(CreateResponse(statusCode, content)));
        }

        public void Enqueue(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> response)
        {
            _responses.Enqueue(response);
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            SendCount++;
            Requests.Add(request);
            RequestBodies.Add(request.Content is null ? string.Empty : await request.Content.ReadAsStringAsync(cancellationToken));
            return _responses.Count > 0
                       ? await _responses.Dequeue()(request, cancellationToken)
                       : CreateResponse(HttpStatusCode.OK, "{}");
        }

        private static HttpResponseMessage CreateResponse(HttpStatusCode statusCode, string content)
        {
            return new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(content, Encoding.UTF8, "application/json")
            };
        }
    }

    [Fact]
    public async Task GetJsonAsync_Success_DeserializesTypedResponse()
    {
        var handler = new QueueHttpMessageHandler();
        handler.Enqueue(HttpStatusCode.OK, "{\"name\":\"ok\",\"value\":42}");
        using var client = new HttpClient(handler);

        var result = await client.GetJsonAsync<ResponseDto>("http://example.com/items");

        Assert.Equal(new ResponseDto("ok", 42), result);
        Assert.Equal(HttpMethod.Get, handler.Requests.Single().Method);
    }

    [Fact]
    public async Task PostJsonAsync_Success_SerializesRequestAndDeserializesTypedResponse()
    {
        var handler = new QueueHttpMessageHandler();
        handler.Enqueue(HttpStatusCode.OK, "{\"name\":\"created\",\"value\":7}");
        using var client = new HttpClient(handler);

        var result = await client.PostJsonAsync<RequestDto, ResponseDto>(
                         "http://example.com/items",
                         new RequestDto("input"));

        Assert.Equal(new ResponseDto("created", 7), result);
        Assert.Equal(HttpMethod.Post, handler.Requests.Single().Method);
        Assert.Contains("\"Name\":\"input\"", handler.RequestBodies.Single());
    }

    [Fact]
    public async Task SendJsonAsync_NonSuccessStatus_ThrowsUnifiedExceptionWithContext()
    {
        var handler = new QueueHttpMessageHandler();
        handler.Enqueue(HttpStatusCode.BadGateway, "{\"error\":\"upstream failed\"}");
        using var client = new HttpClient(handler);

        var exception = await Assert.ThrowsAsync<HttpClientRequestException>(() =>
                            client.SendJsonAsync<RequestDto, ResponseDto>(
                                HttpMethod.Post,
                                "http://example.com/items",
                                new RequestDto("input")));

        Assert.Equal(HttpStatusCode.BadGateway, exception.StatusCode);
        Assert.Equal(new Uri("http://example.com/items"), exception.RequestUri);
        Assert.Contains("upstream failed", exception.ResponseSummary);
    }

    [Fact]
    public async Task GetJsonAsync_EmptyResponse_ReturnsDefaultValue()
    {
        var handler = new QueueHttpMessageHandler();
        handler.Enqueue(HttpStatusCode.NoContent, "");
        using var client = new HttpClient(handler);

        var result = await client.GetJsonAsync<ResponseDto>("http://example.com/items");

        Assert.Null(result);
    }

    [Fact]
    public async Task GetJsonAsync_InvalidJson_ThrowsUnifiedExceptionWithRawException()
    {
        var handler = new QueueHttpMessageHandler();
        handler.Enqueue(HttpStatusCode.OK, "not json");
        using var client = new HttpClient(handler);

        var exception = await Assert.ThrowsAsync<HttpClientRequestException>(() =>
                            client.GetJsonAsync<ResponseDto>("http://example.com/items"));

        Assert.IsType<System.Text.Json.JsonException>(exception.RawException);
        Assert.Contains("not json", exception.ResponseSummary);
    }

    [Fact]
    public async Task GetJsonAsync_Retry_RetriesIdempotentMethodWithBackoff()
    {
        var retryDelays = new List<TimeSpan>();
        var handler = new QueueHttpMessageHandler();
        handler.Enqueue(HttpStatusCode.ServiceUnavailable, "{\"error\":\"try again\"}");
        handler.Enqueue(HttpStatusCode.OK, "{\"name\":\"retried\",\"value\":2}");
        using var client = new HttpClient(handler);

        var result = await client.GetJsonAsync<ResponseDto>(
                         "http://example.com/items",
                         new HttpClientRequestOptions
                         {
                             Retry = new HttpClientRetryOptions
                             {
                                 MaxRetries = 1,
                                 BaseDelay = TimeSpan.FromMilliseconds(1),
                                 BackoffFactor = 2,
                                 OnRetry = retry => retryDelays.Add(retry.Delay)
                             }
                         });

        Assert.Equal(new ResponseDto("retried", 2), result);
        Assert.Equal(2, handler.SendCount);
        Assert.Single(retryDelays);
        Assert.Equal(TimeSpan.FromMilliseconds(1), retryDelays[0]);
    }

    [Fact]
    public async Task PostJsonAsync_Retry_DoesNotRetryNonIdempotentMethodByDefault()
    {
        var handler = new QueueHttpMessageHandler();
        handler.Enqueue(HttpStatusCode.ServiceUnavailable, "{\"error\":\"try again\"}");
        handler.Enqueue(HttpStatusCode.OK, "{\"name\":\"retried\",\"value\":2}");
        using var client = new HttpClient(handler);

        await Assert.ThrowsAsync<HttpClientRequestException>(() =>
            client.PostJsonAsync<RequestDto, ResponseDto>(
                "http://example.com/items",
                new RequestDto("input"),
                new HttpClientRequestOptions
                {
                    Retry = new HttpClientRetryOptions
                    {
                        MaxRetries = 1,
                        BaseDelay = TimeSpan.FromMilliseconds(1)
                    }
                }));

        Assert.Equal(1, handler.SendCount);
    }

    [Fact]
    public async Task PostJsonAsync_Retry_RetriesWhenExplicitlyAllowed()
    {
        var handler = new QueueHttpMessageHandler();
        handler.Enqueue(HttpStatusCode.ServiceUnavailable, "{\"error\":\"try again\"}");
        handler.Enqueue(HttpStatusCode.OK, "{\"name\":\"retried\",\"value\":2}");
        using var client = new HttpClient(handler);

        var result = await client.PostJsonAsync<RequestDto, ResponseDto>(
                         "http://example.com/items",
                         new RequestDto("input"),
                         new HttpClientRequestOptions
                         {
                             Retry = new HttpClientRetryOptions
                             {
                                 MaxRetries = 1,
                                 BaseDelay = TimeSpan.FromMilliseconds(1),
                                 AllowNonIdempotentRetry = true
                             }
                         });

        Assert.Equal(new ResponseDto("retried", 2), result);
        Assert.Equal(2, handler.SendCount);
    }

    [Fact]
    public async Task GetJsonAsync_Retry_StopsDuringCancellation()
    {
        using var cts = new CancellationTokenSource();
        var handler = new QueueHttpMessageHandler();
        handler.Enqueue(HttpStatusCode.ServiceUnavailable, "{\"error\":\"try again\"}");
        using var client = new HttpClient(handler);

        await Assert.ThrowsAsync<TaskCanceledException>(async () =>
        {
            var task = client.GetJsonAsync<ResponseDto>(
                "http://example.com/items",
                new HttpClientRequestOptions
                {
                    Retry = new HttpClientRetryOptions
                    {
                        MaxRetries = 3,
                        BaseDelay = TimeSpan.FromSeconds(5)
                    }
                },
                cts.Token);
            await cts.CancelAsync();
            await task;
        });

        Assert.Equal(1, handler.SendCount);
    }

    [Fact]
    public async Task GetJsonAsync_InvalidHeader_ReportsHeaderFailure()
    {
        var headerResults = new List<HttpClientHeaderResult>();
        var handler = new QueueHttpMessageHandler();
        handler.Enqueue(HttpStatusCode.OK, "{\"name\":\"ok\",\"value\":1}");
        using var client = new HttpClient(handler);

        await client.GetJsonAsync<ResponseDto>(
            "http://example.com/items",
            new HttpClientRequestOptions
            {
                Headers = { ["Content-Type"] = "application/json" },
                OnHeader = headerResults.Add
            });

        var result = Assert.Single(headerResults);
        Assert.Equal("Content-Type", result.Name);
        Assert.False(result.Success);
    }
}
