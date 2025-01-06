using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopMVC2.Models
{
    [Table("Type")]
    public class Type
    {
        public int TypeId { get; set; }

        [Required]
        [MaxLength(50)]
        public string TypeTitle { get; set; }

        public List<Weapon> Weapons { get; set; }
    }
}
