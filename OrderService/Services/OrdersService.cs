using Microsoft.EntityFrameworkCore;
using OrderService.Models.Dtos;
using Stripe;
using OrderService.Data;
using Stripe.Checkout;
using BlogsMessageBus;
using OrderService.Models;
using Microsoft.Extensions.Options;

namespace OrderService.Services.IServices
{
    public class OrdersService : IOrder
    {
        private readonly ApplicationDbContext _context;
        private readonly IUser _userService;
        private readonly IMessageBus _messageBus;
        private readonly CartItemService _cartItemService;
        private readonly CartsService _cartService;
        public OrdersService(ApplicationDbContext context, IUser user, IMessageBus messageBus, CartItemService cartItemService , CartsService cartService)
        {
            _context = context;
            _userService = user;
            _messageBus = messageBus;
            _cartItemService = cartItemService;
            _cartService = cartService;

        }

        public async Task<string> MakeOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return "Order made successfully";
        }

        public async Task<Order> GetOrderByUserId(Guid userId)
        {
            return await _context.Orders.Where(x => x.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<StripeRequestDto> MakePayments(StripeRequestDto stripeRequestDto)
        {
            var order = await _context.Orders.Where(x => x.OrderId == stripeRequestDto.OrderId).FirstOrDefaultAsync();
            var orderProducts = await _context.OrderProducts.Where(x => x.OrderId == order.OrderId).ToListAsync();
            var options = new SessionCreateOptions()
            {
                SuccessUrl = stripeRequestDto.ApprovedUrl,
                CancelUrl = stripeRequestDto.CancelUrl,
                Mode = "payment",
                LineItems = new List<SessionLineItemOptions>()
            };

            foreach (var orderProduct in orderProducts)
            {
                var cartItem = _cartItemService.GetCartItemById(orderProduct.CartItemId);
                var item = new SessionLineItemOptions()
                {
                    PriceData = new SessionLineItemPriceDataOptions()
                    {
                        UnitAmount = (long)order.OrderTotal * 100,
                        Currency = "kes",

                        ProductData = new SessionLineItemPriceDataProductDataOptions()
                        {
                            Name = cartItem.ProductName,
                            Images = new List<string> { "https://imgs.search.brave.com/av4uh1BAXrv7q2gkJt-E709vrIz3mB1-wrcPDtDyZNI/rs:fit:500:0:0/g:ce/aHR0cHM6Ly93d3cu/ZXhwZXJ0YWZyaWNh/LmNvbS9pbWFnZXMv/YXJlYS8xODI5X2wu/anBn" }
                        }
                     },
                        Quantity = cartItem.Quantity


                    };

                options.LineItems.Add(item);
            }
            //discount
            var cart = _cartService.GetCartById(cartItem.CartId);
            var DiscountObj = new List<SessionDiscountOptions>()
            {
                new SessionDiscountOptions()
                {
                    Coupon=cart.CouponCode
                }
            };

            if (cart.Discount > 0)
            {
                options.Discounts = DiscountObj;

            }
            var service = new SessionService();
            Session session = service.Create(options);

            // URL//ID

            stripeRequestDto.StripeSessionUrl = session.Url;
            stripeRequestDto.StripeSessionId = session.Id;

            //update Database =>status/ SessionId 

            order.StripeSessionId = session.Id;
            order.Status = "Ongoing";
            await _context.SaveChangesAsync();

            return stripeRequestDto;
        }

            
    


        public async Task<bool> ValidatePayments(Guid OrderId)
        {
            var order = await _context.Orders.Where(x => x.OrderId == OrderId).FirstOrDefaultAsync();

            var service = new SessionService();
            Session session = service.Get(order.StripeSessionId);

            PaymentIntentService paymentIntentService = new PaymentIntentService();

            PaymentIntent paymentIntent = paymentIntentService.Get(session.PaymentIntentId);

            if (paymentIntent.Status == "succeeded")
            {
                //the payment was success

                order.Status = "Paid";
                order.PaymentIntent = paymentIntent.Id;
                await _context.SaveChangesAsync();
                //payment validation was a success
                var user = await _userService.GetUserById(order.UserId.ToString());

                if (string.IsNullOrWhiteSpace(user.Email))
                {
                    return false;
                }
                else
                {
                    var reward = new RewardsDto()
                    {
                        OrderId = order.OrderId,
                        OrderTotal = order.OrderTotal,
                        Name = user.Name,
                        Email = user.Email

                    };
                    
                    //send this reward to our topic
                    await _messageBus.PublishMessage(reward, "orderadded");
                }
                //Send an email to user
                //Reward user with some bonga points
                return true;
            }
            return false;
        }
    }
}
