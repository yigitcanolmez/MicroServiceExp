using MassTransit;

namespace MessageBroker.Interfaces
{
    public interface IStockReserverRequestPayment : CorrelatedBy<Guid>
    {
        public PaymentMessage Payment { get; set; }
        public List<OrderItemMessage> OrderItemMessages { get; set; }
        public string BuyerId { get; set; }
    }
}
