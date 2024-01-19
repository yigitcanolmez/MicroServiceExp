namespace MessageBroker
{
    public class PaymentSuccessedEvent
    {
        public int OrderId { get; set; }
        public string BuyerId { get; set; }
    }  
}
