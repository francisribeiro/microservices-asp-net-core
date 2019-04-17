using LocationReporter.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;

namespace LocationReporter.Events
{
    public class AMQPEventEmitter : IEventEmitter
    {
        private readonly ILogger _logger;
        private AMQPOptions _rabbitOptions;
        private ConnectionFactory _connectionFactory;
        public const string QUEUE_LOCATIONRECORDED = "memberlocationrecorded";

        public AMQPEventEmitter(
            ILogger<AMQPEventEmitter> logger,
            IOptions<AMQPOptions> amqpOptions)
        {
            _logger = logger;
            _rabbitOptions = amqpOptions.Value;

            _connectionFactory = new ConnectionFactory
            {
                UserName = _rabbitOptions.Username,
                Password = _rabbitOptions.Password,
                VirtualHost = _rabbitOptions.VirtualHost,
                HostName = _rabbitOptions.HostName,
                Uri = _rabbitOptions.Uri
            };

            logger.LogInformation("AMQP Event Emitter configured with URI {0}", _rabbitOptions.Uri);
        }

        public void EmitLocationRecordedEvent(MemberLocationRecordedEvent locationRecordedEvent)
        {
            using (IConnection conn = _connectionFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: QUEUE_LOCATIONRECORDED,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );

                    string jsonPayload = locationRecordedEvent.ToJson();
                    var body = Encoding.UTF8.GetBytes(jsonPayload);

                    channel.BasicPublish(
                        exchange: "",
                        routingKey: QUEUE_LOCATIONRECORDED,
                        basicProperties: null,
                        body: body
                    );
                }
            }
        }
    }
}
