# MQTTnetDemo

## _簡介_
若要觀看詳細說明，請至[Wiki][MQTTChatDemoWiki]

MQTTnetDemo是用聊天室來展示如何用`MQTTnet`來進行MQTT基本的`subscribe`和`publish`的功能

PS: 本範例MQTTnet版本為`3.1.1`，而.NET版本為`.NET Core 3.1`

## MQTTBrokerConsole
此為用MQTTnet實作的聊天室server，這邊開始講解如何使用MqttServer

### 初始化MqttServer
要建立MQTT伺服器前，必須先建立MQTT的IMqttServerOptions，建立方法如下
```csharp
var serverOptions = new MqttServerOptionsBuilder()
    .WithConnectionBacklog(100)
    .WithDefaultEndpointPort(1883)
    // 設定連線者的驗證
    .WithConnectionValidator(ValidateConnector)
    // 設定訂閱的攔截事件
    .WithSubscriptionInterceptor(InterCeptSubscription)
    // 設定訊息的攔截事件
    .WithApplicationMessageInterceptor(InterceptMessage)
    .Build();
```
其中`WithConnectionValidator`、`WithSubscriptionInterceptor`、`WithApplicationMessageInterceptor`為非必要，
如果要對客戶端進行驗證，就用WithConnectionValidator

ServerOptions建立好後，再來就要開始準備MqttServer，建立方法如下
```csharp
var mqttServer = new MqttFactory().CreateMqttServer();
```
基本上mqttServer就已經可以直接啟動了，不過MQTTnet還提供了很多Handler來處理客戶端的連線，接下來會開始講解IMqttServer提供的Handler要如何使用

首先IMqttServer提供的Handler總共有以下幾個
```csharp
var mqttServer = new MqttFactory().CreateMqttServer();
// 設定server接收到客戶端發送的訊息的事件
mqttServer.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(OnApplicationMessageReceived);
// 設定客戶端成功連線server的事件
mqttServer.ClientConnectedHandler = new MqttServerClientConnectedHandlerDelegate(OnClientConnected);
// 設定客戶端斷線的事件
mqttServer.ClientDisconnectedHandler = new MqttServerClientDisconnectedHandlerDelegate(OnClientDisconnected);
// 設定客戶端向server訂閱特定Topic的事件
mqttServer.ClientSubscribedTopicHandler = new MqttServerClientSubscribedTopicHandlerDelegate(OnTopicSubscribe);
// 客戶端向server取消訂閱特定Topic的事件
mqttServer.ClientUnsubscribedTopicHandler = new MqttServerClientUnsubscribedTopicHandlerDelegate(OnTopicUnsubscribe);
```
#### ApplicationMessageReceivedHandler
ApplicationMessageReceivedHandler是一個MqttApplicationMessageReceivedHandlerDelegate，是用來處理客戶端發送的訊息，
其delegate的method如下
```csharp
/// <summary>
/// 處理server接收到客戶端發送的訊息
/// </summary>
/// <param name="e"></param>
private void OnApplicationMessageReceived(MqttApplicationMessageReceivedEventArgs e)
{
    // 客戶針對的Topic
    string topic = e.ApplicationMessage.Topic;
    // Payload是客戶端發送過來的訊息，為byte[]，請依照自己的需求轉換
    string message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload ?? new byte[0]);
    // TODO 看你想要對訊息做甚麼
}
```
使用者傳送的訊息都在`e.ApplicationMessage`內，`Topic`跟`Payload`都在裡面

#### ClientConnectedHandler
ClientConnectedHandler是一個MqttServerClientConnectedHandlerDelegate，用來處理客戶端成功連線server的事件，
其delegate的method如下
```csharp
/// <summary>
/// 客戶端成功連線server的事件
/// </summary>
/// <param name="e"></param>
private void OnClientConnected(MqttServerClientConnectedEventArgs e)
{
    Console.WriteLine($"客戶端: { e.ClientId } 已連接!");
    // TODO 看你想要做甚麼
}
```

