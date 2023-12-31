namespace OrderService.Models.Dtos
{
    public class StripeRequestDto
    {
        public string? StripeSessionUrl { get; set; } // url of stripe, will open stripe for you
        public string? StripeSessionId { get; set; } // needed for payment validation
        public string ApprovedUrl { get; set; } // will be opened if payment was successful
        public string CancelUrl { get; set; } // will be opened if payment was not successful
        public Guid OrderId { get; set; } // selects order we are paying for
    }
}
