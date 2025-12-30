using Shared.Enums;

namespace RX_Server.Services
{
    public interface IPaymentService
    {
        //Xu ly thanh toan, tra ve ket qua thanh toan
        Task<bool> ProcessPayment(int userId, decimal amount, PaymentMethod method);
    }
}