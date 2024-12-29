namespace HopeAir.Domain.Models;

public class SoapSearchFlightRequest
{
    public string Origin { get; set; }
    public string Destination { get; set; }
    public DateTime Date { get; set; }
    public int PassengerCount { get; set; }
}