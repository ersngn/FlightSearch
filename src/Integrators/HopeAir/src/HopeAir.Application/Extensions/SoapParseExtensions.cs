using System.Text;
using System.Xml;
using System.Xml.Serialization;
using HopeAir.Domain.Models;

namespace HopeAir.Application.Extensions;

public static class SoapParseExtensions
{
    public static List<Flight> ParseSoapResponse(string soapResponse)
    {
        if (string.IsNullOrWhiteSpace(soapResponse))
            throw new ArgumentException("SOAP response cannot be null or empty.", nameof(soapResponse));

        var flightDataList = new List<Flight>();

        try
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(soapResponse);

            var flightNodes = xmlDoc.GetElementsByTagName("sky:flight");

            flightDataList.AddRange(from XmlNode node in flightNodes
            select new Flight()
            {
                FlightNumber = GetNodeInnerText(node, "sky:flightNumber"),
                Departure = GetNodeInnerText(node, "sky:departure"),
                Arrival = GetNodeInnerText(node, "sky:arrival"),
                Price = ParseDecimal(GetNodeInnerText(node, "sky:price"), 0),
                Currency = GetNodeInnerText(node, "sky:currency"),
                Duration = GetNodeInnerText(node, "sky:duration"),
                DepartureTime = ParseDateTime(GetNodeInnerText(node, "sky:departureTime")),
                ArrivalTime = ParseDateTime(GetNodeInnerText(node, "sky:arrivalTime"))
            });
        }
        catch (XmlException ex)
        {
            throw new InvalidOperationException("Failed to parse the SOAP response XML.", ex);
        }

        return flightDataList;
    }
    
    public static string CreateSoapEnvelope<T>(T requestObject, string rootElementName, string namespaceUrl)
    {
        if (requestObject == null)
            throw new ArgumentNullException(nameof(requestObject), "Request object cannot be null.");

        if (string.IsNullOrWhiteSpace(rootElementName))
            throw new ArgumentException("Root element name cannot be null or empty.", nameof(rootElementName));

        if (string.IsNullOrWhiteSpace(namespaceUrl))
            throw new ArgumentException("Namespace URL cannot be null or empty.", nameof(namespaceUrl));

        var soapEnvelope = new StringBuilder();
        soapEnvelope.AppendLine(@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:sky=""" + namespaceUrl + @""">");
        soapEnvelope.AppendLine("   <soapenv:Header/>");
        soapEnvelope.AppendLine("   <soapenv:Body>");
        soapEnvelope.AppendLine($"      <sky:{rootElementName}>");
        soapEnvelope.Append(SerializeToXml(requestObject));
        soapEnvelope.AppendLine($"      </sky:{rootElementName}>");
        soapEnvelope.AppendLine("   </soapenv:Body>");
        soapEnvelope.AppendLine("</soapenv:Envelope>");

        return soapEnvelope.ToString();
    }

    private static string SerializeToXml<T>(T obj)
    {
        var xmlSerializer = new XmlSerializer(typeof(T));
        using (var stringWriter = new StringWriter())
        using (var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { OmitXmlDeclaration = true }))
        {
            xmlSerializer.Serialize(xmlWriter, obj);
            return stringWriter.ToString();
        }
    }

    private static string GetNodeInnerText(XmlNode parentNode, string nodeName)
    {
        return parentNode[nodeName]?.InnerText ?? string.Empty;
    }

    private static decimal ParseDecimal(string value, decimal fallback)
    {
        return decimal.TryParse(value, out var result) ? result : fallback;
    }

    private static DateTime ParseDateTime(string value, DateTime? fallback = null)
    {
        return DateTime.TryParse(value, out var result) ? result : fallback ?? DateTime.MinValue;
    }
}