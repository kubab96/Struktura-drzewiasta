using System.ComponentModel.DataAnnotations;

namespace TreeStructure.Models
{
    public class NodeUpsertDTO
    {
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
        public int? ParentId { get; set; }
    }
}
