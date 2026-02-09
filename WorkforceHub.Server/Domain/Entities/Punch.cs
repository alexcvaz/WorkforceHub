using System.ComponentModel.DataAnnotations;
using WorkforceHub.Server.Domain.Enums;

namespace WorkforceHub.Server.Domain.Entities
{
    public class Punch
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public DateTimeOffset Timestamp { get; set; }

        [Required]
        public PunchType Type { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public double? Accuracy { get; set; }

        public string? IpAddress { get; set; }

        public string? UserAgent { get; set; }

        public Punch()
        {
        }

        public Punch(string userId, DateTimeOffset timestamp, PunchType type)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Timestamp = timestamp;
            Type = type;
        }
    }
}
