using Order.Domain.Enums;
using Order.Domain.Exceptions;
using Order.Domain.Validators;

namespace Order.Domain.Entities
{
    public sealed record OrderLocation : ModelBase
    {

        public long VehicleId { get; set; }
        public long RiderId { get; set; }

        public DateOnly DateInit { get; set; }
        public DateOnly DateEnd { get; set; }
        public DateOnly DatePrev { get; set; }

        public EPlans Plan { get; set; }
        public decimal Value { get; set; }
        public decimal? FineRate { get; set; }
        public OrderLocation(long vehicleId, long riderId, DateOnly datePrev, EPlans plan)
        {
            DateInit = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
            VehicleId = vehicleId;
            RiderId = riderId;
            DatePrev = datePrev;
            Plan = plan;
            DateEnd = DateInit.AddDays((int)plan);
            CalculateRent();
        }

        public override bool Validate()
        {
            var validators = new OrderValidator();
            var validation = validators.Validate(this);
            if (!validation.IsValid)
            {
                foreach (var error in validation.Errors)
                {
                    _err?.Add(error.ErrorMessage);
                }
                throw new PersonalizeExceptions("Some errors are wrongs, please fix it and try again.", _err!);
            }
            return validation.IsValid;
        }

        private void CalculateRent()
        {
            Value = Plan switch
            {
                EPlans.Sete => 7 * 30,
                EPlans.Quinze => 15 * 28,
                EPlans.Trinta => 30 * 22,
                EPlans.QuarentaCinco => 45 * 20,
                EPlans.Cinquenta => 50 * 18,
                _ => 0
            };
        }

        public decimal CalculateFineRates(DateOnly datePreview)
        {
            DatePrev = datePreview;
            var days = DateEnd.DayNumber - DatePrev.DayNumber;

            // Se a data informada for inferior à data prevista do término
            if (days < 0)
            {
                FineRate = Plan switch
                {
                    EPlans.Sete => ((Value / (int)Plan) * 0.2m) * Math.Abs(days), // Plano de 7 dias: multa de 20% sobre diárias não efetivadas
                    _ => ((Value / (int)Plan) * 0.4m) * Math.Abs(days) // Outros planos: multa de 40% sobre diárias não efetivadas
                };
                return FineRate ?? 0;
            }
            // Se a data informada for superior à data prevista do término
            else if (days > 0)
            {
                // Calcular multa adicional de R$50,00 por diária adicional
                FineRate = 50 * days;
                return FineRate ?? 0;
            }
            // Se a data informada for igual à data prevista do término, não há multa
            return 0;
        }
    }
}
