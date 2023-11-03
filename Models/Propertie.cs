using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VizGuideBackend.Models
{
    public class Propertie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ReturnType { get; set; }
        [ForeignKey("BaseType")]
        public int BaseTypeId { get; set; }
        public string? IsReadOnly { get; set; }
        public string? Description { get; set; }

    }
}
