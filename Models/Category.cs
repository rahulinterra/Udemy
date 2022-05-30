using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CRUD.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [DisplayName("Display Order")]
        [Required]
        /*[Range(1,int.MaxValue,ErrorMessage ="Display Order Category must be greater than 0")]*/
        public string DisplayOrder { get; set; }
    }
}
