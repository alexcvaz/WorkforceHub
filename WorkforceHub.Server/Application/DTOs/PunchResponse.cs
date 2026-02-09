using WorkforceHub.Server.Domain.Enums;

namespace WorkforceHub.Server.Application.DTOs
{
    public class PunchResponse
    {
        public Guid Id { get; set; }

        public DateTimeOffset Timestamp { get; set; }

        public PunchType Type { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public double? Accuracy { get; set; }

        public string? IpAddress { get; set; }
    }
}
