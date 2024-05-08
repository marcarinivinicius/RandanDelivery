using Vehicle.Domain.Exceptions;
using Vehicle.Domain.Validators;

namespace Vehicle.Domain.Entities
{
    public sealed record Moto : ModelBase
    {

        public string PlateCode { get; set; }
        public string Color { get; set; }
        public string Model { get; set; }
        public string Country { get; set; }
        public DateOnly Fabrication { get; set; }

        protected Moto() { }

        public Moto(string plateCode, string color, string model, string country, DateOnly fabrication)
        {
            PlateCode = plateCode;
            Color = color;
            Model = model;
            Country = country;
            Fabrication = fabrication;
        }

        public override bool Validate()
        {
            var validators = new MotoValidator();
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
    }
}
