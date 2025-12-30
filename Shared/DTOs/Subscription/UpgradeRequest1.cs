using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.DTOs.Subscription
{
    public class UpgradeRequest1
    {
        public int PlanId { get; set; } //Id goi dang ki moi
        public string PaymentMethod { get; set; } = string.Empty; //Phuong thuc thanh toan
    }
}