using MessageBroker.Interfaces;

namespace MessageBroker
{
    public class PaymentFailedEvent : IPaymentFailedEvent
    {
        public PaymentFailedEvent(Guid CorrelationId)
        {
            this.CorrelationId = CorrelationId;
        }
        public string Reason { get; set; }
        public Guid CorrelationId { get; }
        public List<OrderItemMessage> OrderItems { get; set; }
    }
}
