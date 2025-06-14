using Microsoft.AspNetCore.SignalR;
using MQTTnet;
using MQTTnet.Client;
using FactoryPulse_Core.Hubs;

namespace FactoryPulse_Core.Services;

public class MqttService
{
    private readonly IHubContext<SensorHub> _hubContext;
    private IMqttClient? _mqttClient;
    private readonly ILogger<MqttService> _logger;

    public MqttService(IHubContext<SensorHub> hubContext, ILogger<MqttService> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task StartAsync()
    {
        try
        {
            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost", 1883)
                .WithClientId("DotNetSignalRBridge")
                .WithCleanSession(true)
                .Build();

            _mqttClient.ApplicationMessageReceivedAsync += OnMessageReceived;
            _mqttClient.ConnectedAsync += OnConnected;
            _mqttClient.DisconnectedAsync += OnDisconnected;

            await _mqttClient.ConnectAsync(options);
            
            await _mqttClient.SubscribeAsync("factory/+/+");
            
            _logger.LogInformation("Connected to MQTT broker and subscribed to factory topics");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to start MQTT service");
            throw;
        }
    }

    private Task OnConnected(MqttClientConnectedEventArgs e)
    {
        _logger.LogInformation("MQTT client connected successfully");
        return Task.CompletedTask;
    }

    private Task OnDisconnected(MqttClientDisconnectedEventArgs e)
    {
        _logger.LogWarning("MQTT client disconnected: {Reason}", e.Reason);
        return Task.CompletedTask;
    }

    private async Task OnMessageReceived(MqttApplicationMessageReceivedEventArgs e)
    {
        try
        {
            var topic = e.ApplicationMessage.Topic;
            var payload = System.Text.Encoding.UTF8.GetString(Array.Empty<byte>());

            var topicParts = topic.Split('/');
            if (topicParts.Length == 3 && topicParts[0] == "factory")
            {
                var machine = topicParts[1];
                var metric = topicParts[2];

                await _hubContext.Clients.All.SendAsync("SensorUpdate", machine, metric, payload);

                _logger.LogDebug("Forwarded: {Machine}/{Metric} = {Payload}", machine, metric, payload);

                var logDirectory = Path.Combine("Logs", "mqtt-logs");
                Directory.CreateDirectory(logDirectory);

                var logFilePath = Path.Combine(logDirectory, $"{machine}.log");

                using (var writer = new StreamWriter(logFilePath, append: true))
                {
                    await writer.WriteLineAsync($"{DateTime.UtcNow:O} - {machine}/{metric}: {payload}");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing MQTT message");
        }
    }


    public async Task StopAsync()
    {
        try
        {
            if (_mqttClient != null)
            {
                await _mqttClient.DisconnectAsync();
                _mqttClient.Dispose();
                _logger.LogInformation("MQTT service stopped");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error stopping MQTT service");
        }
    }
}