#### ClientDisconnectedHandler
ClientDisconnectedHandler是一個MqttServerClientDisconnectedHandlerDelegate，用來處理客戶端斷線的事件，
其delegate的method如下
```csharp
/// <summary>
/// 客戶端斷線的事件
/// </summary>
/// <param name="e"></param>
private void OnClientDisconnected(MqttServerClientDisconnectedEventArgs e)
{
    Console.WriteLine($"客戶端: { e.ClientId } 已離線!");
    // TODO 看你想要做甚麼
}
```

#### ClientSubscribedTopicHandler
ClientSubscribedTopicHandler是一個MqttServerClientSubscribedTopicHandlerDelegate，用來處理客戶端向server訂閱特定Topic的事件，
其delegate的method如下
```csharp
/// <summary>
/// 客戶端向server訂閱特定Topic
/// </summary>
/// <param name="e"></param>
private void OnTopicSubscribe(MqttServerClientSubscribedTopicEventArgs e)
{
    Console.WriteLine($"客戶端: { e.ClientId } 已訂閱「{ e.TopicFilter.Topic }」!");
    // TODO 看你想要做甚麼
}
```

#### ClientUnsubscribedTopicHandler
ClientUnsubscribedTopicHandler是一個MqttServerClientUnsubscribedTopicHandlerDelegate，用來處理客戶端向server取消訂閱特定Topic的事件，
其delegate的method如下
```csharp
/// <summary>
/// 客戶端向server取消訂閱特定Topic
/// </summary>
/// <param name="e"></param>
private void OnTopicUnsubscribe(MqttServerClientUnsubscribedTopicEventArgs e)
{
    Console.WriteLine($"客戶端: { e.ClientId } 已取消訂閱「{ e.TopicFilter }」!");
    // TODO 看你想要做甚麼
}
```

### 啟動Server
在serverOptions以及handler都設定好後，就可以啟動server了
```csharp
await mqttServer.StartAsync(serverOptions);
```
到這裡server就大功告成了!

### 實作
詳細實作方式請參閱[MQTTChatHandler.cs][MQTTChatHandler]

## MQTTChatClient
此為用MQTTnet實作的客戶端聊天室Winform視窗，這邊講解MQTTChatClient的核心部分`MQTTChatClientHandler.cs`的實作方法

### 初始化MqttClient
要建立MQTT客戶端前，必須先建立MQTT的IMqttClientOptions，建立方法如下
```csharp
var options = new MqttClientOptionsBuilder()
    .WithTcpServer(ip, port)
    .WithCredentials(userName, password)
    // ClientId不給的話，系統會自動生成
    // PS: 所有連線的ClientId要一致，才有辦法收到離線的訊息
    .WithClientId(userName)
    .WithCleanSession(true)
    .WithCommunicationTimeout(TimeSpan.FromSeconds(2))
    .Build();
```

ClientOptions建立好後，再來就要開始準備MqttClient，建立方法如下
```csharp
var mqttClient = new MqttFactory().CreateMqttClient();
```
基本上mqttClient就已經可以直接啟動了，不過MQTTnet還提供了很多Handler來處理連線，接下來會開始講解IMqttClient提供的Handler要如何使用

首先IMqttClient提供的Handler總共有以下幾個
```csharp
mqttClient.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(OnMessageReceived);
mqttClient.ConnectedHandler = new MqttClientConnectedHandlerDelegate(OnClientConnected);
mqttClient.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(OnClientDisconnected);
```

#### ApplicationMessageReceivedHandler
ApplicationMessageReceivedHandler是一個MqttApplicationMessageReceivedHandlerDelegate，是用來處理發送訊息的事件，
其delegate的method如下
```csharp
/// <summary>
/// 處理收到server傳來的訊息要怎麼處理
/// </summary>
/// <param name="e"></param>
private void OnMessageReceived(MqttApplicationMessageReceivedEventArgs e)
{
    var message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload ?? new byte[0]);
    // Do whatever you want
}
```
server端傳送的訊息都在`e.ApplicationMessage`內，`Topic`跟`Payload`都在裡面

