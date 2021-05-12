using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Book_Store.Models
{
    public class BookModel
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(60)]
        public string Title { get; set; }
        public DateTime BublishedAt { get; set; }


        [ForeignKey("Author")]
        public string AuthorId { get; set; }
        public virtual AuthorModel Author { get; set; }

        
        [ForeignKey("Category")]
        public string CategoryId { get; set; }
        public virtual CategoryModel Category { get; set; }
    }
}
