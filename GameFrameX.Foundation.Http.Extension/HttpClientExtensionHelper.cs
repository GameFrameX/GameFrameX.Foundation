namespace GameFrameX.Foundation.Http.Extension;

internal static class HttpClientExtensionHelper
{
    public static CancellationTokenSource CreateTimeoutTokenSource(int timeoutSeconds, CancellationToken cancellationToken)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(timeoutSeconds, nameof(timeoutSeconds));

        var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromSeconds(timeoutSeconds));
        return cts;
    }

    public static async Task<Stream> ReadResponseStreamAsync(HttpResponseMessage response, CancellationToken cancellationToken, IDisposable? owner = null)
    {
        try
        {
            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            return new ResponseMessageStream(response, stream, owner);
        }
        catch
        {
            owner?.Dispose();
            response.Dispose();
            throw;
        }
    }

    private sealed class ResponseMessageStream : Stream
    {
        private readonly HttpResponseMessage _response;
        private readonly Stream _inner;
        private readonly IDisposable? _owner;

        public ResponseMessageStream(HttpResponseMessage response, Stream inner, IDisposable? owner)
        {
            _response = response;
            _inner = inner;
            _owner = owner;
        }

        public override bool CanRead => _inner.CanRead;

        public override bool CanSeek => _inner.CanSeek;

        public override bool CanWrite => _inner.CanWrite;

        public override long Length => _inner.Length;

        public override long Position
        {
            get => _inner.Position;
            set => _inner.Position = value;
        }

        public override void Flush()
        {
            _inner.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _inner.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _inner.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _inner.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _inner.Write(buffer, offset, count);
        }

        public override async ValueTask DisposeAsync()
        {
            try
            {
                await _inner.DisposeAsync();
            }
            finally
            {
                _owner?.Dispose();
                _response.Dispose();
                await base.DisposeAsync();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _inner.Dispose();
                _owner?.Dispose();
                _response.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}