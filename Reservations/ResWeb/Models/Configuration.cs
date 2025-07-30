using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace ResWeb.Models
{
    public class Configuration
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
