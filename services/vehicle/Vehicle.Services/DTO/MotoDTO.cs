namespace Vehicle.Services.DTO
{
    public sealed record MotoDTO
    {
        public long Id { get; set; }
        public string PlateCode { get; set; }
        public string Color { get; set; }
        public string Model { get; set; }
        public string Country { get; set; }
        public DateOnly Fabrication { get; set; }

        protected MotoDTO() { }

        public MotoDTO(long id, string plateCode, string color, string model, string country, DateOnly fabrication)
        {
            Id = id;
            PlateCode = plateCode;
            Color = color;
            Model = model;
            Country = country;
            Fabrication = fabrication;
        }
    }
}
