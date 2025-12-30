using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Enums
{
    public enum PaymentMethod
    {
        Unknown = 0, //Khong xac dinh
        Visa = 1, //The Visa
        MasterCard = 2, //The MasterCard
        PayPal = 3, //Thanh toan qua PayPal
        BankTransfer = 4, //Chuyen khoan ngan hang
        Momo = 5, //Vi dien tu MoMo
        ZaloPay = 6 //Vi dien tu ZaloPay
    }
}