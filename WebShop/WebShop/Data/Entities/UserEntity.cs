using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.Data.Entities
{
    [Table("tblUsers")]
    public class UserEntity : BaseEntity<int>
    {
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        [StringLength(4000)]
        public string Description { get; set; }
        [StringLength(250)]
        public string Image { get; set; }
    }

}
