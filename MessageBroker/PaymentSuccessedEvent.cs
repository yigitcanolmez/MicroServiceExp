﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBroker
{
    public class PaymentSuccessedEvent
    {
        public int OrderId { get; set; }
        public string BuyerId { get; set; }
    }  
}
