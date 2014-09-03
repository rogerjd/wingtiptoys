﻿using System.ComponentModel.DataAnnotations;   
using System.Collections.Generic;
using System.ComponentModel;

namespace WingtipToys.Models
{
    public class Order
    {
        public int OrderID { get; set; }

        public System.DateTime OrderDate { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string PostalCode { get; set; }

        public string Country { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public decimal Total { get; set; }

        public string PaymentTransactionID { get; set; }

        public bool HasBeenShipped { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }
    }
}