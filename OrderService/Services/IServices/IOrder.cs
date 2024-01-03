using OrderService.Models;
using OrderService.Models.Dtos;
using Stripe;

namespace OrderService.Services.IServices
{
    public interface IOrder
    {
        Task<string> MakeOrder(Order order);
        Task<Order> GetOrderByUserId(Guid userId);
        Task<StripeRequestDto> MakePayments(StripeRequestDto stripeRequestDto, string token);
        Task<bool> ValidatePayments(Guid OrderId, string token);
    }
}
