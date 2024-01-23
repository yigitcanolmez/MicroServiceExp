using MassTransit;

namespace MessageBroker.Interfaces
{
    public interface IPaymentCompletedEvent : CorrelatedBy<Guid>
    {


    }
}
