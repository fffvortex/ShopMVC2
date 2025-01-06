using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopMVC2.Models
{
    [Table("Weapon")]
    public class Weapon
    {
        public short WeaponId { get; set; }
        [Required]
        [MaxLength(100)]
        public string? WeaponTitle { get; set; }
        [Required]
        public short WeaponTypeId { get; set; }
        public Type Type { get; set; }
    }
}
