using FluentValidation;
using System.Text.RegularExpressions;
using Vehicle.Domain.Entities;

namespace Vehicle.Domain.Validators
{
    public class MotoValidator : AbstractValidator<Moto>
    {
        public MotoValidator()
        {

            RuleFor(x => x.Plate)
             .NotEmpty().WithMessage("The vehicle plate can't be empty.")
             .When(x => (x.Country == "Brazil" || x.Country == "Brasil"))
             .Must(BeValidBrazilianPlate).WithMessage("The plate is not valid for Brazil.");
            RuleFor(x => x.Color)
                .NotEmpty().WithMessage("The vehicle color can't be empty.")
                .NotNull().WithMessage("The vehicle color can't be null.")
                .MaximumLength(20).WithMessage("The vehicle color must have a maximum of twenty characters.")
                .MinimumLength(2).WithMessage("The rider vehicle model must have at least two characters.");
            RuleFor(x => x.Model)
                .NotEmpty().WithMessage("The vehicle model can't be empty.")
                .NotNull().WithMessage("The vehicle model can't be null.")
                .MaximumLength(80).WithMessage("The rider vehicle model must have the maximum of eighty characters.")
                .MinimumLength(5).WithMessage("The vehicle model must have at least five characters.");
            RuleFor(x => x.Fabrication)
                .NotEmpty().WithMessage("The vehicle's manufacturing date cannot be empty.")
                .NotNull().WithMessage("The vehicle's manufacturing date cannot be null.")
                .Must(BeValidFabricationDate).WithMessage("The vehicle's manufacturing date is not valid. maximum 10 years of manufacture");

        }

        private bool BeValidBrazilianPlate(string plate)
        {
            // Expressão regular para validar placa de carro no formato brasileiro
            var regex = new Regex(@"^[A-Z]{3}\-\d{4}$");

            return regex.IsMatch(plate);
        }

        private bool BeValidFabricationDate(DateOnly date)
        {
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
            DateOnly minDate = currentDate.AddYears(-10);

            // Verifica se a data está entre a data mínima (10 anos atrás) e a data atual
            return date <= currentDate && date >= minDate;
        }

    }
}
