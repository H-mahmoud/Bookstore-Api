using System.ComponentModel.DataAnnotations;

namespace Book_Store.ViewModel
{
    public class PostAuthorViewModel
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
    }
}
