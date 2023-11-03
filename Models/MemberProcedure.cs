using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VizGuideBackend.Models
{
    public class MemberProcedure
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? IsFunction { get; set; }
        [ForeignKey("BaseType")]
        public int BaseTypeId { get; set; }
        public string? Name { get; set; }
        public string? ReturnType { get; set; }
        public string? Signature { get; set; }
        public string? Description { get; set; }
    }
}
