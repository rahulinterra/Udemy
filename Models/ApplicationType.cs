using System.ComponentModel.DataAnnotations;

namespace CRUD.Models
{
    public class ApplicationType
    {
        [Key]
        public int id { get; set; }

        public string name { get; set; }
    }
}

