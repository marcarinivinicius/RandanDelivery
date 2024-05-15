using Order.Services.EnumsDTO;

namespace Order.API.ViewModels.Order
{
    public class CancelOrderViewModel
    {
        public long Id { get; set; }
        public DateTime DatePrev { get; set; }
    }
}
