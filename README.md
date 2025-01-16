# GameFrameX.Foundation

GameFrameX 的基建库, 提供了一些基础的扩展方法和工具类.

## GameFrameX.Foundation.Http.Normalization(HTTP 消息结构标准化组件)

该组件提供了 HTTP 消息结构标准化的功能, 让消息的格式更加统一.

服务器返回的消息包含 `code` 和 `message` 和 `data`, 但是客户端需要统一的返回格式, 需要进行格式化.所以这个组件提供了格式化的功能. 适用于GameFrameX 的整个生态标准
