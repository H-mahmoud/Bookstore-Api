using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Book_Store.Models
{
    public class AuthorModel
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        public virtual ICollection<BookModel> Books{ get; set; }
    }
}
