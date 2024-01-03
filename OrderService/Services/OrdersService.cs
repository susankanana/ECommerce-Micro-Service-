using Microsoft.EntityFrameworkCore;
using OrderService.Models.Dtos;
using Stripe;
using OrderService.Data;
using Stripe.Checkout;
using EcommerceMessageBus;
using OrderService.Models;
using Microsoft.Extensions.Options;

namespace OrderService.Services.IServices
{
    public class OrdersService : IOrder
    {
        private readonly ApplicationDbContext _context;
        private readonly IUser _userService;
        private readonly IMessageBus _messageBus;
        private readonly ICart _cartService;
        public OrdersService(ApplicationDbContext context, IUser user, IMessageBus messageBus, ICart cartService)
        {
            _context = context;
            _userService = user;
            _messageBus = messageBus;
            _cartService = cartService;

        }

        public async Task<string> MakeOrder(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return "Order made successfully";
        }

        public async Task<Order> GetOrderByUserId(Guid userId)
        {
            return await _context.Orders.Where(x => x.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<StripeRequestDto> MakePayments(StripeRequestDto stripeRequestDto , string token)
        {
            var order = await _context.Orders.Where(x => x.OrderId == stripeRequestDto.OrderId).FirstOrDefaultAsync();
            var cart = await _cartService.GetCartByUserId(order.UserId , token);
            var options = new SessionCreateOptions()
            {
                SuccessUrl = stripeRequestDto.ApprovedUrl,
                CancelUrl = stripeRequestDto.CancelUrl,
                Mode = "payment",
                LineItems = new List<SessionLineItemOptions>()
            };

            foreach (var cartItem in cart.Items)
            {
                var item = new SessionLineItemOptions()
                {
                    PriceData = new SessionLineItemPriceDataOptions()
                    {
                        UnitAmount = (long) cartItem.ProductPrice * 100,
                        Currency = "kes",

                        ProductData = new SessionLineItemPriceDataProductDataOptions()
                        {
                            Name = cartItem.ProductName,
                            Images = new List<string> { "https://images.unsplash.com/photo-1564557287817-3785e38ec1f5?q=80&w=1000&auto=format&fit=crop&ixlib=rb-4.0.3&ixid=M3wxMjA3fDB8MHxzZWFyY2h8NHx8aG9vZGllfGVufDB8fDB8fHww" }
                        }
                     },
                        Quantity = cartItem.Quantity


                    };

                options.LineItems.Add(item);
            }
            //discount
            
            var DiscountObj = new List<SessionDiscountOptions>()
            {
                new SessionDiscountOptions()
                {
                    Coupon=order.CouponCode
                }
            };

            if (order.CouponDiscount > 0)
            {
                options.Discounts = DiscountObj;

            }
            //service receives a good documentation of what we are paying for
            var service = new SessionService();
            Session session = service.Create(options); //an instance of a session that will come with more info that we need to open stripe.

            // info given back to us will be a URL and ID

            stripeRequestDto.StripeSessionUrl = session.Url;
            stripeRequestDto.StripeSessionId = session.Id;

            // finally update the Database => status should move from pending / pass SessionId above

            order.StripeSessionId = session.Id;
            order.Status = "Ongoing";
            await _context.SaveChangesAsync();

            return stripeRequestDto;
        }

            
    


        public async Task<bool> ValidatePayments(Guid OrderId, string token)
        {
            var order = await _context.Orders.Where(x => x.OrderId == OrderId).FirstOrDefaultAsync();
            var cart = await _cartService.GetCartByUserId(order.UserId, token);

            var service = new SessionService();
            Session session = service.Get(order.StripeSessionId); //select session we want to validate

            PaymentIntentService paymentIntentService = new PaymentIntentService();

            PaymentIntent paymentIntent = paymentIntentService.Get(session.PaymentIntentId);

            if (paymentIntent.Status == "succeeded")
            {
                //the payment was success

                order.Status = "Paid";
                order.PaymentIntent = paymentIntent.Id;  //can track the payment
                await _context.SaveChangesAsync();
                // await _cartService.DeleteCart(cart.CartId, token);
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
