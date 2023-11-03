using System.ComponentModel.DataAnnotations;

namespace VizGuideBackend.Models
{
    public class Script
    {

        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Code { get; set; }
    }
}
