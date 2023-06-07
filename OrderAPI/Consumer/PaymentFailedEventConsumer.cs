using MassTransit;
using MessageBroker;

namespace OrderAPI.Consumer
{
    public class PaymentFailedEventConsumer : IConsumer<PaymentFailedEvent>
    {
        public Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            throw new NotImplementedException();
        }
    }
}
