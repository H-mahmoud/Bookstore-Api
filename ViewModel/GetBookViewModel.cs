using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Book_Store.ViewModel
{
    public class GetBookViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime BublishedAt { get; set; }

        public GetAuthorViewModel Author { get; set; }
        public GetCategoryViewModel Category { get; set; }
    }
}

