using System.ComponentModel.DataAnnotations;

namespace ResWeb.Models
{
    public class Schedule
    {
        #region DBContext Columns

        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }

        #endregion

        public List<ScheduleDateTime> DateTimes { get; set; } = new List<ScheduleDateTime>();
    }
}
