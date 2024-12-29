using System.Xml.Serialization;

namespace HopeAir.Domain.Models;

public class Flight
{
    [XmlElement("flightNumber", Namespace = "sky")]
    public string FlightNumber { get; set; }

    [XmlElement("departure", Namespace = "sky")]
    public string Departure { get; set; }

    [XmlElement("arrival", Namespace = "sky")]
    public string Arrival { get; set; }

    [XmlElement("price", Namespace = "sky")]
    public decimal Price { get; set; }

    [XmlElement("currency", Namespace = "sky")]
    public string Currency { get; set; }

    [XmlElement("duration", Namespace = "sky")]
    public string Duration { get; set; }

    [XmlElement("departureTime", Namespace = "sky")]
    public DateTime DepartureTime { get; set; }

    [XmlElement("arrivalTime", Namespace = "sky")]
    public DateTime ArrivalTime { get; set; }
}