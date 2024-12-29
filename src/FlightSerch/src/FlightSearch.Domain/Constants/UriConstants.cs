namespace FlightSearch.Domain.Constants;

public static class UriConstants
{
    public static class HopeAirUrls
    {
        private const string HopeAirUrl = $"http://localhost:5164/api/v1/";
        public const string HopeAirFlightSearchUri = HopeAirUrl + "Flights/search";
    }

    public static class AybJetAirUrls
    {
        private const string AybJetUrl = $"http://localhost:5096/api/v1/";
        public const string AybJetFlightSearchUri = AybJetUrl + "Flights/search";
    }
}