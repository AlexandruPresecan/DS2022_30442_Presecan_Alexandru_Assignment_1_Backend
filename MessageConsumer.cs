using DS2022_30442_Presecan_Alexandru_Assignment_1.DTOs;
using DS2022_30442_Presecan_Alexandru_Assignment_1.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace DS2022_30442_Presecan_Alexandru_Assignment_1_Backend
{
    public class MessageConsumer
    {
        private readonly EnergyConsumptionService _energyConsumptionService;

        public MessageConsumer(EnergyConsumptionService energyConsumptionService)
        {
            _energyConsumptionService = energyConsumptionService;
        }

        public void Run()
        {
            ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost" };
            IConnection connection = factory.CreateConnection();
            IModel channel = connection.CreateModel();

            channel.QueueDeclare(queue: "sensor",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            consumer.Received += (o, a) =>
                {
                    string message = Encoding.UTF8.GetString(a.Body.ToArray());
                    EnergyConsumptionDTO? energyConsumption = JsonSerializer.Deserialize<EnergyConsumptionDTO>(message);

                    if (energyConsumption != null)
                        try
                        {
                            _energyConsumptionService.CreateEnergyConsumption(energyConsumption);
                        }
                        catch
                        {

                        }
                };

            channel.BasicConsume(queue: "sensor",
                                 autoAck: false,
                                 consumer: consumer);
        }
    }
}
