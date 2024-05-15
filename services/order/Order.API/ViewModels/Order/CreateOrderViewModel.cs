using Order.Services.EnumsDTO;

namespace Order.API.ViewModels.Order
{
    public class CreateOrderViewModel
    {
        public EPlansDTO Plan { get; set; }
        public DateTime DatePrev { get; set; }

    }
}
