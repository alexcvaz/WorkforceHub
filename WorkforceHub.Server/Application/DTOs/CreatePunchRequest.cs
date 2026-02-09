namespace WorkforceHub.Server.Application.DTOs
{
    public class CreatePunchRequest
    {
        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public double? Accuracy { get; set; }
    }
}
