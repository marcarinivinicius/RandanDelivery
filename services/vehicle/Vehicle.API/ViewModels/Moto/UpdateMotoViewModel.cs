namespace Vehicle.API.ViewModels.Moto;

public class UpdateMotoViewModel
{
    public long Id { get; set; }
    public string PlateCode { get; set; }
    public string Color { get; set; }
    public string Model { get; set; }
    public string Country { get; set; }
    public DateOnly Fabrication { get; set; }

}