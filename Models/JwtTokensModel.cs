using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Book_Store.Models
{
    public class JwtTokensModel
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public DateTime ExpirationDate { get; set; }

        [Required]
        public virtual UserModel User { get; set; }

    }
}