#### ConnectedHandler
ConnectedHandler是一個MqttClientConnectedHandlerDelegate，是用來處理連線成功的事件，
其delegate的method如下
```csharp
/// <summary>
/// 處理客戶端連線成功時要處理的事情
/// </summary>
/// <param name="e"></param>
private void OnClientConnected(MqttClientConnectedEventArgs e)
{
    // Do whatever you want
}
```

#### DisconnectedHandler
DisconnectedHandler是一個MqttClientDisconnectedHandlerDelegate，是用來處理連線失敗的事件，
其delegate的method如下
```csharp
/// <summary>
/// 處理客戶端離線時要處理的事情，記得處理中斷狀態與server的exception
/// </summary>
/// <param name="e"></param>
private void OnClientDisconnected(MqttClientDisconnectedEventArgs e)
{
    // ClientWasConnected為true表示斷線是發生在連線成功後，反之表示連一開始的連線都沒有成功
    if (e.Exception != null && e.ClientWasConnected)
    {
        MessageBox.Show($"發生錯誤{Environment.NewLine}{e.Exception.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
    // TODO Do whatever you want
}
```
其中`e.Exception != null && e.ClientWasConnected`為連線成功後中斷的狀態

### Subscribe
向server發送訂閱請求
```csharp
/// <summary>
/// 向MQTT伺服器訂閱Topic
/// </summary>
/// <param name="topic">Topic名稱</param>
/// <returns></returns>
public async Task SubscribeAsync(string topic, Action<string> OnError)
{
    try
    {
        var topicFilter = new MqttTopicFilterBuilder()
        .WithTopic(topic)
        // Qos要大於0，客戶端才可收到離線訊息
        // AtMostOnce = 0, AtLeastOnce = 1, ExactlyOnce = 2
        .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce)
        .Build();
        await mqttClient.SubscribeAsync(topicFilter);
    }
    catch (Exception e)
    {
        OnError(e.ToString());
    }
}
```
### Publish
向server發佈訊息
```csharp
/// <summary>
/// 非同步發布訊息
/// </summary>
/// <param name="topic">向指定Topic發布</param>
/// <param name="chatMessage">發布訊息</param>
/// <returns></returns>
public async Task PublishAsync(string topic, string userInput, Action<string> OnError)
{
    try
    {
        // 將ChatMessage轉換成json字串
        ChatRoomMessage chatRoomMessage = new ChatRoomMessage { UserName = userName, ChatMessage = new ChatText(userInput), Topic = topic };
        //ChatMessage chatMessage = new ChatMessage() { UserName = userName, Message = userInput };
        var message = JsonConvert.SerializeObject(chatRoomMessage);
        var applicationMessage = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(Encoding.UTF8.GetBytes(message))
            // Qos要大於0，客戶端才可收到離線訊息
            // AtMostOnce = 0, AtLeastOnce = 1, ExactlyOnce = 2
            .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce)
            //.WithRetainFlag(true)
            .Build();
        await mqttClient.PublishAsync(applicationMessage);
    }
    catch (Exception e)
    {
        OnError(e.ToString());
    }
}
```

### 啟動Client
在clientOptions以及handler都設定好後，就可以啟動client了
```csharp
await mqttClient.ConnectAsync(options);
```
到這裡client就大功告成了!

### 實作
詳細實作方式請參閱[MQTTChatClientHandler.cs][MQTTChatClientHandler]與[MainPresenter.cs][MainPresenter]

[MQTTChatDemoWiki]: <https://github.com/JunHaoShih/MQTTnetDemo/wiki>
[MQTTChatHandler]: </MQTTBrokerConsole/MQTT/MQTTChatHandler.cs>
[MQTTChatClientHandler]: </MQTTChatClient/MQTT/MQTTChatClientHandler.cs>
[MainPresenter]: </MQTTChatClient/Presenter/MainPresenter.cs>

