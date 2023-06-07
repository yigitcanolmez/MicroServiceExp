﻿using System.ComponentModel.DataAnnotations.Schema;

namespace OrderAPI.Models
{
    public class OrderItem
    {

        // TO DO int to GUID
        public int Id { get; set; }
        public int ProductId { get; set; }
        [Column(TypeName =  "decimal(18,2)")]
        public decimal Price { get; set; }
        public Order Order { get; set; }

        public int OrderId { get; set; }
        public int Count { get; set; }


    }
}
