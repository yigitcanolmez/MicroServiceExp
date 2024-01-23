using MessageBroker.Interfaces;

namespace MessageBroker
{
    public class PaymentSuccessedEvent : IPaymentCompletedEvent
    {
        public PaymentSuccessedEvent(Guid CorrelationId)
        {
            this.CorrelationId = CorrelationId;
        }

        public Guid CorrelationId { get; }
    }
}
