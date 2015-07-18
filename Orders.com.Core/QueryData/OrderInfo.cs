﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.com.Core.QueryData
{
    public class OrderInfo
    {
        public long OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; }
        public long CustomerID { get; set; }
        public decimal Total { get; set; }
        public long StatusID { get; set; }
        public string Status { get; set; }
        public DateTime? SubmittedOn { get; set; }
    }
}
