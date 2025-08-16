using Humanizer;
using System.ComponentModel.DataAnnotations;

namespace ResWeb.Models
{
    public class ScheduleDateTime
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ScheduleId { get; set; }
        public string CronExpression { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public int? MaxParticipants { get; set; } // Per day, not per schedule.
    }
}
