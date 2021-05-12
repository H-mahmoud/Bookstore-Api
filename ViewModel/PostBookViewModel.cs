using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Book_Store.ViewModel
{
    public class PostBookViewModel
    {
        [Required]
        [MaxLength(60)]
        public string Title { get; set; }

        public DateTime BublishedAt { get; set; }
        
        public string AuthorId { get; set; }

        public string CategoryId { get; set; }

    }
}
