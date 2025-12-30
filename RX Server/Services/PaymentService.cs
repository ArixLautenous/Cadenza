using Shared.Enums;

namespace RX_Server.Services
{
    public class PaymentService : IPaymentService
    {
        public Task<bool> ProcessPayment(int userId, decimal amount, PaymentMethod method)
        {
            //Gia lap logic ket noi den cong thanh toan ben thu 3
            if(amount <= 0)
                return Task.FromResult(false); //So tien khong hop le
            //Gia lap do tre mang
            Thread.Sleep(500);

            switch (method)
            {
                case PaymentMethod.Visa:
                case PaymentMethod.MasterCard:
                case PaymentMethod.Momo:
                case PaymentMethod.ZaloPay:
                case PaymentMethod.PayPal:
                case PaymentMethod.BankTransfer:
                    //Goi API ben thu 3 o day de xu ly thanh toan
                    //Gia lap thanh toan thanh cong
                    return Task.FromResult(true);
                case PaymentMethod.Unknown:
                    default:
                    //Phuong thuc thanh toan khong hop le
                    return Task.FromResult(false);

            }
        }
    }
}
