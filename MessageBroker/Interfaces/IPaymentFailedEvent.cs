using MassTransit;

namespace MessageBroker.Interfaces
{
    public interface IPaymentFailedEvent : CorrelatedBy<Guid>
    {
        public List<OrderItemMessage> OrderItems { get; set; }
        public string Reason { get; set; }
    }
}
